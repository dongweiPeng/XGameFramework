using UnityEngine;
using System.Collections;
using System.Threading;
namespace XFramework.Net
{
    public class Locker
    {
        private object syncObj;
        public Locker()
        {
            syncObj = new object();
        }

        public void Lock()
        {
            if (syncObj != null)
            {
                Monitor.Enter(syncObj);
            }
        }

        public void UnLock()
        {
            if (syncObj != null)
            {
                Monitor.Exit(syncObj);
            }
        }
    }
}
