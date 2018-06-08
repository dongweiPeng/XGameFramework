using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    /// <summary>
    /// 数学相关的工具类
    /// </summary>
    public static partial class Util
    {
        /// <summary>
        /// 重映射函数
        /// </summary>
        /// <param name="input_start">映射起点</param>
        /// <param name="input_end">映射终点</param>
        /// <param name="output_start">目标起点</param>
        /// <param name="output_end">目标终点</param>
        /// <param name="input">隐射范围</param>
        /// <returns></returns>
        public static float ReMap(float input_start, float input_end, float output_start, float output_end, float input)
        {
            float f = (output_end - output_start) / (input_end - input_start);
            return (float)System.Math.Round(output_start + (input - input_start) * f, 2);
        }


       /// <summary>
       /// 随机获取不重复的数
       /// </summary>
       /// <param name="range">随机的范围</param>
       /// <param name="count">获取个数</param>
       /// <returns></returns>
        public static int[] RandomNotSame(int range, int count)
        {
            int[] index = new int[range];
            for (int i = 0; i < range; i++) index[i] = i;
            System.Random r = new System.Random();
            int[] result = new int[count];
            int site = range;//设置上限 
            int id;
            for (int j = 0; j < count; j++)
            {
                id = r.Next(1, site - 1);
                //在随机位置取出一个数，保存到结果数组 
                result[j] = index[id];
                //最后一个数复制到当前位置 
                index[id] = index[site - 1];
                //位置的上限减少一 
                site--;
            }
            return result;
        }
    }
}
