/**************************************************
Copyright (C) 2018 Sakura Studio 版权所有
Author: Peng Dongwei
CreateTime: 8/3/2018 12:23:43 PM
Note :
***************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XFramework;

public class CurrencyManager : ISingleton<CurrencyManager>
{
    /// <summary>
    /// 铜钱
    /// </summary>
    public int m_Coin;
    /// <summary>
    /// 金币
    /// </summary>
    public int m_Gold;
    /// <summary>
    /// 砖石
    /// </summary>
    public int m_Diamond;
    /// <summary>
    /// 体力
    /// </summary>
    public int m_Stamina;
}
