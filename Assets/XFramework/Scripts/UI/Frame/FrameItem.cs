/**************************************************
Copyright (C) 2018 Sakura Studio 版权所有
Author: Peng Dongwei
CreateTime: 7/24/2018 5:52:09 PM
Note : 
如何更改数据 ：         
            FrameData data = default(FrameData);
            data.Type = 1; //类型对应了配置表的
            data.Value = 100;
            EventsMgr.GetInstance().TriigerEvent(EventsType.FrameItem_Refresh, data);

如何接受点击跳转响应 ：在配置表中
***************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XFramework.UI { 
    public class FrameItem : MonoBehaviour {
        public Text  m_Num;
        public Button m_Button;
       /// <summary>
       /// 货币栏的类型：0 铜钱 1 金币 2 体力 3 砖石 4 花 5 竞技币 6 武魂
       /// </summary>
        private int m_Type;

        public struct FrameData {
            public int Type;
            public int Value;
        }

        private void Awake()
        {
            EventsMgr.GetInstance().AttachEvent(EventsType.FrameItem_Refresh, (data)=> {
                FrameData frameData = (FrameData)data;
                if(frameData.Type == m_Type)
                {
                    this.m_Num.text = ((int)frameData.Value).ToString();
                }
            });

  
        }

        /// <summary>
        /// 初始化FrameItem
        /// </summary>
        /// <param name="type">类型：0 铜钱 1 金币 2 体力 3 砖石 4 花 5 竞技币 6 武魂</param>
        /// <param name="jumpWindowId">跳转窗口的ID</param>
        public void Init(int type, int jumpWindowId=0, bool bAppendMode = false)
        {
            this.m_Num.text = "0";
            this.m_Type = type;

            if (jumpWindowId != 0)
            {
                this.m_Button.onClick.AddListener(() =>
                {
                    UIManager.Instance.ShowWindow((WindowID)jumpWindowId, bAppendMode);
                });
            }
        }
    }
}
