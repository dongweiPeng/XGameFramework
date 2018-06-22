/**************************************************
Copyright (C) 2018 Sakura Studio 版权所有
Author: Peng Dongwei
CreateTime: 6/19/2018 4:42:13 PM
Note : 资源更新
***************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using zcode.AssetBundlePacker;

public class UpdateView : MonoBehaviour
{
    public string m_URL = "";
    public Slider m_Slider;
    public Text m_UpdateTips;
    public Text m_TxtVersion;
    public Text m_TxtDebug;

    private int m_CurrentStage = 0;
    private Updater m_Updater;

    uint m_CllientVersion = 0;
    uint m_ServerVersion = 0;

    /// <summary>
    ///   状态对应的描述信息
    /// </summary>
    public static readonly string[] STATE_DESCRIBE_TABLE =
    {
        "",
        "初始化更新信息",
        "连接服务器",
        "获取服务器版本",
        "下载资源",
        "解析资源",
        "清理缓存目录数据",
        "更新完成",
        "更新失败",
        "更新取消",
        "更新中断",
    };
/*
    public static readonly string[] STATE_DESCRIBE_TABLE =
   {
        "",
        "初始化更新信息",
        "连接服务器",
        "更新主配置文件",
        "下载资源",
        "解析资源",
        "清理缓存目录数据",
        "更新完成",
        "更新失败",
        "更新取消",
        "更新中断",
    };*/

    private void Start()
    {
       

        //设定资源加载模式为仅加载AssetBundle资源
        ResourcesManager.LoadPattern = new AssetBundleLoadPattern();
        //设定场景加载模式为仅加载AssetBundle资源
        SceneResourcesManager.LoadPattern = new AssetBundleLoadPattern();
        //切换到示例GUI阶段
        m_CurrentStage = 1;

    }

    void LaunchUpdater()
    {
        m_Updater = gameObject.GetComponent<Updater>();
        if (m_Updater == null)
            m_Updater = gameObject.AddComponent<Updater>();

        List<string> url_group = new List<string>();
        url_group.Add(m_URL);

        m_Updater.OnUpdate += (Args) =>
        {
            if (Args.CurrentState == Updater.emState.UpdateAssetBundle)
            {
                if (m_ServerVersion!=0) return;

                string file = Common.GetCacheFileFullName(Common.RESOURCES_MANIFEST_FILE_NAME);
                ResourcesManifest server_resources_manifest = Common.LoadResourcesManifestByPath(file);
                m_ServerVersion = server_resources_manifest.Data.Version;
                string version = m_TxtVersion.text+"\n" + string.Format("服务器版本号:{0}", m_ServerVersion);
                m_TxtVersion.text = version;
                
                if (m_ServerVersion == m_CllientVersion)
                {
                    m_Updater.AbortUpdate();
                    m_Updater = null;
                    m_Slider.gameObject.SetActive(false);
                    m_UpdateTips.text = "版本一致，准备进入游戏";

                    LoadModel();
                }
            }
        };

        m_Updater.OnDone += (Args)=> {
            m_Slider.gameObject.SetActive(false);
            m_Updater = null;
            m_UpdateTips.text = "更新成功，准备进入游戏";
            LoadModel();
        };
       
        m_Updater.StartUpdate(url_group);
    }

    void InitUpdater()
    {
        if (m_CurrentStage == 1 && AssetBundleManager.Instance.IsReady)
        {
            string full_name = Common.GetFileFullName(Common.RESOURCES_MANIFEST_FILE_NAME);
            m_TxtDebug.text = full_name;
            ResourcesManifest client_resource_manifest = Common.LoadResourcesManifestByPath(full_name);
            if (client_resource_manifest != null)
            {
                m_CllientVersion = client_resource_manifest.Data.Version;
                m_TxtVersion.text = string.Format("客户端版本号:{0}", m_CllientVersion);
            }
            m_CurrentStage = 2;
            LaunchUpdater();
        }
    }

    private void Update()
    {
        InitUpdater();
        if (m_Updater != null)
        {
            m_UpdateTips.text = STATE_DESCRIBE_TABLE[(int)m_Updater.CurrentState];
            m_Slider.value = m_Updater.CurrentStateCompleteValue / m_Updater.CurrentStateTotalValue;
            if (m_Updater.IsDone)
            {
                if (m_Updater.IsFailed)
                {
                    m_UpdateTips.text = "更新失败, 重新开始";
                    Destroy(m_Updater);
                }
                else
                {
                    m_UpdateTips.text = "更新成功";
                }
            }
        }
    }

    void LoadModel()
    {
        string SCENE_FILE = "BattleScene";
        SceneResourcesManager.LoadSceneAsync(SCENE_FILE);

        /*
        string MODEL_FILE = "Assets/PackRoot/Model/RoleModel.prefab";
        GameObject prefab = ResourcesManager.Load<GameObject>(MODEL_FILE);
        if (prefab != null)
        {
            GameObject model_ = GameObject.Instantiate(prefab);
            model_.transform.position = Vector3.zero;
        }*/
    }
}
