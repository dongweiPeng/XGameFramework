using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XFramework.Utility
{
    /// <summary>
    /// Unity本身相关的工具类
    /// </summary>
    public static partial class Util
    {
        /// <summary>
        /// 删除子节点
        /// </summary>
        /// <param name="go"></param>
        public static void ClearChild(Transform go)
        {
            if (go == null) return;
            for (int i = go.childCount - 1; i >= 0; i--)
            {
                GameObject.Destroy(go.GetChild(i).gameObject);
            }
        }

        /// <summary>
        /// 添加子节点到目标
        /// </summary>
        /// <param name="target"></param>
        /// <param name="child"></param>
        /// <param name="scale"></param>
        public static void AddChildToTarget(Transform target, Transform child, Vector3 scale = default(Vector3))
        {
            child.SetParent(target);
            child.localScale = scale;
            child.localPosition = Vector3.zero;
            child.localEulerAngles = Vector3.zero;
            ChangeChileLayer(child, target.gameObject.layer);
        }

        public static void ChangeChileLayer(Transform t, int layer)
        {
            t.gameObject.layer = layer;
            for (int i = 0; i > t.childCount; ++i)
            {
                Transform child = t.GetChild(i);
                child.gameObject.layer = layer;
                ChangeChileLayer(child, layer);
            }
        }

        /// <summary>
        /// 添加脚本到指定的对象上, 如果对象上已经有了该脚本则不重复添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go"></param>
        /// <returns></returns>
        public static T TryAddComponent<T>(GameObject go) where T : Component
        {
            if (go != null)
            {
                T t = go.GetComponent<T>();
                if (t == null)
                {
                    t = go.AddComponent<T>();
                }
                return t;
            }
            return null;
        }

        /// <summary>
        /// 尝试删除指定脚本
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go"></param>
        public static void TryRemove<T>(GameObject go) where T : Component
        {
            if (go != null)
            {
                T t = go.GetComponent<T>();
                if (t != null)
                {
                    GameObject.Destroy(t);
                }
            }
        }

        /// <summary>
        /// 归一化指定的物体
        /// </summary>
        /// <param name="go"></param>
        public static void Reset(GameObject go)
        {
            go.transform.localPosition = Vector3.zero;
            go.transform.localEulerAngles = Vector3.zero;
            go.transform.localScale = Vector3.one;
        }

        /// <summary>
        /// 获取字符串的长度无视颜色值
        /// </summary>
        /// <param name="text">文本</param>
        /// <returns></returns>
        public static int StringLengthIgnoreColor(string text)
        {
            int length = 0;
            for (int i = 0; i < text.Length;)
            {
                if (text[i] == '<')
                {
                    if (text.Substring(i, 8) == "<color=#")
                    {
                        i += 14;
                        continue;
                    }
                    if (text.Substring(i, 8) == "</color>")
                    {
                        i += 8;
                        continue;
                    }
                }
                i++;
                length++;
            }
            return length;
        }
    }
}
