using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Management;
using System.Web;

namespace Vulcan.AspNetMvc.Utils
{
    public class IPUtils
    {
        private static string _LocalIP = string.Empty;
        /// <summary>
        /// 获取本机IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIP()
        {
            if (string.IsNullOrEmpty(_LocalIP))
            {
                try
                {
                    System.Net.IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
                    for (int i = 0; i < addressList.Length; i++)
                    {
                        _LocalIP = addressList[i].ToString();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("获取本机IP地址出错", ex);
                }
            }
            return _LocalIP;
        }

        private static string _LocalMAC = string.Empty;
        /// <summary>
        /// 获取本机MAC地址
        /// </summary>
        /// <returns></returns>
        public static string GetLocalMAC()
        {
            if (string.IsNullOrEmpty(_LocalMAC))
            {
                try
                {

                    ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                    ManagementObjectCollection moc = mc.GetInstances();
                    foreach (ManagementObject mo in moc)
                    {
                        if (mo["IPEnabled"].ToString() == "True")
                        {
                            _LocalMAC = mo["MacAddress"].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("获取本机MAC地址出错", ex);
                }
            }
            return _LocalMAC;
        }

        /// <summary>
        /// 获取web系统客户端IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetWebClientIP()
        {
            string usip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (String.IsNullOrEmpty(usip))
                usip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            return usip;
        }

    }
}
