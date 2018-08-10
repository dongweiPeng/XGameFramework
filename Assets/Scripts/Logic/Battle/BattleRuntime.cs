/**************************************************
Copyright (C) 2018 Sakura Studio 版权所有
Author: Peng Dongwei
CreateTime: 8/3/2018 12:03:39 PM
Note :
***************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BattleRuntime  {
    /// <summary>
    /// 是否胜利
    /// </summary>
    public bool m_IsWin;
    /// <summary>
    /// 是否是boss关
    /// </summary>
    public bool m_IsBossSection;
    public Player m_Caster;
    public Player m_Target;
}
