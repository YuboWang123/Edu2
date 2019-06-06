using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Web;
using Quartz;

namespace Edu.UI.Service.Reporter
{

    public class LogSv:IJob
    {
        public LogSv()
        {

        }

        public string GetIP()
        {
            try
            {
                string HostName = Dns.GetHostName(); //得到主机名
                IPHostEntry IpEntry = Dns.GetHostEntry(HostName);
                for (int i = 0; i < IpEntry.AddressList.Length; i++)
                {
                    //从IP地址列表中筛选出IPv4类型的IP地址
                    //AddressFamily.InterNetwork表示此IP为IPv4,
                    //AddressFamily.InterNetworkV6表示此地址为IPv6类型
                    if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        return IpEntry.AddressList[i].ToString();
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public Task Execute(IJobExecutionContext context)
        {
            string reportDirectory = context.MergedJobDataMap["dir"].ToString();
            var dailyReportFullPath = string.Format("{0}report_{1}.log", reportDirectory, DateTime.Now.Day);
            var logContent = string.Format("Date:{0}==>>ip: {1}{2}", DateTime.Now, GetIP(), Environment.NewLine);
            return Task.Run(()=> File.AppendAllText(dailyReportFullPath, logContent));
        }
    }
}