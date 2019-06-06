using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Policy;
using System.Web;
using IPTools.Core;
using MaxMind.Db;
namespace Edu.UI.Service
{
    public class ipSvc
    {
        /// <summary>
        /// get global ip infos
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string,object> GetAddr()
        {
            using (var dr = new Reader(HttpContext.Current.Server.MapPath("~/App_Data/GeoLite2-City.mmdb")))
            {
                string ipAdd = GetLocalIPAddress();
                if (ipAdd.StartsWith("192.168") || ipAdd.StartsWith("127"))
                {
                    return null;
                }

                var ip = IPAddress.Parse(ipAdd);
                var data = dr.Find<Dictionary<string, object>>(ip);
                return data;
            }
        }

        /// <summary>
        /// get chinese ip infos
        /// </summary>
        /// <returns></returns>
        public static IpInfo GetCnIpInfo()
        {
            string ipAdd = GetLocalIPAddress();
            if (ipAdd.StartsWith("192.168") || ipAdd.StartsWith("127"))
            {
                return null;
            }

            return IpTool.Search(ipAdd);
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
    }
}