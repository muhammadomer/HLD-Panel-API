using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Quartz;

namespace HLD.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private IScheduler _scheduler;
        public ValuesController(ISchedulerFactory schedulerFactory)
        {
            this._schedulerFactory = schedulerFactory;
        }
        [HttpGet]
        public async Task<string[]> Get()
        {
            // 1. Obtaining Scheduler by Scheduling Factory
            _scheduler = await _schedulerFactory.GetScheduler();
            // 2. Open Scheduler
            await _scheduler.Start();
            // 3. Create a trigger
            var trigger = TriggerBuilder.Create().
           WithSimpleSchedule(x=>x.WithIntervalInSeconds(2).RepeatForever())// Executes every two seconds
    .Build();
            // 4. Creating Tasks
            var jobDetail = JobBuilder.Create<MyJob>()
              .WithIdentity("job", "group")
              .Build();
            // 5. Binding Triggers and Taskers to Schedulers
            await _scheduler.ScheduleJob(jobDetail, trigger);
            return await Task.FromResult(new string[] { "value1", "value2" });
        }
    }

    public class MyJob : IJob // Create the implementation class of IJob and implement Excute method.
    {
        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() =>
            {
                //using (StreamWriter sw = new StreamWriter(@"C:\Users\Administrator\Desktop\error.log", true, Encoding.UTF8))
                //{
                //    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));
                //}

                Debug.WriteLine("testing");

            });
        }
    }
}
