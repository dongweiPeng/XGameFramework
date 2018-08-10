/**************************************************
Copyright (C) 2018 Sakura Studio 版权所有
Author: Peng Dongwei
CreateTime: 8/2/2018 12:29:27 PM
Note :事件处理器
***************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EventHandler {
    string Name();
    void Handle(dbc.Event evt);
}
