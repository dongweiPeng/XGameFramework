using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtoBuf;
namespace XFramework.Net
{
    public delegate void MessageHandler(Message message);
    public interface INetManager
    {
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="cmd">消息号</param>
        /// <param name="data">消息内容</param>
        void SendMessage(CmdNumber cmd, IExtensible data);
        /// <summary>
        /// 指定ip链接
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        void Connect(string ip, int port, string serverName);
        /// <summary>
        /// 注册消息处理函数
        /// </summary>
        /// <param name="handler">处理函数</param>
        /// <param name="cmd">消息号</param>
        void Register(MessageHandler handler, CmdNumber cmd);
        /// <summary>
        /// 反注册
        /// </summary>
        /// <param name="cmd"></param>
        void UnRegister(CmdNumber cmd);
        /// <summary>
        /// 断开链接
        /// </summary>
        void DisConnect();
        /// <summary>
        /// 设置掉线处理
        /// </summary>
        /// <param name="handler"></param>
        void SetOfflineHandler(OfflineHandler handler);
        /// <summary>
        /// 是否处理断线状态
        /// </summary>
        /// <returns></returns>
        bool IsOffline();

    }
}