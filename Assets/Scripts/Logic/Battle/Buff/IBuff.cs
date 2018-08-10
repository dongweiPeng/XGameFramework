/**************************************************
Copyright (C) 2018 Sakura Studio 版权所有
Author: Peng Dongwei
CreateTime: 8/6/2018 11:03:36 AM
Note :
***************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuff {
    string Name();
    float GetValue();
    void SetValue(float value);
    BuffValueType GetValueType();
    void SetValueType(BuffValueType type);
    EntityType Owner();
    void SetOwner(BattleEntity player);
    BuffType GetBuffType();
    void SetBuffType(BuffType type);
    int GetRound();
    void SetRound(int round);
    void Free();
}
