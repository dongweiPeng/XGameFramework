/**************************************************
Copyright (C) 2018 Sakura Studio 版权所有
Author: Peng Dongwei
CreateTime: 6/26/2018 2:46:12 PM
Note :
***************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XFramework.Data;
using XFramework;
using dbc;
public class LocalDataManager : ISingleton<LocalDataManager>, IDataManager
{
    public Dictionary<string, UIFrame> m_UIFrame_Dict;
    public Dictionary<System.Int32, List<dbc.Event>> m_Event_Pool = new Dictionary<System.Int32, List<dbc.Event>>();
    public Dictionary<System.Int32, dbc.Section> m_Map_Dict;
    public Dictionary<System.Int32, dbc.Exp> m_Exp_Dict;
    public Dictionary<System.Int32, dbc.Spell> m_Spell_Dict;
    public Dictionary<System.Int32, dbc.CommonDialog> m_ComDialog_Dict;
    public Dictionary<System.Int32, dbc.Monster> m_Monster_Dict;
    public Dictionary<System.Int32, dbc.MonsterAttr> m_MonserAttr_Dict;

    public void LoadData(Action complate = null)
    {
        Debug.Log("加载资源中....");
        UIFrameTable frameTable = ProtoTool.Load<UIFrameTable>("UIFrame");
        m_UIFrame_Dict = ProtoTool.BuildMap<string, UIFrame>("WindowID", frameTable.tlist);
     
        dbc.SectionTable sectionTable = ProtoTool.Load<dbc.SectionTable>("Section");
        m_Map_Dict = ProtoTool.BuildMap<System.Int32, dbc.Section>("ID", sectionTable.tlist);

        dbc.ExpTable exptable = ProtoTool.Load<dbc.ExpTable>("Exp");
        m_Exp_Dict = ProtoTool.BuildMap<System.Int32, dbc.Exp>("level", exptable.tlist);

        dbc.SpellTable spellTable = ProtoTool.Load<dbc.SpellTable>("Spell");
        m_Spell_Dict = ProtoTool.BuildMap<System.Int32, dbc.Spell>("ID", spellTable.tlist);

        dbc.CommonDialogTable dialogTable = ProtoTool.Load<dbc.CommonDialogTable>("CommonDialog");
        m_ComDialog_Dict = ProtoTool.BuildMap<System.Int32, dbc.CommonDialog>("ID", dialogTable.tlist);

        dbc.MonsterTable monsterTable = ProtoTool.Load<dbc.MonsterTable>("Monster");
        m_Monster_Dict = ProtoTool.BuildMap<System.Int32, dbc.Monster>("ID", monsterTable.tlist);

        dbc.MonsterAttrTable monsterAttrTable = ProtoTool.Load<dbc.MonsterAttrTable>("MonsterAttr");
        m_MonserAttr_Dict = ProtoTool.BuildMap<System.Int32, dbc.MonsterAttr>("ID", monsterAttrTable.tlist);
        

        //事件 根据章节进行分类
        dbc.EventTable eventTable = ProtoTool.Load<dbc.EventTable>("Event");
        foreach (var v in eventTable.tlist) {
            if (m_Event_Pool.ContainsKey(v.SectionId)) {
                List<dbc.Event> list = m_Event_Pool[v.SectionId];
                list.Add(v);
            }
            else
            {
                List<dbc.Event> list = new List<dbc.Event>();
                list.Add(v);
                m_Event_Pool.Add(v.SectionId, list);
            }
        }
        if(complate!=null) complate();
    }

    public void PreLoadData(Action complate = null)
    {
        Debug.Log("预加载资源中....");
        complate();
    }
}


