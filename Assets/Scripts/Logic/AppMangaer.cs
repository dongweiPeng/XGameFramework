/**************************************************
Copyright (C) 2018 Sakura Studio 版权所有
Author: Peng Dongwei
CreateTime: 8/3/2018 2:33:49 PM
Note : 资源检查完毕后的处理
***************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XFramework;
using XFramework.UI;

public class AppMangaer : ISingleton<AppMangaer> {
    public int m_LoginTime;
    public void InitGame() {
        PlayerManager.Instance().Init() ;
        UIManager.Instance.ShowWindow(WindowID.BattlePanel);
    }

    /// <summary>
    /// 在线时间
    /// </summary>
    /// <returns></returns>
    public int OnlineTime() {
        return 0;
    }
}
