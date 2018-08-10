/**************************************************
Copyright (C) 2018 Sakura Studio 版权所有
Author: Peng Dongwei
CreateTime: 8/3/2018 2:04:26 PM
Note : 玩家的管理器
***************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XFramework;

public class PlayerManager:ISingleton<PlayerManager>
{
    /// <summary>
    /// 基础玩家数据
    /// </summary>
    private Player m_BaseInstance;
    /// <summary>
    /// 战斗时的数据，属性不同
    /// </summary>
    private Player m_BattleInstance;

    public void Init() {
        ShowManager.Instance().Add("初始化主角.......");
        SpellManager.Instance().Init();
    }
    public Player BaseInstance
    {
        get
        {
            m_BaseInstance = m_BaseInstance ?? new Player(listenLv:true);
            return m_BaseInstance;
        }
    }

    /// <summary>
    /// 战斗过程中所用的实例
    /// </summary>
    public Player BattleInstance
    {
        get
        {
            m_BattleInstance = m_BattleInstance ?? new Player();
            return m_BattleInstance;
        }
    }

    /// <summary>
    /// 复活战斗实例
    /// </summary>
    public void RecoveryBattleInstance() {
        AttributeManager.Instance().Init(BaseInstance, BaseInstance.m_AttrPoint);
        AttributeManager attr = AttributeManager.Instance();
        m_BattleInstance.m_Hp = attr.Hp();
        m_BattleInstance.m_Mp = attr.Mp();
        m_BattleInstance.m_Fight = attr.Fight();
        m_BattleInstance.m_Dodge = attr.Dodge();
        m_BattleInstance.m_Defence = attr.Defence();
    }
}
