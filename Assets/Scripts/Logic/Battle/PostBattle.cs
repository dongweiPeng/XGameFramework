/**************************************************
Copyright (C) 2018 Sakura Studio 版权所有
Author: Peng Dongwei
CreateTime: 8/3/2018 11:38:15 AM
Note : 战后处理
***************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostBattle : IBattleHandler
{
    public void Execute(BattleRuntime runtime, Action nextHandler = null)
    {
        if (runtime.m_IsWin)
        {
            dbc.Section curmap = MapManager.Instance().m_CurSection;
            if (curmap.Exp > 0)
            {
                ExpManager.Instance().AddExp(curmap.Exp);
                CurrencyManager.Instance().m_Stamina = curmap.Potential;
            }
            if (curmap.Potential > 0)
            {
                CurrencyManager.Instance().m_Stamina = curmap.Potential;
            }
            if (runtime.m_IsBossSection)
            {

            }
        }
        if (nextHandler != null) nextHandler();
    }

    public BattleStatus Status()
    {
        return BattleStatus.postbattle;
    }
}
