/**************************************************
Copyright (C) 2018 Sakura Studio 版权所有
Author: Peng Dongwei
CreateTime: 8/2/2018 10:40:37 AM
Note :
***************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XFramework;

public class ShowManager : ISingleton<ShowManager>
{
    private Queue<string> m_ShowQueue = new Queue<string>();

    public Queue<string> ShowQueue
    {
        get
        {
            return m_ShowQueue;
        }
    }

    public void Add(string text, bool record = false, bool debug = false)
    {
        if (text.Length<=1) {
            Debug.LogError("===========================>该行有空格"+ text);
        }
        if (debug)
        {
            Debug.Log(text);
        }
        ShowQueue.Enqueue(text);
    }
}
