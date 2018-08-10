/**************************************************
Copyright (C) 2018 Sakura Studio 版权所有
Author: Peng Dongwei
CreateTime: 8/3/2018 4:42:53 PM
Note : 地图解锁管理
***************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMapUnLock  {
    List<dbc.Section> IsUnLock(dbc.Section currentSection, List<dbc.Section> sectionList);
    List<dbc.Section> IsUnLock(List<dbc.Section> sectionList);
}
