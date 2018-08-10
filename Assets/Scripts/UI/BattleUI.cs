/**************************************************
Copyright (C) 2018 Sakura Studio 版权所有
Author: Peng Dongwei
CreateTime: 8/1/2018 1:27:10 PM
Note :
***************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XFramework.UI;
using XFramework;

public class BattleUI : UIWndBase
{
    public Text ShowText;
    public Text CachText;

    bool m_FinishBattle = true;
    private void Awake()
    {
        EventsMgr.GetInstance().AttachEvent(EventsType.Battle_End, (result) => {
            this.m_FinishBattle = true;
        });
        BattleManager.Instance().Init(()=> {
            StartCoroutine(RandomEvent());
        });
    }
    public IEnumerator RandomEvent()
    {
        while (true)
        {
            dbc.Event evt = BattleManager.Instance().RandomEvent();
            
            if (evt.Type == 1)
            {
                m_FinishBattle = false;
                BattleEntity monster = BattleManager.Instance().Monster;
                BattleEntity player = BattleManager.Instance().Player;

                BattleEntity caster = null;
                BattleEntity target = null;
                //根据等级决定先手
                if (monster.m_Lv > player.m_Lv)
                {
                    caster = monster;
                    target = player;
                }
                else
                {
                    caster = player;
                    target = monster;
                }
                //播放敌我双方回合 一个回合结束后清除所有的buff
                while (!m_FinishBattle)
                {
                    BattleManager.Instance().Battle(caster, target);
                    yield return new WaitForEndOfFrame();
                    BattleManager.Instance().Battle(target, caster);
                    yield return new WaitForEndOfFrame();
                    BuffManager.Instance().Clear();
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
    string m_ShowContent;
    string m_Content;
    int m_ShowIndex;
    int m_ContentLength;

    Queue<string> m_ShowQueue = new Queue<string>();
    private void FixedUpdate()
    {
        if (string.IsNullOrEmpty(m_Content))
        {
            if (ShowManager.Instance().ShowQueue.Count > 0)
            {
                m_Content = ShowManager.Instance().ShowQueue.Dequeue();
            }
            else
            {
                m_Content = "在思考做点什么......";
            }
            m_Content += "\n";
            m_ShowIndex = 0;
            m_ContentLength = m_Content.Length;

        }
        if (m_ShowIndex < m_ContentLength)
        {
            m_ShowIndex++;
            ShowText.text = (m_Content.Substring(0, m_ShowIndex));
        }
        else
        {
            m_ShowQueue.Enqueue(m_Content);
            if (m_ShowQueue.Count > 10)
            {
                m_ShowQueue.Dequeue();
            }
            string cachText = string.Empty;
            foreach (var cach in m_ShowQueue)
            {
                cachText += cach;
            }
            CachText.text = cachText;

            m_Content = string.Empty;
        }
    }


    private void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 100, 100), "11"))
        {
            MapManager.Instance().ChangeMap(11);
        }
        if (GUI.Button(new Rect(200, 0, 100, 100), "12"))
        {
            MapManager.Instance().ChangeMap(12);
        }
        if (GUI.Button(new Rect(0, 200, 100, 100), "13"))
        {
            MapManager.Instance().ChangeMap(13);
        }
        if (GUI.Button(new Rect(200, 200, 100, 100), "21"))
        {
            MapManager.Instance().ChangeMap(21);
        }
        if (GUI.Button(new Rect(0, 400, 100, 100), "22"))
        {
            MapManager.Instance().ChangeMap(22);
        }
    }
}
