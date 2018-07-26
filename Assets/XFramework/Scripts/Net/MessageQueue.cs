using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace XFramework.Net
{
    public class MessageQueue
    {
        private Queue<Message> msgQueue;
        private Locker locker;

        public MessageQueue()
        {
            msgQueue = new Queue<Message>();
            locker = new Locker();
        }

        public void Enqueue(Message msg)
        {
            locker.Lock();
            msgQueue.Enqueue(msg);
            locker.UnLock();
        }

        public Message Dequeue()
        {
            locker.Lock();
            Message msg = msgQueue.Dequeue();
            locker.UnLock();
            return msg;
        }

        public bool IsEmpty
        {
            get
            {
                return (msgQueue.Count == 0);
            }
        }

        public int GetCount()
        {
            return msgQueue.Count;
        }
    }
}