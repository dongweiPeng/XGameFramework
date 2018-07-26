/// <summary>
///	网络 线程 
/// </summary>
using UnityEngine;
using System.Collections;
using System.Threading;
namespace XFramework.Net
{
    public class NetThread
    {
        private ThreadState state;
        private Thread thread;

        public NetThread()
        {
            state = ThreadState.None;
        }

        public bool Start(NetSocket socket)
        {
            if (state == ThreadState.None)
            {
                thread = new Thread(new ParameterizedThreadStart(CallFun));
                thread.Start(socket);
                state = ThreadState.Running;
                return true;
            }
            return false;
        }

        public void Final()
        {
            if (state == ThreadState.Running)
            {
                Join();
            }
        }

        public void Join()
        {
            thread.Join();
            state = ThreadState.Final;
        }

        public bool CheckRunning()
        {
            return (state == ThreadState.Running);
        }

        private void CallFun(object parm)
        {
            Debug.Log("Severce thread CallFun Beign");
            NetSocket client = parm as NetSocket;
            client.Receive();
        }
    }
}