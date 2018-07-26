/**************************************************
Copyright (C) 2018 Sakura Studio 版权所有
Author: Peng Dongwei
CreateTime: 7/24/2018 5:46:43 PM
Note : Frame 功能，目前支持 简单数据显示 + 点击回调 + 刷新回调 + 对齐方式
***************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XFramework.Utility;
using XFramework.AssetBundlePacker;

namespace XFramework.UI
{
    public class UIFrame : MonoBehaviour
    {
        /// <summary>
        /// 上下左右的 摆放顺序
        /// </summary>
        public GameObject m_Root;
        /// <summary>
        /// 返回按钮
        /// </summary>
        public Button m_Button;
        /// <summary>
        /// 上一个显示的Frame
        /// </summary>
        private dbc.UIFrame m_LastFrame;
        /// <summary>
        /// 缓存的ItemList
        /// </summary>
        private Dictionary<string, GameObject> m_CachFrameItemDict = new Dictionary<string, GameObject>();

        private void Awake()
        {
            m_Button.onClick.AddListener(()=>UIManager.Instance.CurShowWnd.Back());
        }
        public void Init(WindowID wndId)
        {
            bool bContain = LocalDataManager.Instance().m_UIFrame_Dict.ContainsKey(wndId.ToString());
            if (!bContain)
            {
                ShowFrame(false);
                Debug.LogWarning(string.Format("该窗口{0}, 还没加入到UIFrame中", wndId.ToString()));
                return;
            }

            dbc.UIFrame frame = LocalDataManager.Instance().m_UIFrame_Dict[wndId.ToString()];
            //没有Frame的窗口
            if (frame.FrameType == 0)
            {
                ShowFrame(false);
            }
            //根据窗口类型显示不同的排版显示
            //type = 1 顶栏显示 ,其他类型暂时没写
            else if (frame.FrameType == 1)
            {
                //跟上一次的一样直接显示
                if (m_LastFrame!=null && m_LastFrame.BarContent.Equals(frame.BarContent))
                {
                    ShowFrame(true);
                }
                else
                {
                    ShowFrame(GetFrameItemArray(frame));
                }
            }
            m_LastFrame = frame;
        }

        /// <summary>
        /// 优先在缓存中获取，缓存中不存在的则创建
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        GameObject[] GetFrameItemArray(dbc.UIFrame frame)
        {
            string[] BarArray = frame.BarContent.Split(';');
            string[] JumpArray = null;
            string[] JumpTypeArray = null;
            if (frame.JumpWndID.Length > 1)
            {
                JumpArray = frame.JumpWndID.Split(';');
                JumpTypeArray = frame.JumpWndType.Split(';');
            }
            GameObject[] FrameItemArray = new GameObject[BarArray.Length];
            FrameItem frameItem = null;
            for (int i = 0, iMax = BarArray.Length; i < iMax; i++)
            {
                string key = BarArray[i];
                //存在缓存中
                if (m_CachFrameItemDict.ContainsKey(key))
                {
                    FrameItemArray[i] = m_CachFrameItemDict[key];
                    frameItem = FrameItemArray[i].GetComponent<FrameItem>();
                }
                //创建新的FrameItem并加入缓存
                else
                {
                    string itemPath = string.Format("BuildIn/Frame/{0}", key);
                    GameObject goFrameItem = ResourcesManager.Load<GameObject>(itemPath);
                    goFrameItem = Util.NewGameObject(goFrameItem, m_Root);
                    frameItem = goFrameItem.GetComponent<FrameItem>();
                    FrameItemArray[i] = goFrameItem;
                    m_CachFrameItemDict.Add(key, goFrameItem);
                }

                //设置跳转
                if (JumpArray != null && JumpArray.Length > i)
                {
                    frameItem.Init(int.Parse(key), int.Parse(JumpArray[i]), int.Parse(JumpTypeArray[i])==1);
                }
                else
                {
                    frameItem.Init(int.Parse(key));
                }
            }
            return FrameItemArray;
        }
        public void ShowFrame(bool bActive)
        {
            gameObject.SetActive(bActive);
        }

        public void ShowFrame(GameObject[] FrameItemArray)
        {
            foreach (var v in m_CachFrameItemDict)
            {
                bool bContain = false;
                foreach(var go in FrameItemArray)
                {
                    if(go == v.Value)
                    {
                        bContain = true;
                        go.SetActive(true);
                    }
                }
                if(!bContain) v.Value.SetActive(false);
            }
            ShowFrame(true);
        }
    }
}
