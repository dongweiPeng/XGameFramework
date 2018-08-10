/**************************************************
Copyright (C) 2018 Sakura Studio 版权所有
Author: Peng Dongwei
CreateTime: 8/7/2018 12:24:30 PM
Note :
***************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISave {
    void Save<T>(T t);
    T GetData<T>();
}
