using DataAccess.Helper;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HLD.WebApi.Jobs
{
    public class QuartzServicesUtilities
    {
        public static void StartJob<TJob>(IScheduler scheduler, string cron)
               where TJob : IJob
        {
            var jobName = typeof(TJob).FullName;

            var job = JobBuilder.Create<TJob>()
                .WithIdentity(jobName)
                .Build();

            if (jobName == "HLD.WebApi.Jobs.UpdateZincOrder_InProgressSuccess_Job")
            {
                var trigger = TriggerBuilder.Create()
             .ForJob(job)
             .WithSimpleSchedule
              (s =>
                s.WithIntervalInHours(1)
                 .RepeatForever()
              )
             .StartNow()
             .Build();

                scheduler.ScheduleJob(job, trigger);
                //var trigger = TriggerBuilder.Create()
                //         .ForJob(job)
                //         .WithDailyTimeIntervalSchedule
                //          (s =>
                //            s.WithIntervalInHours(6)
                //             .OnEveryDay()
                //          )
                //         .Build();

                //scheduler.ScheduleJob(job, trigger);
            }

            //if (jobName == "HLD.WebApi.Jobs.ProductWarehouseQtyJob")
            //{
            //    var trigger = TriggerBuilder.Create()
            //  .ForJob(job)
            //  .WithSimpleSchedule
            //   (s =>
            //     s.WithIntervalInHours(1)
            //      .RepeatForever()
            //   )
            //  .StartNow()
            //  .Build();

            //    scheduler.ScheduleJob(job, trigger);
            //}

            if (jobName == "HLD.WebApi.Jobs.WarehouseProductQtyJob")
            {
                var trigger = TriggerBuilder.Create()
                .ForJob(job)
                .WithSimpleSchedule
                (s =>
                s.WithIntervalInHours(6)
                .RepeatForever()
                )
                .StartNow()
                .Build();

                scheduler.ScheduleJob(job, trigger);
            }

            if (jobName == "HLD.WebApi.Jobs.CreateOrderInSellerCloud")
            {
                var trigger = TriggerBuilder.Create()
             .ForJob(job)
             .WithSimpleSchedule
              (s =>
                s.WithIntervalInMinutes(10)
                 .RepeatForever()
              )
             .StartNow()
             .Build();

                scheduler.ScheduleJob(job, trigger);
            }
            if (jobName == "HLD.WebApi.Jobs.S3FileReadingJob")
            {
                var trigger = TriggerBuilder.Create()
             .ForJob(job)
             .WithSimpleSchedule
              (s =>
                s.WithIntervalInMinutes(1)
                 .RepeatForever()
              )
             .StartNow()
             .Build();

                scheduler.ScheduleJob(job, trigger);
            }
            if (jobName == "HLD.WebApi.Jobs.CompressImagesJob")
            {
                var trigger = TriggerBuilder.Create()
             .ForJob(job)
             .WithSimpleSchedule
              (s =>
                s.WithIntervalInHours(6)
                 .RepeatForever()
              )
             .StartNow()
             .Build();

                scheduler.ScheduleJob(job, trigger);

            }

            if (jobName == "HLD.WebApi.Jobs.GetSellerOrderNotes")
            {
                var trigger = TriggerBuilder.Create()
             .ForJob(job)
             .WithSimpleSchedule
              (s =>
                s.WithIntervalInHours(1)
                 .RepeatForever()
              )
             .StartNow()
             .Build();

                scheduler.ScheduleJob(job, trigger);

            }
            if (jobName == "HLD.WebApi.Jobs.GetPOOrderUpdatesFromSellerCloudJob")
            {
                var trigger = TriggerBuilder.Create()
             .ForJob(job)
             .WithSimpleSchedule
              (s =>
                s.WithIntervalInHours(1)
                 .RepeatForever()
              )
             .StartNow()
             .Build();

                scheduler.ScheduleJob(job, trigger);

            }
            if (jobName == "HLD.WebApi.Jobs.ZincASINWatchListJob")
            {
                //var trigger = TriggerBuilder.Create()
                //    .WithIdentity("watchlist_trigger", "watchlist_group")
                //     .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(23, 00))
                //    .ForJob(job)
                //   .Build();

                //scheduler.ScheduleJob(job, trigger);

                var trigger = TriggerBuilder.Create()
                 .ForJob(job)
                 .WithSimpleSchedule
                  (s =>
                    s.WithIntervalInHours(12)
                     .RepeatForever()
                  )
                 .StartNow()
                 .Build();
                scheduler.ScheduleJob(job, trigger);

            }
            if (jobName == "HLD.WebApi.Jobs.ReadExcelFile")
            {
                var trigger = TriggerBuilder.Create()
                    .WithIdentity("watchlist_trigger1", "watchlist_group1")
                     .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(07, 00))
                    .ForJob(job)
                   .Build();
                scheduler.ScheduleJob(job, trigger);

                //   var trigger = TriggerBuilder.Create()
                //.ForJob(job)
                //.WithSimpleSchedule
                // (s =>
                //   s.WithIntervalInHours(1)
                //    .RepeatForever()
                // )
                //.StartNow()
                //.Build();
                //   scheduler.ScheduleJob(job, trigger);
            }
            if (jobName == "HLD.WebApi.Jobs.GetProductCatalogDetail")
            {
                //var trigger = TriggerBuilder.Create()
                //    .WithIdentity("watchlist_trigger2", "watchlist_group2")
                //     .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(01, 00))
                //    .ForJob(job)
                //   .Build();
                //scheduler.ScheduleJob(job, trigger);

                var trigger = TriggerBuilder.Create()
             .ForJob(job)
             .WithSimpleSchedule
              (s =>
                s.WithIntervalInHours(12)
                 .RepeatForever()
              )
             .StartNow()
             .Build();
                scheduler.ScheduleJob(job, trigger);
            }
            if (jobName == "HLD.WebApi.Jobs.GetOrdersFromBestBuyJob")
            {
                var trigger = TriggerBuilder.Create()
         .ForJob(job)
         .WithSimpleSchedule
          (s =>
            s.WithIntervalInMinutes(10)
             .RepeatForever()
          )
         .StartNow()
         .Build();

                scheduler.ScheduleJob(job, trigger);

            }
            if (jobName == "HLD.WebApi.Jobs.ZincWatchListJobsNew")
            {
                var trigger = TriggerBuilder.Create()
         .ForJob(job)
         .WithSimpleSchedule
          (s =>
            s.WithIntervalInHours(10)
             .RepeatForever()
          )
         .StartNow()
         .Build();

                scheduler.ScheduleJob(job, trigger);

            }
            else
            {
                var trigger = TriggerBuilder.Create()
             .ForJob(job)
             .WithSimpleSchedule
              (s =>
                s.WithIntervalInMinutes(30)
                 .RepeatForever()
              )
             .StartNow()
             .Build();

                scheduler.ScheduleJob(job, trigger);
            }
        }
    }
}
