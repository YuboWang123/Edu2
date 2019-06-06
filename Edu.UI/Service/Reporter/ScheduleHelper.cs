using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Edu.UI.Service.Reporter
{
    public class ScheduleHelper
    {

        private StdSchedulerFactory factory;
        private NameValueCollection props;
        private IScheduler scheduler;
        private string reportDirectory = string.Format("~/reports/{0}/", DateTime.Now.ToString("yyyy-MM"));
       
        private IJobDetail job;

        private ITrigger trigger;
        public ScheduleHelper()
        {
            props = new NameValueCollection
            {
                { "quartz.serializer.type", "binary" },
                {"quartz.threadPool.threadCount","3" }
            };

            factory = new StdSchedulerFactory(props);
            reportDirectory = System.Web.Hosting.HostingEnvironment.MapPath(reportDirectory);
            if (!Directory.Exists(reportDirectory))
            {
                Directory.CreateDirectory(reportDirectory);
            }
        }

        public async void End()
        {
            await scheduler.Shutdown();
        }

        public async void StartLogService()
        {
            scheduler = await factory.GetScheduler();
            await scheduler.Start();
            job = JobBuilder.Create<LogSv>()
                .WithIdentity("loger", "group1")
                .UsingJobData("dir",reportDirectory)
                .Build();
            trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(5)
                    .RepeatForever())
                .Build();


            await scheduler.ScheduleJob(job, trigger);

         
        }


        public async void StartMailService()
        {
             scheduler = await factory.GetScheduler();
            await scheduler.Start();
            job = JobBuilder.Create<EmailSv>()
                .WithIdentity("mail", "group1")
                .UsingJobData("dir",reportDirectory)
                .Build();
            trigger = TriggerBuilder.Create()
                .WithIdentity("trigger2", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(13)
                    .RepeatForever())
                .Build();

      
            await scheduler.ScheduleJob(job, trigger);

            // some sleep to show what's happening
           // await Task.Delay(TimeSpan.FromDays(7));

        }
    }
}