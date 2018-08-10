/**************************************************
Copyright (C) 2018 Sakura Studio 版权所有
Author: Peng Dongwei
CreateTime: 8/2/2018 4:30:07 PM
Note : 系统类型的一些公用方法
***************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace XFramework.Utility
{
    public static partial class Util
    {
        /// <summary>
        /// 把String 解析到int
        /// </summary>
        /// <param name="str"></param>
        /// <param name="split"></param>
        /// <returns></returns>
        public static int[] StringSplitIntArray(string str, char split)
        {
            string[] array =  str.Split(split);
            int[] reslut = new int[array.Length];

            for(int i=0; i<array.Length; i++)
            {
                reslut[i] = int.Parse(array[i]);
            }
            return reslut;
        }
    }
}