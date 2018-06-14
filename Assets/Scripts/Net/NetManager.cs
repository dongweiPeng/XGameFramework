using System;
using System.Collections;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;
using System.IO;
/// <summary>
/// 网络管理器，负责链接+收发网络包+断线处理等
/// </summary>
namespace XFramework.Net
{
    public class NetManager : MonoBehaviour, INetManager
    {
        private MessageQueue m_SendQueue;
        private MessageQueue m_ReciveQueue;
        //  private Dictionary<CmdNumber, MessageHandler> m_HandlerDict = new Dictionary<CmdNumber, MessageHandler>();
        private NetProcessor m_Processor;

#if USE_PROTODLL
	private DTOSerializer dto = new DTOSerializer();
#endif
        public void Connect(string ip, int port, string serverName)
        {
            m_SendQueue = m_SendQueue ?? new MessageQueue();
            m_ReciveQueue = m_ReciveQueue ?? new MessageQueue();
            m_Processor = m_Processor ?? new NetProcessor();
            m_Processor.Connect(ip, (ushort)port, serverName);
        }

        /// <summary>
        /// 防止线程卡死，在调用的时候需要这样使用
        /// </summary>
        private void OnDestroy()
        {
            DisConnect();
        }
        public void DisConnect()
        {
            Debug.Log("客户端 请求 断开网络链接");
            if (m_Processor != null)
            {
                m_Processor.socket.DestoryThread();
            }
        }

        private void Update()
        {
            if (m_Processor != null)
            {
                m_Processor.HandleMsg();
            }
        }

        public bool IsOffline()
        {
            return false;
        }

        public void Register(MessageHandler handler, CmdNumber cmd)
        {
            if (m_Processor != null)
            {
                m_Processor.RegParseFun(handler, cmd);
            }
        }

        public void UnRegister(CmdNumber cmd)
        {
            if (m_Processor != null)
            {
                m_Processor.DetachParseFun(cmd);
            }
        }

        public void SendMessage(CmdNumber cmd, IExtensible data)
        {
            byte[] bytes = null;
            if (data != null)
            {
                MemoryStream mem = new MemoryStream();
#if USE_PROTODLL
				dto.Serialize(mem, cmd);
#else
                Serializer.Serialize<IExtensible>(mem, data);
#endif
                bytes = NetTools.MemoryStreamToBytes(mem, 0);
            }
            Message message = new Message(cmd, bytes);
            m_Processor.Send(message);
        }

        public object AnalysisMessage(Message msg, System.Type type)
        {
            MemoryStream mem = new MemoryStream(msg.msgData);
            object result = ProtoBuf.Meta.RuntimeTypeModel.Default.Deserialize(mem, null, type);
            return result;
        }

        public void SetOfflineHandler(OfflineHandler handler)
        {
            if (m_Processor.socket != null)
            {
                m_Processor.socket.m_OfflineHandler += handler;
            }
        }
    }
}
