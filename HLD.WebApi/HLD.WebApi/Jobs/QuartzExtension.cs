using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace HLD.WebApi.Jobs
{
    public static class QuartzExtension
    {

        public static void AddQuartz(this IServiceCollection services)
        {
            services.Add(new ServiceDescriptor(typeof(IJob), typeof(QuartzJob), ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(typeof(IJob), typeof(UpdateSCOrderStatusJob), ServiceLifetime.Transient));
            services.AddSingleton<IJobFactory, ScheduledJobFactory>();
            //services.AddSingleton<IJobDetail>(provider =>
            //{
            //    return JobBuilder.Create<QuartzJob>()
            //      .WithIdentity("Sample.job")
            //      .Build();
            //});

            //services.AddSingleton<ITrigger>(provider =>
            //{
            //    return TriggerBuilder.Create()
            //    .WithIdentity($"Sample.trigger")
            //    .WithSimpleSchedule
            //     (s =>
            //        s.WithInterval(TimeSpan.FromMinutes(1))
            //        .RepeatForever()
            //     )
            //     .Build();
            //});



            //services.AddSingleton<IJobDetail>(provider =>
            //{
            //    return JobBuilder.Create<UpdateSCOrderStatusJob>()
            //      .WithIdentity("updateStatus")
            //      .Build();
            //});

            //services.AddSingleton<ITrigger>(provider =>
            //{
            //    return TriggerBuilder.Create()
            //    .WithIdentity($"updateStatus.trigger")
            //    .WithSimpleSchedule
            //     (s =>
            //        s.WithInterval(TimeSpan.FromMinutes(3))
            //        .RepeatForever()
            //     )
            //     .Build();
            //});


            IJobDetail firstJob = JobBuilder.Create<QuartzJob>()
               .WithIdentity("firstJob")
               .Build();

            ITrigger firstTrigger = TriggerBuilder.Create()
                             .WithIdentity("firstTrigger")
                             .WithSimpleSchedule
                 (s =>
                    s.WithInterval(TimeSpan.FromMinutes(3))
                    .RepeatForever()
                 )
                             .Build();


            IJobDetail secondJob = JobBuilder.Create<UpdateSCOrderStatusJob>()
                           .WithIdentity("secondJob")
                           .Build();

            ITrigger secondTrigger = TriggerBuilder.Create()
                             .WithIdentity("secondTrigger")
                             .WithSimpleSchedule
                 (s =>
                    s.WithInterval(TimeSpan.FromMinutes(3))
                    .RepeatForever()
                 )
                             .Build();

            var properties = new NameValueCollection
            {
                { "quartz.threadPool.threadCount", "2" }
            };

            services.AddSingleton<IScheduler>(provider =>
            {
                var schedulerFactory = new StdSchedulerFactory(properties);
                var scheduler = schedulerFactory.GetScheduler().Result;
                scheduler.Start();
                scheduler.ScheduleJob(firstJob, firstTrigger);
                scheduler.ScheduleJob(secondJob, secondTrigger);
                scheduler.JobFactory = provider.GetService<IJobFactory>();
 
                return scheduler;
            });
        }

        public static void UseQuartz(this IApplicationBuilder app)
        {
            app.ApplicationServices.GetService<IScheduler>()
                .ScheduleJob(app.ApplicationServices.GetService<IJobDetail>(),
                app.ApplicationServices.GetService<ITrigger>()
                );
        }
    }
}
