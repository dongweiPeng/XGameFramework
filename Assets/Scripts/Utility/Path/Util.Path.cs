using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Unity 路径相关的工具
/// </summary>
namespace XFramework.Utility
{
    public static partial class Util
    {

        /// <summary>
        /// 取得数据存放目录
        /// 持久化目录，可读写目录
        /// </summary>
        public static string DataPath
        {
            get
            {
                /*
                //编辑器内测试不同平台的逻辑bug， 表现无法测试，必须到真实平台测试
                #if PC_RUN_ANDROID || PC_RUN_IOS || PC_RUN_WINDOWS
		    		return Application.persistentDataPath + "/" + game + "/"+GetPlatformFolder()+"/";
                #endif

                if (Application.isMobilePlatform)
                {
                    #if UNITY_ANDROID
				    return Application.persistentDataPath + "/" + game + "/" + GetPlatformFolder() + "/";
			    	//return "/data/data/"+Application.buildGUID+"/files/"+ game + "/" + GetPlatformFolder() + "/";
                    #else
                    return Application.persistentDataPath  + "/" + GetPlatformFolder() + "/";
                    #endif
                }
                if (Application.platform == RuntimePlatform.WindowsEditor)
                {
                    return Application.dataPath + "/" + GetPlatformFolder() + "/";
                }*/
                return "c:/";
            }
        }

        public static string GetPlatformFolder()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                return "Android";
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                return "IOS";
            }
            else if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                return "Windows";
            }
            else
            {
                return "Windows";
            }
        }

        /// <summary>
        /// 本地路径
        /// </summary>
        /// <returns></returns>
        public static string GetRelativePath()
        {
            if (Application.isEditor)
                return "file://" + System.Environment.CurrentDirectory.Replace("\\", "/") + "/Assets/" + "/";
            else if (Application.isMobilePlatform || Application.isConsolePlatform)
                return "file:///" + DataPath;
            else // For standalone player.
                return "file://" + Application.streamingAssetsPath + "/";
        }

        /// <summary>
        /// 应用程序内容路径 这个是不同平台对应的StreamingAsserts的路径
        /// </summary>
        public static string AppContentPath()
        {

            #if PC_RUN_ANDROID || PC_RUN_IOS || PC_RUN_WINDOWS
			return (Application.dataPath + "/" + AppConst.AssetDirname + "/" + GetPlatformFolder()+"/");
            #endif

            string path = string.Empty;
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    path = "jar:file://" + Application.dataPath + "!/assets/" + GetPlatformFolder() + "/";
                    break;
                case RuntimePlatform.IPhonePlayer:
                    path = Application.dataPath + "/Raw/" + GetPlatformFolder() + "/";
                    break;
                default:
                    path = Application.dataPath + "/" + "/" + GetPlatformFolder() + "/";
                    break;
            }
            return path;
        }
    }
}
