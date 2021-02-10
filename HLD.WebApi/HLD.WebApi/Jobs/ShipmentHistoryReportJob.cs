using Amazon.Runtime.Internal.Util;
using DataAccess.DataAccess;
using DataAccess.Helper;
using DataAccess.ViewModels;
using Microsoft.Extensions.Configuration;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HLD.WebApi.Jobs
{
    [DisallowConcurrentExecution]
    public class ShipmentHistoryReportJob : IJob
    {
        IConnectionString _connectionString = null;


        ShipmentDataAccess _shipmentDataAccess = null;
        private readonly IConfiguration _configuration;
   
        public ShipmentHistoryReportJob(IConnectionString connectionString, IConfiguration configuration)
        {
            _connectionString = connectionString;
            this._configuration = configuration;

            _shipmentDataAccess = new ShipmentDataAccess(_connectionString);
        }
        public async Task Execute(IJobExecutionContext context)
        {

            ShipmentHistoryDetals();


              await Task.CompletedTask;

        }
        public void ShipmentHistoryDetals()
        {
            string DateTo = DateTime.Now.ToString("yyyy-MM-dd"); ;
            string DateFrom = DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd");
            List<ShipmentHistoryViewModel> historyViewModels = new List<ShipmentHistoryViewModel>();
            historyViewModels= _shipmentDataAccess.GetShipmentHistoryListforReport(DateTo, DateFrom, 1278, "", "","",10000,0,"");
            //var list = historyViewModels.GroupBy(x => new { x.SKU, x.ShipmentId })
            //  .Select(p => new
            //  {
            //      p.Key.ShipmentId,
            //      p.Key.SKU,
            //      POIDs = p.Select(i => new POIDs { POId = i.POId, ShipedQty = i.ShipedQty }),
            //      ShippedPO = p.Sum(o => o.ShipedQty),
            //      Data = p.Select(s => new ShipmentHistoryViewModel
            //      {
            //          CompressedImage = s.CompressedImage,
            //          ImageName = s.ImageName,
            //          Title = s.Title,
            //          Vendor = s.Vendor,
            //          VendorId = s.VendorId,
            //          Type = s.Type,
            //          Status = s.Status,
            //          ReceivedDate = s.ReceivedDate,
            //          ExpectedDelivery=s.ExpectedDelivery,
            //          ShippedDate = s.ShippedDate,
            //          ShipedQty = s.ShipedQty,
            //          ReceivedQty = s.ReceivedQty,
            //          CreatedOn = s.CreatedOn,
            //          TrakingNumber = s.TrakingNumber,

            //      })
            //  });

            //foreach (var _historyitem in list)
            //{

            //}
            _shipmentDataAccess.UpdateShipmentHistoryReport(historyViewModels);
        }
    }
}
