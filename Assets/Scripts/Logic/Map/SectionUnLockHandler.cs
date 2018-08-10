/**************************************************
Copyright (C) 2018 Sakura Studio 版权所有
Author: Peng Dongwei
CreateTime: 8/3/2018 4:44:33 PM
Note :关卡为前置条件
***************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using dbc;
using UnityEngine;
using XFramework.Utility;

public class SectionUnLockHandler : IMapUnLock
{
    public List<Section> IsUnLock(List<Section> sectionList)
    {
        throw new NotImplementedException();
    }

    public List<Section> IsUnLock(Section currentSection, List<Section> sectionList)
    {
        List<dbc.Section> result = new List<Section>();
        foreach (var section in sectionList)
        {
            int[] sectionTypeArray = Util.StringSplitIntArray(section.UnLock, ';');
            int[] sectionCondArray = Util.StringSplitIntArray(section.PreSection, ';');
            for (int i = 0; i < sectionTypeArray.Length; i++)
            {
                if (sectionTypeArray[i] == 1 && sectionCondArray[i] == currentSection.ID)
                {
                    result.Add(section);
                }
            }
        }
        return result;
    }
}
