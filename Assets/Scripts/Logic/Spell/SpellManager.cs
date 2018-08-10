/**************************************************
Copyright (C) 2018 Sakura Studio 版权所有
Author: Peng Dongwei
CreateTime: 8/1/2018 12:21:00 PM
Note : 法术管理
***************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XFramework;
/// <summary>
/// 技能管理器
/// </summary>
public class SpellManager : ISingleton<SpellManager> {
    public List<Spell> m_SpellList= new List<Spell>();

    public void Init() {
        ShowManager.Instance().Add("初始化主角技能.......");
        int[]spells =  GameConfig.PlayerInitSpells;
        foreach (var v in spells) {
            Add(v);
        }
    }

    /// <summary>
    /// 是否某个仙法已经学习了
    /// </summary>
    /// <param name="spellId"></param>
    /// <returns></returns>
    public bool IsLearned(int spellId) {
        foreach(var spell in m_SpellList)
        {
            if (spell.m_Id == spellId) return true;
        }
        return false;
    }

    /// <summary>
    /// 撤销修炼
    /// </summary>
    /// <param name="spell"></param>
    public void Free(Spell spell) {
        spell.m_Status = Spell.Status.free;

        ShowManager.Instance().Add(string.Format("撤销修炼武功：{0}", spell.m_Data.Name));
    }

    /// <summary>
    /// 修炼
    /// </summary>
    /// <param name="spell"></param>
    public void Learn(Spell spell)
    {
        spell.m_Status = Spell.Status.learning;
        ShowManager.Instance().Add(string.Format("开始修炼武功：{0}", spell.m_Data.Name));
    }

    /// <summary>
    /// 学习新的技能
    /// </summary>
    /// <param name="spellId">技能ID</param>
    public void Add(int spellId) {
        if (!IsLearned(spellId))
        {
            Spell spell = new Spell();
            spell.m_Id = spellId;
            spell.m_Lv = 1;
            spell.m_Data = LocalDataManager.Instance().m_Spell_Dict[spellId];
            m_SpellList.Add(spell);
            ShowManager.Instance().Add(string.Format("获得新武功：{0}", spell.m_Data.Name));
        }
    }

    /// <summary>
    /// 获得正在修炼的仙法
    /// </summary>
    /// <returns></returns>
    public List<Spell> GetLearningList() {
        return m_SpellList.FindAll((spell)=>spell.m_Status == (Spell.Status.learning));
    }

    public void SpellLvUp(Spell spell) {
        spell.m_Lv++;
        ShowManager.Instance().Add(string.Format("{0}：修炼完毕，等级得到上升", spell.m_Data.Name));
    }

    /// <summary>
    /// 随机一个技能
    /// </summary>
    /// <returns></returns>
    public Spell RandomSpell() {
        Spell spell = m_SpellList[UnityEngine.Random.Range(0, m_SpellList.Count)];
        return spell;
    }

    /// <summary>
    /// 解析技能
    /// </summary>
    public void ParseSpell(BattleEntity caster, BattleEntity target, Spell spell) {
        if (spell == null) return;
        //技能输出
        int skillTextId = UnityEngine.Random.Range(1, 5);
        string skillText = string.Empty;
        if (skillTextId == 1)
        {
            skillText = spell.m_Data.SkillText1;
        }
        else if (skillTextId == 2)
        {
            skillText = spell.m_Data.SkillText2;
        }
        else if (skillTextId == 3)
        {
            skillText = spell.m_Data.SkillText3;
        }
        else if (skillTextId == 4)
        {
            skillText = spell.m_Data.SkillText4;
        }
        string[] skillNameArray = spell.m_Data.Name.Split(';');
        string skillName = skillNameArray[UnityEngine.Random.Range(0, skillNameArray.Length)];

        DialogManager.Instance().Wirte(skillText, caster.m_Name, target.m_Name, skillName);
    }
}
