/**************************************************
Copyright (C) 2018 Sakura Studio 版权所有
Author: Peng Dongwei
CreateTime: 6/26/2018 2:11:30 PM
Note : 数据表接口
***************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XFramework.Data{
    public interface IDataManager
    {
        /// <summary>
        /// 预加载通用的配置文件，比如global表
        /// </summary>
        /// <param name="complate"></param>
        void PreLoadData(System.Action complate = null);
        /// <summary>
        /// 加载配置文件
        /// </summary>
        /// <param name="complate"></param>
        void LoadData(System.Action complate = null);
    }
}
