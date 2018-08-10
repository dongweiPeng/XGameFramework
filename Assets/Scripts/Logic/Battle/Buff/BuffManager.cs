/**************************************************
Copyright (C) 2018 Sakura Studio 版权所有
Author: Peng Dongwei
CreateTime: 8/6/2018 11:03:11 AM
Note :
***************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XFramework;

public class BuffManager : ISingleton<BuffManager>
{
    private List<IBuff> m_FreeBuffList = new List<IBuff>();
    private List<IBuff> m_WorkBuffList = new List<IBuff>();

    public void Init()
    {

    }

    public void AddBuff(BuffInfo info, BattleEntity attacher)
    {
        IBuff buff = null;
        if (m_FreeBuffList.Count > 0)
        {
            foreach (var v in m_FreeBuffList)
            {
                if (v.GetBuffType() == info.type && v.GetValueType() == info.valueType
                    && v.GetValue() == info.value && v.GetRound() == info.round)
                {
                    buff = v;
                    break;
                }
            }
        }
        if (buff == null)
        {
            buff = new Buff();
            buff.SetBuffType(info.type);
            buff.SetValueType(info.valueType);
            buff.SetValue(info.value);
            buff.SetRound(info.round);
            buff.SetOwner(attacher);
        }
        else
        {
            m_FreeBuffList.Remove(buff);
        }
        m_WorkBuffList.Add(buff);
    }

    public List<IBuff> GetBuffList(BattleEntity player) {
       return m_WorkBuffList.FindAll((buff)=>buff.Owner()== player.m_Type);
    }

    public void Clear() {
        foreach(var v in m_WorkBuffList)
        {
            v.Free();
            m_FreeBuffList.Add(v);
        }
    }
}
