/// <summary>
/// socket 的基本封装
/// </summary>
using UnityEngine;
using System.Net.Sockets;
using System.Threading;
using System;

namespace XFramework.Net
{
    public delegate void OfflineHandler(OfflineType type);
    public class NetSocket
    {
        private TcpClient tcp;
        /// <summary>
        /// 连接线程 
        /// </summary>
        private Thread connThread;
        private NetThread thread;

        /// <summary>
        /// socket 状态 
        /// </summary>
        private volatile SocketState state = SocketState.None;

        private string IP;
        private int port;
        //address type：ipv4(AddressFamily.InterNetwork) or ipv6(InterNetworkV6)
        AddressFamily addressFamily = AddressFamily.InterNetwork;

        private bool isStartConnect = false;
        private float startConnectTime;

        public ManualResetEvent connDone = new ManualResetEvent(false);

        public Action OnConnSuccessCallBack;
        public Action OnConnFailCallBack;
        public delegate bool OnReceiveCallBack(byte[] data, int len);
        public OnReceiveCallBack OnRevCallBack;

        public Action OnDisConnectCallBack;

        public event OfflineHandler m_OfflineHandler;

        public NetSocket()
        {
            state = SocketState.None;
        }

        public void TryConnect(string ip, int port)
        {
            this.IP = ip;
            this.port = port;

            //iOS9.2 after need support IPV6，for submit appstore
            string newServerIp = "";
            IPConvert.getIPType(IP, port.ToString(), out newServerIp, out addressFamily);
            if (!string.IsNullOrEmpty(newServerIp)) { IP = newServerIp; }

            if (connThread == null)
            {
                connThread = new Thread(SyncTryConnect);
                connThread.Start();
                isStartConnect = true;
                startConnectTime = Time.realtimeSinceStartup;
            }
        }

        private void SyncTryConnect()
        {
            try
            {
                tcp = new TcpClient(addressFamily);
                tcp.SendBufferSize = 1024;
                tcp.ReceiveBufferSize = 1024;
                tcp.ReceiveTimeout = 10;

                connDone.Reset();
                tcp.BeginConnect(this.IP, this.port, new AsyncCallback(TryConnectCallBack), tcp);
                connDone.WaitOne();

            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }
        }

        public void TryConnectCallBack(IAsyncResult ar)
        {
            connDone.Set();
            TcpClient t = (TcpClient)ar.AsyncState;
            t.EndConnect(ar);
            HandleConnResult();
        }

        private void HandleConnResult()
        {
            isStartConnect = false;
            if (connThread != null)
            {
                connThread.Join();
            }
            connThread = null;
            thread = null;
            if ((tcp != null) && tcp.Connected)
            {
                Debug.Log("连接成功");
                state = SocketState.Connect;
                StartTcp();
                if (OnConnSuccessCallBack != null)
                {
                    OnConnSuccessCallBack();
                }
            }
            else
            {
                Debug.Log("连接失败");
                if (OnConnFailCallBack != null)
                {
                    OnConnFailCallBack();
                }
            }
        }

        private void StartTcp()
        {
            if (thread == null)
            {
                Debug.Log("为tcp 创建新线程");
                thread = new NetThread();
                thread.Start(this);
            }
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        public void Receive()
        {
            while (CheckConnect())
            {
                if (state == SocketState.Connect)
                {
                    try
                    {
                        byte[] revBytes = new byte[tcp.ReceiveBufferSize];
                        int num = tcp.GetStream().Read(revBytes, 0, revBytes.Length);
                        if (num != 0 && OnRevCallBack != null)
                        {
                            OnRevCallBack(revBytes, num);
                        }
                        else
                        {
                            if (OnDisConnectCallBack != null)
                            {
                                OnDisConnectCallBack();
                            }
                            state = SocketState.Disconnect;
                            OnDisConnect(OfflineType.ByServer);
                            Debug.Log("服务器要求断线, 接收数据失败, 连接断开");
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.Log("读取数据失败" + ex.Message);
                        state = SocketState.Disconnect;
                        //      OnDisConnect();
                    }
                }
                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        public bool Send(byte[] data, int len)
        {
            if (CheckConnect())
            {
                try
                {
                    tcp.GetStream().Write(data, 0, len);
                    return true;

                }
                catch (Exception ex)
                {
                    Debug.LogError("数据发送失败" + ex.Message);
                    state = SocketState.Disconnect;
                    OnDisConnect(OfflineType.NetEnvironment);
                    return false;
                }
            }
            else
            {
                Debug.Log("未创建链接");
            }
            return false;
        }

        /// <summary>
        ///	 检查链接超时
        /// </summary>
        public void CheckConnTimeout()
        {
            if (!isStartConnect || Time.frameCount % 10 != 0)
                return;

            float curTime = Time.realtimeSinceStartup;
            if (curTime - startConnectTime >= 5)
            {
                Debug.Log("链接超时, 准备从重连");
                connDone.Set();
                HandleConnResult();
            }
        }
        /// <summary>
        /// 判断是否链接上 
        /// </summary>
        private bool CheckConnect()
        {
            return (state == SocketState.Connect);
        }

        /// <summary>
        /// 销毁线程 
        /// </summary>
        public void DestoryThread()
        {
            if (tcp != null)
            {
                tcp.Close();
            }

            tcp = null;
            thread = null;
            if (connThread != null)
            {
                connThread = null;
            }
            state = SocketState.Disconnect;
            Debug.Log("客户端 成功 关掉网络连接");
        }

        /// <summary>
        /// 断开链接 
        /// </summary>
        public void DisConnect()
        {
            if (thread != null)
            {
                thread.Join();
            }
            DestoryThread();
        }

        /// <summary>
        /// 掉线处理
        /// </summary>
        /// <param name="offlineType"></param>
        private void OnDisConnect(OfflineType offlineType)
        {
            if (m_OfflineHandler != null)
            {
                m_OfflineHandler(offlineType);
            }
        }
    }
}
