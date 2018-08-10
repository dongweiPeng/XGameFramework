/**************************************************
Copyright (C) 2018 Sakura Studio 版权所有
Author: Peng Dongwei
CreateTime: 8/2/2018 6:46:47 PM
Note : 属性管理器  属性值 = 基础属性 + 属性点 + 技能属性
***************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XFramework;

public class AttributeManager : ISingleton<AttributeManager>
{
    Player m_Player;
    AttributePoint m_AttrPoint;

    public void Init(Player player, AttributePoint point)
    {
        this.m_Player = player;
        this.m_AttrPoint = point;
    }

    /// <summary>
    /// 生命 一个生命属性点+160生命
    /// </summary>
    /// <returns></returns>
    public int Hp (){
        return m_Player.m_Hp + m_AttrPoint.m_Hp * 160;
    }

    /// <summary>
    /// 斗气
    /// </summary>
    /// <returns></returns>
    public int Mp() {
        return m_Player.m_Mp + m_AttrPoint.m_Mp * 40;
    }

    /// <summary>
    /// 攻击
    /// </summary>
    /// <returns></returns>
    public int Fight()
    {
        return m_Player.m_Fight + m_AttrPoint.m_Power * 40;
    }

    /// <summary>
    /// 格挡
    /// </summary>
    /// <returns></returns>
    public int Defence()
    {
        return m_Player.m_Defence + m_AttrPoint.m_Block * 12;
    }

    /// <summary>
    /// 灵巧
    /// </summary>
    /// <returns></returns>
    public int Dodge()
    {
        return m_Player.m_Dodge + m_AttrPoint.m_Dodge * 12;
    }

    /// <summary>
    /// 天赋-减少潜能消耗
    /// </summary>
    /// <returns></returns>
    public float Talent() {
        return  m_AttrPoint.m_Talent * 0.10f;
    }

    /// <summary>
    /// 机缘-减少战斗事件的概率
    /// </summary>
    /// <returns></returns>
    public float Fate()
    {
        return m_AttrPoint.m_Fate * 0.45f;
    }
}
