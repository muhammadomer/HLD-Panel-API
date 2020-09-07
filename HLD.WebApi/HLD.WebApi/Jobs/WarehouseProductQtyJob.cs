using DataAccess.DataAccess;
using DataAccess.Helper;
using DataAccess.ViewModels;
using Newtonsoft.Json;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace HLD.WebApi.Jobs
{
    [DisallowConcurrentExecution]
    public class WarehouseProductQtyJob : IJob
    {
        IConnectionString _connectionString = null;
        ServiceReference1.AuthHeader authHeader = null;
        ProductWarehouseQtyDataAccess productWarehouseQtyDataAccess = null;
        ProductDataAccess productDataAccess = null;
        private readonly ILogger logger;
        ChannelDecrytionDataAccess channelDecrytionDataAccess = null;
        EncDecChannel _EncDecChannel = null;
        GetChannelCredViewModel _getChannelCredViewModel = null;
        public WarehouseProductQtyJob(IConnectionString connectionString, ILogger<WarehouseProductQtyJob> _logger)
        {
            _connectionString = connectionString;

            productWarehouseQtyDataAccess = new ProductWarehouseQtyDataAccess(_connectionString);
            productDataAccess = new ProductDataAccess(_connectionString);
            _EncDecChannel = new EncDecChannel(_connectionString);
            channelDecrytionDataAccess = new ChannelDecrytionDataAccess(connectionString);
            logger = _logger;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            int status = channelDecrytionDataAccess.CheckZincJobsStatus("WarehouseProductQtyJob");
            if (status == 1)
            {
                logger.LogInformation("WarehouseProductQtyJob Job Started At =>" + DateTime.Now.ToString());
                try
                {
                    List<string> skuList = productDataAccess.GetProductDetailForWarehouseQtyUpdate_ALLSKU();
                    foreach (var item in skuList)
                    {

                        GetWarahouseQty(item);
                    }
                    //GetWarahouseQty("D7-1988-18");

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message + " :");
                }
            }


            await Task.CompletedTask;
        }
        public void GetWarahouseQty(string SKU)
        {

            string ApiURL = "";
            ApiURL = "https://lp.api.sellercloud.com/rest/api";
            AuthenticateSCRestViewModel authenticate = new AuthenticateSCRestViewModel();
            _getChannelCredViewModel = _EncDecChannel.DecryptedData("sellercloud");
            authenticate = _EncDecChannel.AuthenticateSCForIMportOrder(_getChannelCredViewModel, ApiURL);

            List<WareHouseProductQty> warehouselist = new List<WareHouseProductQty>();
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest
                    .Create(ApiURL + "/Inventory/Warehouses?productID=" + SKU);
                request.Method = "GET";
                request.Accept = "application/json;";
                request.ContentType = "application/json";
                request.Headers["Authorization"] = "Bearer " + authenticate.access_token;

                string strResponse = "";
                using (WebResponse webResponse = request.GetResponse())
                {
                    using (StreamReader stream = new StreamReader(webResponse.GetResponseStream()))
                    {
                        strResponse = stream.ReadToEnd();
                    }
                }
                warehouselist = JsonConvert.DeserializeObject<List<WareHouseProductQty>>(strResponse);

                var status = productWarehouseQtyDataAccess.SaveWarehouseProductQty_New(warehouselist);
                var Item = warehouselist.Where(s => s.WarehouseID != 364 && s.WarehouseID != 365).ToList();
                logger.LogInformation("WarehouseProductQtyJob Job SKU =>" + SKU.ToString());
                var PhyQtyt = Item.Sum(s => s.PhysicalQty);
                productWarehouseQtyDataAccess.UpdateProductCatalogByWHSKU(PhyQtyt, SKU);
            }
            catch (Exception exp)
            {
                logger.LogInformation("WarehouseProductQtyJob Job Exceptions =>" + exp.ToString());
                System.Diagnostics.Debug.WriteLine(exp.Message + " :");
            }

        }

        //public void GetWarahouseQty_New(List<string> skuList)
        //{
        //    List<WareHouseViewModel> wareHouses = new List<WareHouseViewModel>();
        //    wareHouses = productWarehouseQtyDataAccess.GetWareHousesNamesList();

        //    int i = 0;
        //    string SKU = "IPXR-VJ-RD";
        //    //foreach (var SKU in skuList)
        //    {
        //        i++;
        //        logger.LogInformation("WarehouseProductQtyJob Job Updating SKU=>" + SKU.ToString() + "RecordesUpdating" + i.ToString());
        //        string ApiURL = "";
        //        ApiURL = "https://lp.api.sellercloud.com/rest/api";
        //        AuthenticateSCRestViewModel authenticate = new AuthenticateSCRestViewModel();
        //        _getChannelCredViewModel = _EncDecChannel.DecryptedData("sellercloud");
        //        authenticate = _EncDecChannel.AuthenticateSCForIMportOrder(_getChannelCredViewModel, ApiURL);

        //        List<WareHouseProductQty> warehouselist = new List<WareHouseProductQty>();
        //        try
        //        {
        //            HttpWebRequest request = (HttpWebRequest)WebRequest
        //                .Create(ApiURL + "/Inventory/Warehouses?productID=" + SKU);
        //            request.Method = "GET";
        //            request.Accept = "application/json;";
        //            request.ContentType = "application/json";
        //            request.Headers["Authorization"] = "Bearer " + authenticate.access_token;

        //            string strResponse = "";
        //            using (WebResponse webResponse = request.GetResponse())
        //            {
        //                using (StreamReader stream = new StreamReader(webResponse.GetResponseStream()))
        //                {
        //                    strResponse = stream.ReadToEnd();
        //                }
        //            }
        //            warehouselist = JsonConvert.DeserializeObject<List<WareHouseProductQty>>(strResponse);

        //            //var status = productWarehouseQtyDataAccess.SaveWarehouseProductQty_New(warehouselist, wareHouses);
        //            var Item = warehouselist.Where(s => s.WarehouseID != 364 && s.WarehouseID != 365).ToList();
        //            logger.LogInformation("WarehouseProductQtyJob Job SKU =>" + SKU.ToString());
        //            var PhyQtyt = Item.Sum(s => s.PhysicalQty);
        //            productWarehouseQtyDataAccess.UpdateProductCatalogByWHSKU(PhyQtyt, SKU);
        //        }
        //        catch (Exception exp)
        //        {
        //            logger.LogInformation("WarehouseProductQtyJob Job Exceptions =>" + exp.ToString());
        //            System.Diagnostics.Debug.WriteLine(exp.Message + " :");
        //            //      continue;
        //        }
        //    }
        //}

    }
}
