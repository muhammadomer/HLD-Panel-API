using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace HLD.WebApi.Jobs
{
    public class QuartzStartup
    {
        private IScheduler _scheduler; // after Start, and until shutdown completes, references the scheduler object

        // starts the scheduler, defines the jobs and the triggers
        public void Start()
        {
            if (_scheduler != null)
            {
                throw new InvalidOperationException("Already started.");
            }

            var properties = new NameValueCollection
            {
                { "quartz.scheduler.instanceName", "QuartzWithCore" },
                { "quartz.scheduler.instanceId", "QuartzWithCore" },
                { "quartz.jobStore.type", "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz" },
                { "quartz.jobStore.useProperties", "true" },                
                { "quartz.threadPool.threadCount", "2" },
                { "quartz.serializer.type", "json" },
            };

            var schedulerFactory = new StdSchedulerFactory(properties);
            _scheduler = schedulerFactory.GetScheduler().Result;
            _scheduler.Start().Wait();

            var userEmailsJob = JobBuilder.Create<QuartzJob>()
                .WithIdentity("SendUserEmails")
                .Build();
            var userEmailsTrigger = TriggerBuilder.Create()
                .WithIdentity("UserEmailsCron")
                .StartNow()
                .WithSimpleSchedule
                 (s =>
                    s.WithInterval(TimeSpan.FromMinutes(6))
                    .RepeatForever()
                 )
                .Build();

            var userEmailsJob1 = JobBuilder.Create<UpdateSCOrderStatusJob>()
               .WithIdentity("SendUserEmails1")
               .Build();
            var userEmailsTrigger1 = TriggerBuilder.Create()
                .WithIdentity("UserEmailsCron1")
                .StartNow()
                .WithSimpleSchedule
                 (s =>
                    s.WithInterval(TimeSpan.FromMinutes(6))
                    .RepeatForever()
                 )
                .Build();

            _scheduler.ScheduleJob(userEmailsJob, userEmailsTrigger).Wait();
            _scheduler.ScheduleJob(userEmailsJob1, userEmailsTrigger1).Wait();
           
        }

        // initiates shutdown of the scheduler, and waits until jobs exit gracefully (within allotted timeout)
        public void Stop()
        {
            if (_scheduler == null)
            {
                return;
            }

            // give running jobs 30 sec (for example) to stop gracefully
            if (_scheduler.Shutdown(waitForJobsToComplete: true).Wait(30000))
            {
                _scheduler = null;
            }
            else
            {
                // jobs didn't exit in timely fashion - log a warning...
            }
        }
    }
}
 
