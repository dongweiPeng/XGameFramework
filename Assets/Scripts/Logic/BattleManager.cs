/**************************************************
Copyright (C) 2018 Sakura Studio 版权所有
Author: Peng Dongwei
CreateTime: 8/1/2018 1:24:03 PM
Note : 战斗管理
***************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XFramework;

public class BattleManager : ISingleton<BattleManager>
{
    BattleEntity m_Player;
    BattleEntity m_Monster;

    public BattleEntity Player
    {
        get
        {
            return m_Player;
        }
    }

    public BattleEntity Monster
    {
        get
        {
            return m_Monster;
        }
    }
    Player player;
    public void Init(System.Action callback)
    {
        //初始化玩家数据
        RecovryPlayer();
        m_Monster = new BattleEntity();
        //初始化地图
        MapManager.Instance().Init();
        MapManager.Instance().ChangeMap(GameConfig.MapProgress);

        callback();
    }


    private void RecovryPlayer()
    {
        player = player ?? new Player();
        m_Player = m_Player ?? new BattleEntity();
        AttributeManager.Instance().Init(player, player.m_AttrPoint);
        m_Player.m_MaxHp = m_Player.m_Hp = AttributeManager.Instance().Hp();
        m_Player.m_MaxMp = m_Player.m_Mp = AttributeManager.Instance().Mp();
        m_Player.m_Fight = AttributeManager.Instance().Fight();
        m_Player.m_Defence = AttributeManager.Instance().Defence();
        m_Player.m_Dodge = AttributeManager.Instance().Dodge();
        m_Player.m_Name = player.m_Name;
        m_Player.m_Hit = player.m_Hit;
        m_Player.m_Type = EntityType.player;
        m_Player.m_Lv = player.m_Lv;
    }

    public dbc.Event RandomEvent()
    {
        dbc.Event evt = EventManager.Instance().GetEvent();
        string content = string.Empty;
        if (EventCanExecute(evt))
        {
            if (Random.Range(0, 101) <= evt.ExeRate)
            {
                content = evt.SucText;
                if (evt.Type == 1)
                {
                    Monster monster = MonsterManger.Instance().RandomMonster(evt);
                    m_Monster.m_MaxHp = m_Monster.m_Hp = monster.m_Hp;
                    m_Monster.m_MaxMp = m_Monster.m_Mp = monster.m_Mp;
                    m_Monster.m_Fight = monster.m_Fight;
                    m_Monster.m_Defence = monster.m_Defence;
                    m_Monster.m_Dodge = monster.m_Dodge;
                    m_Monster.m_Hit = monster.m_Hit;
                    m_Monster.m_Name = monster.m_Data.name;
                    m_Monster.m_Type = EntityType.enemy;
                    m_Monster.m_Spell = monster.m_Spell;
                    m_Monster.m_Lv = monster.m_Lv;
                }
            }
            else
            {
                content = evt.FailText;
            }
        }
        else
        {
            content = evt.FailText;
        }

        string[] texts = content.Split(';');
        for (int i = 0; i < texts.Length; i++)
        {
            string text = texts[i];

            if (text.Contains("{0}") && text.Contains("{1}"))
            {
                if (evt.Type == 1)
                {
                    content = (string.Format(text, Player.m_Name, m_Monster.m_Name));
                } else if (evt.Type == 2) {
                    Spell spell = SpellManager.Instance().RandomSpell();
                    content = (string.Format(text, Player.m_Name, spell.m_Data.Name));
                }
            }
            else if (text.Contains("{0}"))
            {
                content = (string.Format(text, Player.m_Name));
            }
            else
            {
                content = text;
            }
        }
        ShowManager.Instance().Add(content);
        return evt;
    }

    /// <summary>
    /// 事件可否执行 0 无限制，1 等级限制 2 仙法限制 3关卡限制
    /// </summary>
    /// <returns></returns>
    private bool EventCanExecute(dbc.Event evt)
    {
        if (evt.PreCond == 0) return true;
        if (evt.PreCond == 1)
        {
            return Player.m_Lv >= evt.CondValue;
        }
        if (evt.PreCond == 2)
        {
            return SpellManager.Instance().IsLearned(evt.CondValue);
        }
        if (evt.PreCond == 3)
        {
            return MapManager.Instance().IsMapOpened(evt.CondValue);
        }
        return false;
    }

    public void Battle(BattleEntity caster, BattleEntity target)
    {
        if (caster.m_Type == EntityType.player)
        {
            Spell spell = SpellManager.Instance().RandomSpell();
            caster.m_Spell = spell;
        }
        SpellManager.Instance().ParseSpell(caster, target, caster.m_Spell);
        DmgResult result = DmgCalc.Instance().Calc(caster, target);
        target.m_Hp -= result.value;
        if (target.m_Hp <= 0)
        {
            result.isDie = true;
            EventsMgr.GetInstance().TriigerEvent(EventsType.Battle_End, true);
            if (target.m_Type == EntityType.player)
            {
                DialogManager.Instance().Wirte("{0}已经复活......", target.m_Name);
                RecovryPlayer();
            }
        }
        else
        {
            result.isDie = false;
        }
        DialogManager.Instance().ShowCommonDialog(caster, target, result);
    }
}
