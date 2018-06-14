/// <summary>
/// socket 处理器, 主要处理了 平台服，网关服的接入操作 和 消息的派发
/// </summary>
using UnityEngine;
using System.Collections.Generic;
using System;

namespace XFramework.Net {
public class NetProcessor
{
    public NetSocket socket;
    private ProcessorState state;

    //接收消息的缓存
    private ByteBuffer revBuffer = new ByteBuffer();
    private MessageQueue revMsgQueue = new MessageQueue();

    //缓存发送队列, 有些消息可能没有发送成功
    private MessageQueue cacheNoSendQuene = new MessageQueue();

    //注册反解析委托
    private Dictionary<CmdNumber, MessageHandler> parser = new Dictionary<CmdNumber, MessageHandler>();

    public Dictionary<CmdNumber, MessageHandler> CmdParser
    {
        get { return parser; }
    }

    private static readonly List<CmdNumber> mInLoadingCanDealMsgs = new List<CmdNumber>
        {
            CmdNumber.RespondPingClientCmd_S,
            CmdNumber.RespondMsgClientCmd_S
        };

    private InLoadingMessageCache mInLoadingMessageCache;

    public NetProcessor()
    {
        socket = new NetSocket();
        mInLoadingMessageCache = new InLoadingMessageCache(this);
        socket.OnDisConnectCallBack = OnDisConnectByServer;
        state = ProcessorState.None;
    }
    private void OnDisConnectByServer()
    {
        throw new Exception("服务器主动断线");
    }
   
    /// <summary>
    /// 服务器链接
    /// </summary>
    /// <param name="ip">当前服务器ip</param>
    /// <param name="port">当前服务器端口</param>
    /// <param name="serverName">服务器名字,网关服，游戏服等</param>
    public void Connect(string ip, ushort port, string serverName)
    {
        Debug.Log(string.Format("请求链接 {0} 服务器 ip={1}; port={2}", serverName, ip, port));
        socket.OnConnSuccessCallBack = delegate ()
        {
            Debug.Log(string.Format("{0}  服务器 已建立链接", serverName));
            state = ProcessorState.Access;
        };
        socket.OnConnFailCallBack = delegate() {
            throw new Exception(string.Format("{0}  服务器 链接失败 ip={2}； port = {3}", serverName, ip, port));
        };
        socket.OnRevCallBack = CacheMsg;
        socket.TryConnect(ip, port);
    }

    #region 注册处理器
    /// <summary>
    /// 注册消息回调
    /// </summary>
    public bool RegParseFun(MessageHandler parsefun, CmdNumber cmdno)
    {
        if (parser.ContainsKey(cmdno))
        {
            parser.Remove(cmdno);
        }
        parser[cmdno] = parsefun;
        return true;
    }

    /// <summary>
    /// 反注册消息回调
    /// </summary>
    public void DetachParseFun(CmdNumber cmdno)
    {
        if (parser.ContainsKey(cmdno))
        {
            parser[cmdno] = null;
            parser.Remove(cmdno);
        }
    }
    #endregion


    #region 消息的发送与接收
    /// <summary>
    /// 发送消息
    /// </summary>
    public bool Send(Message msg)
    {
        byte[] cmd = NetTools.PackMesage(msg);
        return socket.Send(cmd, cmd.Length);
    }

    /// <summary>
    /// 缓存接收到的信息并压入 队列中，等待主线程提取
    /// </summary>
    public bool CacheMsg(byte[] cmd, int len)
    {
        revBuffer.Push(cmd, (uint)len);
        while (revBuffer.CheckOk())
        {
            Message msg = NetTools.UnPackMessage(revBuffer.GetData());
            revMsgQueue.Enqueue(msg);
            revBuffer.Pop();
        }
        return true;
    }

    /// <summary>
    /// 主线程处理缓存的队列数据
    /// </summary>
    public void HandleMsg()
    {
        HandleMsgsWhenLoadingEnd();

        while (!revMsgQueue.IsEmpty)
        {
            ParseMsg(revMsgQueue.Dequeue());
        }
    }

    #region 场景加载中的消息缓存处理
    /// <summary>
    /// 可在loading中处理的消息
    /// </summary>
    public void CahceMsgsInLoading()
    {
        while (!revMsgQueue.IsEmpty)
        {
            Message msg = revMsgQueue.Dequeue();
            if (IsMessageNeedHandleInLoading(msg.msgkey))
            {
                ParseMsg(msg);
            }
            else
            {
                mInLoadingMessageCache.AddCacheMsgInLoading(msg);
            }
        }
    }

    /// <summary>
    /// 在loading中需处理的个别消息
    /// </summary>
    /// <returns></returns>
    private bool IsMessageNeedHandleInLoading(CmdNumber msgKey)
    {
        return mInLoadingCanDealMsgs.Contains(msgKey);
    }

    private void HandleMsgsWhenLoadingEnd()
    {
        if (null != mInLoadingMessageCache)
        {
            mInLoadingMessageCache.ParseAllCacheMsg();
        }
    }
    #endregion

    /// <summary>
    ///	具体的处理消息，并委托出去
    /// </summary>
    public void ParseMsg(Message msg)
    {
        if (parser.ContainsKey(msg.msgkey))
        {
            MessageHandler hanldFun = parser[msg.msgkey];
            hanldFun(msg);
        }
        else
        {
            Debug.LogError("未处理消息 " + msg.msgkey);
        }
    }

    private bool CanLogMessage(CmdNumber msgKey)
    {
        return msgKey != CmdNumber.RespondPingClientCmd_S && msgKey != CmdNumber.RespondMsgClientCmd_S &&
               msgKey != CmdNumber.RequestPingClientCmd_C;
    }

    #endregion

    #region 缓存到发送队列,目前没有处理缓存的消息,全部缓存了下来
    public void PushNoSendMsg(Message msg)
    {
        cacheNoSendQuene.Enqueue(msg);
    }

    public void PopNoSendMsg()
    {
        while (!cacheNoSendQuene.IsEmpty)
        {
            Message msg = cacheNoSendQuene.Dequeue();
            Send(msg);
        }
    }

    public void ClearNoSendMsg()
    {
        while (!cacheNoSendQuene.IsEmpty)
        {
            cacheNoSendQuene.Dequeue();
        }
    }
    #endregion
}


public class InLoadingMessageCache
{
    private readonly List<Message> mInLoadingCacheMsgs;
    private readonly NetProcessor mProcessor;

    public InLoadingMessageCache(NetProcessor processor)
    {
        mInLoadingCacheMsgs = new List<Message>();
        mProcessor = processor;
    }

    public void AddCacheMsgInLoading(Message cacheMsg)
    {
        mInLoadingCacheMsgs.Add(cacheMsg);
    }

    public void ParseAllCacheMsg()
    {
        while (mInLoadingCacheMsgs.Count > 0)
        {
            Message msg = mInLoadingCacheMsgs[0];
            mInLoadingCacheMsgs.RemoveAt(0);
            mProcessor.ParseMsg(msg);
        }
    }
    }
}