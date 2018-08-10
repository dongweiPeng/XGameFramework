/**************************************************
Copyright (C) 2018 Sakura Studio 版权所有
Author: Peng Dongwei
CreateTime: 8/3/2018 11:30:15 AM
Note :
***************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreBattle : IBattleHandler
{
    public void Execute(BattleRuntime runtime, Action nextHandler = null)
    {
        //初始化地图
        MapManager.Instance().Init();
        MapManager.Instance().ChangeMap(GameConfig.MapProgress);

        if (nextHandler != null) nextHandler();
    }

    public BattleStatus Status()
    {
        return BattleStatus.prebattle;
    }
}
