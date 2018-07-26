using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using UnityEngine;
namespace XFramework.Net
{
    public class IPConvert
    {

        [DllImport("__Internal")]
        private static extern string getIPv6(string mHost, string mPort);

        public static string GetIPv6(string mHost, string mPort)
        {
#if UNITY_IPHONE && !UNITY_EDITOR
		    return getIPv6(mHost, mPort);
#else
            return mHost;
#endif
        }

        public static void getIPType(String serverIp, String serverPorts, out String newServerIp, out AddressFamily mIPType)
        {
            mIPType = AddressFamily.InterNetwork;
            newServerIp = serverIp;
            try
            {
                string mIPv6 = GetIPv6(serverIp, serverPorts);
                if (!string.IsNullOrEmpty(mIPv6))
                {
                    string[] m_StrTemp = System.Text.RegularExpressions.Regex.Split(mIPv6, "&&");
                    if (m_StrTemp != null && m_StrTemp.Length >= 2)
                    {
                        string IPType = m_StrTemp[1];
                        if (IPType == "ipv6")
                        {
                            newServerIp = m_StrTemp[0];
                            mIPType = AddressFamily.InterNetworkV6;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log(string.Format("Ipv6 转换失败 {0}", e.Message));
            }
        }
    }
}