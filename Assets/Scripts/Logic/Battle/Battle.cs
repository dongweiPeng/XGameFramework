/**************************************************
Copyright (C) 2018 Sakura Studio 版权所有
Author: Peng Dongwei
CreateTime: 8/3/2018 11:32:18 AM
Note : 战斗中
***************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : IBattleHandler
{
    public void Execute(BattleRuntime runtime, Action nextHandler = null)
    {

       

      
        if (nextHandler != null) nextHandler();
    }

    public BattleStatus Status()
    {
        return BattleStatus.battleing;
    }
}
