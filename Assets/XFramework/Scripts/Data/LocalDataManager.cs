/**************************************************
Copyright (C) 2018 Sakura Studio 版权所有
Author: Peng Dongwei
CreateTime: 6/26/2018 2:46:12 PM
Note :
***************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XFramework.Data;
using XFramework;
using dbc;
public class LocalDataManager : ISingleton<LocalDataManager>, IDataManager
{
    public Dictionary<string, UIFrame> m_UIFrame_Dict;
    public void LoadData(Action complate = null)
    {
        Debug.Log("加载资源中....");
           UIFrameTable frameTable = ProtoTool.Load<UIFrameTable>("UIFrame");
           m_UIFrame_Dict = ProtoTool.BuildMap<string, UIFrame>("WindowID", frameTable.tlist);
        complate();
    }

    public void PreLoadData(Action complate = null)
    {
        Debug.Log("预加载资源中....");
        complate();
    }
}


