using UnityEngine;
using System.Collections;
using System;

namespace XFramework.Net
{
    public class ByteBuffer
    {
        private uint size;
        private uint curSize;
        private byte[] buffer;

        public ByteBuffer()
        {
            size = 256;
            curSize = 0;
            buffer = new byte[size];
        }

        public ByteBuffer(uint size)
        {
            this.size = size;
            this.curSize = 0;
            buffer = new byte[size];
        }

        public byte[] GetData()
        {
            ushort dataSize = ConvertToUShortAll(buffer);
            if (dataSize <= curSize)
            {
                byte[] buf = new byte[dataSize];
                Array.Copy(buffer, 0, buf, 0, dataSize);
                return buf;
            }
            return null;
        }

        public static ushort ConvertToUShort(byte[] buffer)
        {
            return BitConverter.ToUInt16(buffer, 0);
        }

        public static ushort ConvertToUShortAll(byte[] buffer)
        {
            byte[] dataBytes = new byte[sizeof(int)];
            Array.Copy(buffer, 0, dataBytes, 0, dataBytes.Length);
            Array.Reverse(dataBytes);
            int size = BitConverter.ToInt32(dataBytes, 0);
            return (ushort)size;

            // return (ushort)(BitConverter.ToUInt16(buffer, 0) + sizeof(uint));
        }

        public uint CurSize
        {
            get
            {
                return curSize;
            }
        }

        public bool CheckOk()
        {
            return ((curSize != 0) && (curSize >= ConvertToUShortAll(buffer)));
        }

        public void Push(byte[] val, uint len)
        {
            if ((size - curSize) < len)
            {
                size += len;
                byte[] buf = new byte[size];
                Array.Copy(buffer, 0, buf, 0, curSize);
                buffer = buf;
            }
            Array.Copy(val, 0, buffer, curSize, len);
            curSize += len;
        }

        public void Pop()
        {
            ushort dataSize = ConvertToUShortAll(buffer);
            if (dataSize <= curSize)
            {
                curSize -= dataSize;
                Array.Copy(buffer, dataSize, buffer, 0, curSize);
            }
        }
    }
}