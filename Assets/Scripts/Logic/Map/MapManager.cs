/**************************************************
Copyright (C) 2018 Sakura Studio 版权所有
Author: Peng Dongwei
CreateTime: 8/1/2018 2:07:26 PM
Note : 地图管理
***************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XFramework;

public class MapManager : ISingleton<MapManager> {
    /// <summary>
    /// 当前进度
    /// </summary>
    public int m_ProgressId;
    /// <summary>
    /// 当前在玩的关卡
    /// </summary>
    public dbc.Section m_CurSection;
    /// <summary>
    /// 已经解锁的关卡集合
    /// </summary>
    private List<dbc.Section> m_OpenMapList = new List<dbc.Section>();
    /// <summary>
    /// 没有解锁的关卡集合
    /// </summary>
    private List<dbc.Section> m_CloseMapList = new List<dbc.Section>();
    /// <summary>
    /// 解锁条件
    /// </summary>
    private EventUnLockHandler m_EventUnLockHandler;
    private TimeUnLockHandler m_TimeUnLockHandler;
    private SectionUnLockHandler m_SectionUnLockHandler;

    public Dictionary<int/*事件类型*/, List<dbc.Event>> m_CurMapEventPool = new Dictionary<int, List<dbc.Event>>();

    /// <summary>
    /// 切换场景，同时切换事件池
    /// </summary>
    /// <param name="mapId"></param>
    public void ChangeMap(int mapId)
    {
        if (this.m_CurSection==null || mapId != this.m_CurSection.ID)
        {
            m_CurMapEventPool.Clear();
            List<dbc.Event> list = LocalDataManager.Instance().m_Event_Pool[mapId];
            foreach (var v in list) {
                if (m_CurMapEventPool.ContainsKey(v.Type))
                {
                    List<dbc.Event> eventList = m_CurMapEventPool[v.Type];
                    eventList.Add(v);
                }
                else
                {
                    List<dbc.Event> eventList = new List<dbc.Event>();
                    eventList.Add(v);
                    m_CurMapEventPool.Add(v.Type, eventList);
                }
            }

            this.m_CurSection = LocalDataManager.Instance().m_Map_Dict[mapId];
            ShowManager.Instance().Add(string.Format("准备探索新的区域......"));
            ShowManager.Instance().Add(string.Format("进入新的区域：{0}", this.m_CurSection.Name));
        }
    }

    /// <summary>
    /// 开关卡
    /// </summary>
    /// <param name="mapId"></param>
    public void OpenMap() {
        int mapId = m_CurSection.ID;
        if (!IsMapOpened(mapId)) {
            dbc.Section map = LocalDataManager.Instance().m_Map_Dict[mapId];
            if (m_CloseMapList.Count <= 0) return;
            //前置关卡为条件
            List<dbc.Section> reslutList = m_SectionUnLockHandler.IsUnLock(map, m_CloseMapList);
            //事件为条件
            if (reslutList.Count <= 0) return;
            reslutList = m_EventUnLockHandler.IsUnLock(map, reslutList);
            //时间为条件
            if (reslutList.Count <= 0) return;
            reslutList = m_TimeUnLockHandler.IsUnLock(map, reslutList);
            //降序排选最大的保存进度
            if (reslutList.Count <= 0) return;
            //双表维护并且更改地图进度
            foreach (var v in reslutList) {
                m_OpenMapList.Add(v);
                m_CloseMapList.Remove(v);
                m_ProgressId = m_ProgressId < v.ID ? v.ID : m_ProgressId;
            }

            foreach (var v in reslutList) {
                ShowManager.Instance().Add(string.Format("解锁新的区域：{0}", v.Name), true);
            }
        }
    }

    /// <summary>
    /// 关卡是否打开了
    /// </summary>
    /// <param name="mapId"></param>
    /// <returns></returns>
    public bool IsMapOpened(int mapId) {
        foreach (var v in m_OpenMapList)
        {
            if (v.ID == mapId) return true;
        }
        return false;
    }

    /// <summary>
    /// 最大开放的区域
    /// </summary>
    /// <returns></returns>
    public int MaxOpenRegion() {
        dbc.Section map = LocalDataManager.Instance().m_Map_Dict[m_ProgressId];
        return map.Map;
    }

    /// <summary>
    /// 初始化地图
    /// </summary>
    public void Init() {
        ShowManager.Instance().Add(string.Format("读取地图中......"));
        Dictionary<System.Int32, dbc.Section> mapDict = LocalDataManager.Instance().m_Map_Dict;
        foreach (var v in mapDict) {
            if (v.Key <= GameConfig.MapProgress)
            {
                m_OpenMapList.Add(mapDict[v.Key]);
            }
            else
            {
                m_CloseMapList.Add(mapDict[v.Key]);
            }
        }
        m_TimeUnLockHandler = new TimeUnLockHandler();
        m_EventUnLockHandler = new EventUnLockHandler();
        m_SectionUnLockHandler = new SectionUnLockHandler();
        ShowManager.Instance().Add(string.Format("地图读取成功"));
    }
}
