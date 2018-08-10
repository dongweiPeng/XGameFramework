/**************************************************
Copyright (C) 2018 Sakura Studio 版权所有
Author: Peng Dongwei
CreateTime: 8/8/2018 10:32:13 AM
Note : 存储
***************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSave : ISave
{
    public T GetData<T>()
    {
        string msg = PlayerPrefs.GetString(GetKey());
        return JsonUtility.FromJson<T>(msg);
    }

    public virtual string GetKey()
    {
        return "";
    }

    public void Save<T>(T t)
    {
        string msg = JsonUtility.ToJson(t);
        PlayerPrefs.SetString(GetKey(), msg);
        PlayerPrefs.Save();
    }
}
