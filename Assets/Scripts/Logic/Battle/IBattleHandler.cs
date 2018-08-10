/**************************************************
Copyright (C) 2018 Sakura Studio 版权所有
Author: Peng Dongwei
CreateTime: 8/3/2018 11:27:21 AM
Note : 战斗接口
***************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XFramework;

public interface IBattleHandler {
    /// <summary>
    /// 战斗状态
    /// </summary>
    /// <returns></returns>
    BattleStatus Status();
    /// <summary>
    /// 执行逻辑
    /// </summary>
    void Execute(BattleRuntime runtime, System.Action nextHandler=null);

}

public enum BattleStatus {
    prebattle,
    battleing,
    postbattle
}
