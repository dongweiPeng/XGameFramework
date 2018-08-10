/**************************************************
Copyright (C) 2018 Sakura Studio 版权所有
Author: Peng Dongwei
CreateTime: 8/3/2018 5:14:41 PM
Note : 事件解锁
***************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using dbc;
using UnityEngine;
using XFramework.Utility;

public class EventUnLockHandler : IMapUnLock
{
    public List<Section> IsUnLock(List<Section> sectionList)
    {
        List<dbc.Section> result = new List<Section>();
        foreach (var section in sectionList)
        {
            int[] sectionTypeArray = Util.StringSplitIntArray(section.UnLock, ';');
            int[] sectionCondArray = Util.StringSplitIntArray(section.PreSection, ';');
            for (int i = 0; i < sectionTypeArray.Length; i++)
            {
                if (sectionTypeArray[i] == 3 && EventManager.Instance().IsEventTriggered(sectionCondArray[i]))
                {
                    result.Add(section);
                }
            }
        }
        return result;
    }

    public List<Section> IsUnLock(Section currentSection, List<Section> sectionList)
    {
        throw new NotImplementedException();
    }
}
