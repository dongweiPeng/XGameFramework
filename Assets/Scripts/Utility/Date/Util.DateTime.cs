using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Utility
{
    /// <summary>
    /// 日期相关的工具类
    /// </summary>
    public static partial class Util
    {
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static string CurrentStamp() {
            System.TimeSpan span = DateTime.UtcNow - new DateTime(1970, 1, 1, 0 ,0, 0, 0);
            return Convert.ToInt64(span.TotalSeconds).ToString(); 
        }

        /// <summary>
        /// 转换为Unix时间
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int Convert2UnixTime(System.DateTime time) {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }


        /// <summary>
        /// 转换为Unix时间
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime Stamp2DateTime(System.DateTime stamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(stamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }
    }
}
