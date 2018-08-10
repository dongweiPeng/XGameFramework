/**************************************************
Copyright (C) 2018 Sakura Studio 版权所有
Author: Peng Dongwei
CreateTime: 8/7/2018 11:27:33 AM
Note : 战斗结构体，所有的结构都需要转换成这个
***************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EntityType{
    unkownen,
    player,
    enemy,
}
public class BattleEntity  {
    public int m_Lv;
    public string m_Name;
    public int m_Hp;
    public int m_MaxHp;
    public int m_Mp;
    public int m_MaxMp;
    public int m_Fight;
    public int m_Defence;
    public int m_Dodge;
    public int m_Hit;
    public EntityType m_Type;
    public Spell m_Spell;
}
