using System;
using System.IO;
using System.Text;
namespace XFramework.Net
{
    public enum ThreadState
    {
        None,
        Running,
        Final,
    }

    public enum SocketState
    {
        None,
        Connect,
        Disconnect,
    }

    public enum ProcessorState
    {
        None,
        Platform,
        Access,
        Reconnect,
        Over,
    }

    public enum OfflineType
    {
        /// <summary>
        /// 无掉线
        /// </summary>
        None,
        /// <summary>
        /// 服务器踢下线
        /// </summary>
        ByServer,
        /// <summary>
        /// 网络不好
        /// </summary>
        NetEnvironment,
    }

    public class NetTools
    {
        /// <summary>
        /// 封包 java服务器，需要转字节序
        /// 小樱服务器编码 java服务器
        /// </summary>
        public static byte[] PackMesage(Message msg)
        {
            ushort dataLen = 0;
            if (msg.msgData != null)
            {
                dataLen = (ushort)msg.msgData.Length;
            }
            short key = (short)msg.msgkey;
            //包的总长度 = 文件头长度 + cmd长度 ＋　数据长度
            int packLen = sizeof(int) + sizeof(short) + dataLen;
            byte[] send = new byte[packLen];
            byte[] packBytes = BitConverter.GetBytes(packLen);
            Array.Reverse(packBytes);
            byte[] cmdBytes = BitConverter.GetBytes((short)msg.msgkey);
            Array.Reverse(cmdBytes);
            byte[] dataBytes = msg.msgData;

            Array.Copy(packBytes, 0, send, 0, packBytes.Length);
            Array.Copy(cmdBytes, 0, send, packBytes.Length, cmdBytes.Length);
            Array.Copy(dataBytes, 0, send, packBytes.Length + cmdBytes.Length, dataBytes.Length);
            //  PrintBytes("发送字节>>", send);
            return send;
        }
        /* c++ zqgame服务器
        public static byte[] PackMesage(Message msg)
        {
            ushort dataLen = 0;
            if (msg.msgData != null)
            {
                dataLen = (ushort)msg.msgData.Length;
            }
            short key = (short)msg.msgkey;
            //文件头长度
            int headLen = sizeof(int);
            //包的总长度 = 文件头长度＋　数据长度
            int packLen = dataLen + headLen;

            byte[] send = new byte[packLen];

            byte[] dataLength = BitConverter.GetBytes(dataLen);
            byte[] keyByte = BitConverter.GetBytes(key);

            Array.Copy(dataLength, 0, send, 0, dataLength.Length);
            Array.Copy(keyByte, 0, send, dataLength.Length, keyByte.Length);

            if (msg.msgData != null)
            {
                Array.Copy(msg.msgData, 0, send, headLen, dataLen);
            }

            string log = "";
            foreach (byte b in send)
            {
                log += b + ",";
            }
            UnityEngine.Debug.Log("发送字节序列>>" + log);
            return send;
        } 

        /// <summary>
        /// 解包
        /// </summary>
        public static Message UnPackMessage(byte[] data){
            ushort dataSize = ByteBuffer.ConvertToUShortAll (data);
            if(dataSize<= data.Length){
                int len = dataSize - sizeof(uint);
                byte[] revBuf = new byte[len];
                Array.Copy (data, sizeof(int), revBuf, 0, len);

                Message msg = new Message ();
                msg.msgkey = (CmdNumber)BitConverter.ToUInt16 (data, sizeof(ushort));
                msg.msgData = revBuf;
                return msg;
            }
            return null;
        }*/

        public static Message UnPackMessage(byte[] data)
        {
            ushort dataSize = ByteBuffer.ConvertToUShortAll(data);
            if (dataSize <= data.Length)
            {
                byte[] cmdBytes = new byte[(sizeof(short))];
                Array.Copy(data, sizeof(int), cmdBytes, 0, cmdBytes.Length);
                Array.Reverse(cmdBytes);

                byte[] msgBytes = new byte[data.Length - (sizeof(short) + sizeof(int))];
                Array.Copy(data, (sizeof(short) + sizeof(int)), msgBytes, 0, msgBytes.Length);
                Message msg = new Message();
                int msgkey = BitConverter.ToInt16(cmdBytes, 0);
                msg.msgkey = (CmdNumber)msgkey;
                msg.msgData = msgBytes;
                return msg;
            }
            return null;
        }

        public static void PrintBytes(string title, byte[] send)
        {
            string log = "";
            foreach (byte b in send)
            {
                log += Convert.ToString(b, 16) + ",";
            }
            UnityEngine.Debug.Log(string.Format("{0} {1}", title, log));
        }

        /// <summary>
        /// 流转换为字节
        /// </summary>
        public static byte[] MemoryStreamToBytes(MemoryStream memstream, int offset)
        {
            memstream.Seek(offset, SeekOrigin.Begin);
            int bufLen = (int)memstream.Length - offset;
            if (bufLen < 0)
            {
                bufLen = 0;
            }
            byte[] bytes = new byte[bufLen];
            memstream.Read(bytes, 0, bufLen);
            memstream.Seek(0, SeekOrigin.Begin);

            return bytes;
        }

        public static byte[] UTF8StringToBytes(string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }

        public static string UTF8BytesToString(byte[] b)
        {
            if (b == null)
            {
                return "";
            }
            return Encoding.UTF8.GetString(b);
        }
    }
}