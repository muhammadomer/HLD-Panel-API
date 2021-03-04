using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DataAccess.Helper;

using DataAccess.ViewModels;
using Quartz;
using Quartz.Impl;
using HLD.WebApi.Jobs;
using Quartz.Spi;
using System.Net;
using HLD.WebApi.Interfaces;

namespace HLD.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

        }
        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();


            services.AddAuthorization();
            services.AddTransient<IConnectionString, ConnectionString>();
            //getting JWT sections
            var appSettingJwtSection = Configuration.GetSection("JwtSection");
            services.Configure<JwtAppSetting>(appSettingJwtSection);
            var appSettingJwt = appSettingJwtSection.Get<JwtAppSetting>();

            //jwt authentication
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        //UserDataAccess userDataAccess = new UserDataAccess();
                        //var _configurations = context.HttpContext.RequestServices.GetRequiredService<IUser>();
                        //var userId = int.Parse(context.Principal.Identity.Name);
                        //var user = _configurations.GetById(userId);
                        //if (user == null)
                        //{
                        //    // return unauthorized if user no longer exists
                        //    context.Fail("Unauthorized");
                        //}
                        return Task.CompletedTask;
                    }
                };

                options.SaveToken = true;
                options.RequireHttpsMetadata = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = appSettingJwt.Site,
                    ValidIssuer = appSettingJwt.Site,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettingJwt.SigningKey))
                };
            });
            services.AddTransient<ISendEmailOfNewOrder, SendNewOrderEmailAfter>();
            // to get sku quantity from seller cloud warehouse to local
            //services.UseQuartz(typeof(ProductWarehouseQtyJob));
            #region Uncomment Before Publish

            //order
            //services.UseQuartz(typeof(ReadExcelFile));
            //services.UseQuartz(typeof(WarehouseProductQtyJob));
            //services.UseQuartz(typeof(GetProductCatalogDetail));
            ////  update dashboard data
            //services.UseQuartz(typeof(InsertOrderSKU_ProfitHistory));
            ////  update Local sellercloud order status and payment status
            //services.UseQuartz(typeof(UpdateSCOrderStatusNewJob));
            //services.UseQuartz(typeof(UpdateQueuedJobLinkStatus));
            ////  to get sku qty for dropship none sku.from seller cloud
            //services.UseQuartz(typeof(BestBuyDropshipNoneWarehouseQtyFromSC_insert_Job));
            //  to update qty on bestbuy
            services.UseQuartz(typeof(BestBuyQuantityUpdateJob)); //never comment

            //services.UseQuartz(typeof(UpdateOrdersFromBestBuyJob));
            //services.UseQuartz(typeof(GetOrdersFromBestBuyJob));
            ////  tracking

            //services.UseQuartz(typeof(BestBuyTrackingExportJob));
            //services.UseQuartz(typeof(S3FileReadingJob));
            //services.UseQuartz(typeof(GetSellerOrderNotes));

            //services.UseQuartz(typeof(ShipmentHistoryReportJob));
            //services.UseQuartz(typeof(ZincASINWatchListNewJob));

            //services.UseQuartz(typeof(UpdateOrdersFromBestBuyNewJob));
            //services.UseQuartz(typeof(GetOrdersFromBestBuyNewJob));
            //services.UseQuartz(typeof(CreateOrderInSellerCloudNewJob));


            #endregion
            //  services.UseQuartz(typeof(QuartzJob)); // email job
            // services.UseQuartz(typeof(UpdateSCOrderStatusJob));
            // services.UseQuartz(typeof(CreateOrderInSellerCloud));
            //  services.UseQuartz(typeof(UpdateOrdersFromBestBuyNewJob));
            // services.UseQuartz(typeof(GetOrdersFromBestBuyJob));
            //  services.UseQuartz(typeof(BestBuyPriceUpdateJob));

            ///services.UseQuartz(typeof(ZincASINWatchListJob));

            //services.UseQuartz(typeof(ZincWatchListJobsNew));
            // services.UseQuartz(typeof(GetOrdersOfAllMarketPlacesJobs));
            //services.UseQuartz(typeof(UpdateZincOrder_InProcess_OrderRequestSent_Job));

            //services.UseQuartz(typeof(UpdateZincOrder_InProgressSuccess_Job));
            //  services.UseQuartz(typeof(GetPOOrderUpdatesFromSellerCloudJob));
            //services.UseQuartz(typeof(CompressImagesJob));
            //  services.UseQuartz(typeof(ReadEmail));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime, IScheduler scheduler, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }


            scheduler.ScheduleJob(app.ApplicationServices.GetService<IJobDetail>(), app.ApplicationServices.GetService<ITrigger>());
            var schedulerr = app.ApplicationServices.GetService<IScheduler>();
            //QuartzServicesUtilities.StartJob<ProductWarehouseQtyJob>(schedulerr, "");
            #region Un comment before Publish
            //QuartzServicesUtilities.StartJob<ReadExcelFile>(schedulerr, "");
            //QuartzServicesUtilities.StartJob<WarehouseProductQtyJob>(schedulerr, "");
            //QuartzServicesUtilities.StartJob<GetProductCatalogDetail>(schedulerr, "");
            //QuartzServicesUtilities.StartJob<InsertOrderSKU_ProfitHistory>(schedulerr, "");
            //QuartzServicesUtilities.StartJob<UpdateSCOrderStatusNewJob>(schedulerr, "");
            //QuartzServicesUtilities.StartJob<UpdateQueuedJobLinkStatus>(schedulerr, "");
            //QuartzServicesUtilities.StartJob<UpdateOrdersFromBestBuyJob>(schedulerr, "");
            //QuartzServicesUtilities.StartJob<BestBuyDropshipNoneWarehouseQtyFromSC_insert_Job>(schedulerr, "");
              QuartzServicesUtilities.StartJob<BestBuyQuantityUpdateJob>(schedulerr, "");

            //QuartzServicesUtilities.StartJob<BestBuyTrackingExportJob>(schedulerr, "");
            //QuartzServicesUtilities.StartJob<S3FileReadingJob>(schedulerr, "");
            //QuartzServicesUtilities.StartJob<GetSellerOrderNotes>(schedulerr, "");
            //QuartzServicesUtilities.StartJob<ShipmentHistoryReportJob>(schedulerr, "");
            //QuartzServicesUtilities.StartJob<GetOrdersFromBestBuyNewJob>(schedulerr, "");
            //QuartzServicesUtilities.StartJob<UpdateOrdersFromBestBuyNewJob>(schedulerr, "");
            //QuartzServicesUtilities.StartJob<CreateOrderInSellerCloud>(schedulerr, "");

            //QuartzServicesUtilities.StartJob<ZincASINWatchListNewJob>(schedulerr, "");

            //QuartzServicesUtilities.StartJob<CreateOrderInSellerCloudNewJob>(schedulerr, "");


            #endregion
            // QuartzServicesUtilities.StartJob<CreateOrderInSellerCloud>(schedulerr, "");
            //   QuartzServicesUtilities.StartJob<QuartzJob>(schedulerr, ""); //email job
            // QuartzServicesUtilities.StartJob<UpdateSCOrderStatusJob>(schedulerr, "");
            //  QuartzServicesUtilities.StartJob<GetOrdersFromBestBuyJob>(schedulerr, "");
            // QuartzServicesUtilities.StartJob<BestBuyPriceUpdateJob>(schedulerr, "");

            //QuartzServicesUtilities.StartJob<ZincASINWatchListJob>(schedulerr, "");

            //QuartzServicesUtilities.StartJob<ZincWatchListJobsNew>(schedulerr, "");
            // QuartzServicesUtilities.StartJob<GetPOOrderUpdatesFromSellerCloudJob>(schedulerr, "");
            // QuartzServicesUtilities.StartJob<GetOrdersOfAllMarketPlacesJobs>(schedulerr, "");
            //  QuartzServicesUtilities.StartJob<UpdateZincOrder_InProcess_OrderRequestSent_Job>(schedulerr, "");

            //  QuartzServicesUtilities.StartJob<UpdateZincOrder_InProgressSuccess_Job>(schedulerr, "");

            //     QuartzServicesUtilities.StartJob<CompressImagesJob>(schedulerr, "");
            // QuartzServicesUtilities.StartJob<ReadEmail>(schedulerr, "");



            var config = this.Configuration.GetAWSLoggingConfigSection();

            loggerFactory.AddAWSProvider(config);
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();


        }
    }
}
