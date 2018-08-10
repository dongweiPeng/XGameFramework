/**************************************************
Copyright (C) 2018 Sakura Studio 版权所有
Author: Peng Dongwei
CreateTime: 8/1/2018 12:18:37 PM
Note :
***************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell {
    public int m_Lv;
    public int m_Id;
    public dbc.Spell m_Data;
    public Status m_Status;

    public int Fight()
    {
        return m_Data.Damage*m_Lv;
    }

    public int Hit() {
        return m_Data.Hit * m_Lv;
    }

    public int Block() {
        return m_Data.Block * m_Lv;
    }

    public int Dodge() {
        return m_Data.Dodge * m_Lv;
    }

    public enum Status
    {
        free,
        learning,
        max,
    }
}
