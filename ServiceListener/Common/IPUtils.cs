using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace project_install.Common
{
    class IPUtils
    {

        /// <summary>
        /// 获取本机物理网卡的ip
        /// </summary>
        /// <returns></returns>
        public static ArrayList IPAddress()
        {
            ArrayList IPs = new ArrayList();
            IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress ip in localIPs)
            {
                //根据AddressFamily判断是否为ipv4,如果是InterNetWork则为ipv6
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    IPs.Add(ip.ToString());
            }
            return IPs;
        }

        public static bool PortInUse(int port)
        {
            bool inUse = false;

            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipEndPoints = ipProperties.GetActiveTcpListeners();

            foreach (IPEndPoint endPoint in ipEndPoints)
            {
                if (endPoint.Port == port)
                {
                    inUse = true;
                    break;
                }
            }

            return inUse;
        }

    }
}
