/**************************************************
Copyright (C) 2018 Sakura Studio 版权所有
Author: Peng Dongwei
CreateTime: 8/7/2018 12:21:12 PM
Note :玩家储存：等级 + 属性点 + 技能数据
***************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePlayer:BaseSave  {
    public override string GetKey()
    {
        return "player";
    }
}
