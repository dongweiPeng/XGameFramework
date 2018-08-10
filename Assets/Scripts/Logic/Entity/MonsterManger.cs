/**************************************************
Copyright (C) 2018 Sakura Studio 版权所有
Author: Peng Dongwei
CreateTime: 8/2/2018 12:08:35 PM
Note :
***************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XFramework;
public class MonsterManger : ISingleton<MonsterManger> {
    private Dictionary<int/*事件id*/, List<dbc.Monster>> Monster_Pool = new Dictionary<int, List<dbc.Monster>>();
    private Monster m_MonsterInstace;

    /// <summary>
    /// 根据事件随机一个怪物，随机怪物池会缓存下来
    /// </summary>
    /// <param name="evt"></param>
    /// <returns></returns>
    public Monster RandomMonster(dbc.Event evt) {
        m_MonsterInstace = m_MonsterInstace ?? new Monster();
        dbc.Section section = null;
        section = LocalDataManager.Instance().m_Map_Dict[evt.SectionId];
        List<dbc.Monster> monsterList = null;
        if (Monster_Pool.ContainsKey(evt.ID))
        {
            monsterList = Monster_Pool[evt.ID];
        }
        else
        {
            string[] pool = section.MonsterPool.Split(';');
            monsterList = new List<dbc.Monster>();
            foreach (string monsterId in pool) {
                dbc.Monster data = LocalDataManager.Instance().m_Monster_Dict[int.Parse(monsterId)];
                monsterList.Add(data);
            }
            Monster_Pool.Add(evt.ID, monsterList);
        }

        dbc.Monster randomMonster = monsterList[Random.Range(0, monsterList.Count)];
        dbc.MonsterAttr randomMonsterAttr = LocalDataManager.Instance().m_MonserAttr_Dict[randomMonster.attrid];
      
        Spell spell = new Spell();
        spell.m_Data = LocalDataManager.Instance().m_Spell_Dict[randomMonster.skillid];
        spell.m_Lv = randomMonster.skillLv;
     
        float offset = section.Lv * section.Stength * 0.0001f;
        m_MonsterInstace.m_Hp = (int)(randomMonsterAttr.HP * offset);
        m_MonsterInstace.m_Mp= (int)(randomMonsterAttr.MP * offset);
        m_MonsterInstace.m_Fight = (int)(randomMonsterAttr.Damage * offset);
        m_MonsterInstace.m_Hit = (int)(randomMonsterAttr.Hit * offset);
        m_MonsterInstace.m_Defence = (int)(randomMonsterAttr.Block * offset);
        m_MonsterInstace.m_Dodge = (int)(randomMonsterAttr.Dodge * offset);
        m_MonsterInstace.m_Data = randomMonster;
        m_MonsterInstace.m_Spell = spell;

        Debug.Log(string.Format("随机了一个{0} 怪物，技能{1}", m_MonsterInstace.m_Data.name, spell.m_Data.Name));
        return m_MonsterInstace;
    }
}
