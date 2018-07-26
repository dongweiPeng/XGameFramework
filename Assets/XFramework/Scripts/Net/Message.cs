using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace XFramework.Net
{
    public class Message
    {
        public CmdNumber msgkey;
        public byte[] msgData;

        public Message(CmdNumber key, byte[] data)
        {
            this.msgkey = key;
            this.msgData = data;
        }

        public Message()
        {
            this.msgkey = CmdNumber.None;
            this.msgData = null;
        }

        public void Reset()
        {
            this.msgkey = CmdNumber.None;
            this.msgData = null;
        }
    }
}
