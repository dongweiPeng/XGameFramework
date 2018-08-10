/**************************************************
Copyright (C) 2018 Sakura Studio 版权所有
Author: Peng Dongwei
CreateTime: 8/1/2018 1:09:15 PM
Note : 通用经验管理器
***************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using XFramework;
/// <summary>
/// 经验管理
/// </summary>
public class ExpManager:ISingleton<ExpManager>{
    /// <summary>
    /// 当前的经验
    /// </summary>
    public long m_CurExp;
    /// <summary>
    /// 当前需要的经验
    /// </summary>
    public int m_CurNeedExp; 
    public event Action<int> levelUp;

    private long m_TotalExp;
    private int m_Lv;
    private int m_MaxLv;
    private Dictionary<int, dbc.Exp> m_ExpDict;
    public void Init(long totalExp) {
        this.m_TotalExp = totalExp;
        m_ExpDict =  LocalDataManager.Instance().m_Exp_Dict;
        m_MaxLv = m_ExpDict[m_ExpDict.Count].level;
        CalcExp();
    }

    /// <summary>
    /// 添加经验
    /// </summary>
    /// <param name="exp"></param>
    public void AddExp(int exp) {
        m_CurExp += exp;
        m_TotalExp += exp;
        if (m_CurExp>= m_CurNeedExp)
        {
            CalcExp();
            if (levelUp != null)
            {
                levelUp(this.m_Lv);
            }
        }
    }

    /// <summary>
    /// 计算经验
    /// </summary>
    private void CalcExp() {
        bool bMaxExp = true;
        foreach (var v in m_ExpDict)
        {
            if (this.m_TotalExp < v.Value.totalExp)
            {
                bMaxExp = false;
                this.m_Lv = Mathf.Clamp(v.Key - 1, 1, m_MaxLv);
                if (v.Key - 1 >= 1)
                {
                    this.m_CurExp = this.m_TotalExp - m_ExpDict[v.Key - 1].totalExp;
                }
                else
                {
                    this.m_CurExp = this.m_TotalExp;
                }
                this.m_CurNeedExp = m_ExpDict[v.Key].exp;
                break;
            }
        }
        if (bMaxExp)
        {
            this.m_Lv = m_MaxLv;
            this.m_CurExp = 0;
            this.m_CurNeedExp = 0;
        }
        Debug.Log(string.Format("等级 = {0}; 经验={1};  总经验={2}", this.m_Lv, this.m_CurExp, this.m_TotalExp));
    }
}
