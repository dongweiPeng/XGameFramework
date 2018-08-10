/**************************************************
Copyright (C) 2018 Sakura Studio 版权所有
Author: Peng Dongwei
CreateTime: 8/1/2018 2:08:18 PM
Note :
***************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XFramework;
using XFramework.Utility;

public class EventManager : ISingleton<EventManager> {
    public BattleEventHandler m_BattleEvent;

    /// <summary>
    /// 获取一个事件
    /// </summary>
    /// <returns></returns>
    public dbc.Event GetEvent() {
        int type = GetEventType();
        List<dbc.Event> list = MapManager.Instance().m_CurMapEventPool[type];
        //在指定的范围内随机
        List<int> rangeList = new List<int>();
        foreach (var v in list) {
            rangeList.Add(v.TrigRate);
        }
        int rangeIdx = Util.RandomRange(rangeList);
        return list[rangeIdx];
    }
    /// <summary>
    /// 根据概率获取事件类型 1：战斗（85） 2：奇遇（5） 3：修炼（2） 4：其他（3）
    /// </summary>
    /// <returns></returns>
    private int GetEventType() {
        int range = Random.Range(1, 101);
        if (range <= 85) return 1;
        else if (range <= 90) return 2;
        else if (range <= 97) return 3;
        return 4;
    }

    /// <summary>
    /// 事件是否已经被触发
    /// </summary>
    /// <param name="eventId"></param>
    /// <returns></returns>
    public bool IsEventTriggered(int eventId) {
        return false;
    }

}
