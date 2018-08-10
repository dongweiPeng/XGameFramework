/**************************************************
Copyright (C) 2018 Sakura Studio 版权所有
Author: Peng Dongwei
CreateTime: 8/6/2018 11:12:42 AM
Note :
***************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffType
{
    Defence,//防御
    Hit,    //命中
    Damage, //受伤
}
public enum BuffValueType
{
    fix,
    rate,
}


public struct BuffInfo
{
    public BuffType type;
    public float value;
    public BuffValueType valueType; //只类型，0：默认是固定值，1：百分比
    public int round;
}

public class Buff : IBuff
{
    protected BuffType m_Type;
    protected int m_Round;
    protected float m_Value;
    protected BuffValueType m_ValueType;
    protected EntityType m_Owner;

    public float GetValue()
    {
        return m_Value;
    }

    public void SetValue(float value)
    {
        this.m_Value = value;
    }

    public void Free()
    {
        this.m_Owner = EntityType.unkownen;
        this.m_Value = 0;
        this.m_Round = 0;
    }

    public EntityType Owner()
    {
        return this.m_Owner;
    }

    public void SetOwner(BattleEntity player)
    {
        this.m_Owner = player.m_Type;
    }

    public BuffValueType GetValueType()
    {
        return this.m_ValueType;
    }

    public void SetValueType(BuffValueType type)
    {
        this.m_ValueType = type;
    }

    public string Name()
    {
        throw new NotImplementedException();
    }

    public int GetRound()
    {
        return this.m_Round;
    }

    public void SetRound(int round)
    {
        this.m_Round = round;
    }

    public BuffType GetBuffType()
    {
        return this.m_Type;
    }

    public void SetBuffType(BuffType type)
    {
        this.m_Type = type;
    }

}
