/**************************************************
Copyright (C) 2018 Sakura Studio 版权所有
Author: Peng Dongwei
CreateTime: 6/26/2018 2:03:24 PM
Note :游戏入口
***************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XFramework.Data;
using XFramework.AssetBundlePacker;
using XFramework.UI;

namespace XFramework
{
    public enum GameMode
    {
        Develoepment, //开发模式
        QA, //测试
        Release, //发布模式
    }
    public enum LogType
    {
        Info,
        Warn,
        Error,
    }
    public class GameManager : MonoSingleton<GameManager>
    {
        /// <summary>
        /// 游戏模式
        /// </summary>
        public GameMode m_GameMode = GameMode.Develoepment;
        public bool m_BFrameLog = true;
        IDataManager dataMgr = null;

        private void Awake()
        {
            LogFramework("启动游戏框架");
            dataMgr = LocalDataManager.Instance();
            LogFramework("开始: 预加载配置文件");
            dataMgr.PreLoadData(() =>
            {
                if (m_BFrameLog) LogFramework("结束：预加载配置文件");
                //to-do 这里查看本地预加载文件并实施是否要更新等操作
                if (m_GameMode == GameMode.Release)
                {
                    CheckAssets();
                }
                else
                {
                    InitGame();
                }
            });
        }

        /// <summary>
        /// 检查资源
        /// </summary>
        void CheckAssets()
        {
            LogFramework("开始:资源更新");
            GameObject assetcheck = ResourcesManager.LoadResources<GameObject>("Panel/AssetCheckPanel");
            assetcheck = Utility.Util.NewGameObject(assetcheck, gameObject);
            assetcheck.GetComponent<UpdateView>().StartCheck((result) => {
                LogFramework("结束:资源更新");
                if (string.IsNullOrEmpty(result))
                {
                    InitGame(result);
                }
                else
                {
                    throw new System.Exception("资源更新失败");
                }
             }
            );
        }

        void InitGame(string result = "")
        {
            LogFramework("开始:本地表格读取");
            dataMgr.LoadData(() =>
            {
                LogFramework("结束:本地表格读取");
                LogFramework("结束:启动流程, 开始加载登录界面");
                UIManager.Instance.ShowWindow(WindowID.LoginPanel);
            });
        }

        void LogFramework(string msg, LogType type = 0)
        {
            if (m_BFrameLog)
            {
                if (type == LogType.Info) Debug.Log(string.Format("游戏框架日志>>>{0}", msg));
                else if (type == LogType.Warn) Debug.LogWarning(string.Format("游戏框架日志>>>{0}", msg));
                else if (type == LogType.Error) Debug.LogError(string.Format("游戏框架日志>>>{0}", msg));
            }
        }
    }
}
