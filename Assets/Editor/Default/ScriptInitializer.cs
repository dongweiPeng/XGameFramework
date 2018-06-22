/**************************************************
Copyright (C) 2018 Sakura Studio 版权所有
Author: Peng Dongwei
CreateTime: #CREATETIME#
Note : 默认文本创建时间修改
***************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace XFamework.Editor
{
    public class ScriptInitializer : UnityEditor.AssetModificationProcessor
    {
     
        public static void OnWillCreateAsset(string path){
            path = path.Replace(".meta", "");
            if (path.ToLower().EndsWith(".cs"))
            {
                string content = File.ReadAllText(path);
                content = content.Replace("#CREATETIME#", System.DateTime.Now.ToString());
                File.WriteAllText(path, content);
            }
        }
    }
}
