
using DataAccess.DataAccess;
using DataAccess.Helper;
using DataAccess.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HLD.WebApi.Jobs
{
    [DisallowConcurrentExecution]
    public class GetPOOrderUpdatesFromSellerCloudJob : IJob
    {

        IConnectionString _connectionString = null;


        PurchaseOrderDataAccess _PODataAccess = null;
        AuthenticationSellercloud authenticationSellercloud = null;
        private readonly IConfiguration _configuration;
        private readonly ILogger logger;
        GetChannelCredViewModel _getChannelCredViewModel = null;
        ChannelDecrytionDataAccess channelDecrytionDataAccess = null;
        EncDecChannel _EncDecChannel = null;
        string ApiURL = null;
        ServiceReference1.AuthHeader authHeader = null;
        public GetPOOrderUpdatesFromSellerCloudJob(IConnectionString connectionString, ILogger<GetPOOrderUpdatesFromSellerCloudJob> _logger, IConfiguration configuration)
        {

            _connectionString = connectionString;

            this._configuration = configuration;
            ApiURL = _configuration.GetValue<string>("SCURL:URL");
            _EncDecChannel = new EncDecChannel(_connectionString);
            _PODataAccess = new PurchaseOrderDataAccess(_connectionString);
            authenticationSellercloud = new AuthenticationSellercloud();
            channelDecrytionDataAccess = new ChannelDecrytionDataAccess(_connectionString);
            this.logger = _logger;
            _getChannelCredViewModel = new GetChannelCredViewModel();
            _getChannelCredViewModel = _EncDecChannel.DecryptedData("sellercloud");

        }

        public async Task Execute(IJobExecutionContext context)
        {
            List<int> Orders = _PODataAccess.GetAllPurchaseOrdersToGetUpdate();
            if (Orders.Count > 0)
            {
                AuthenticateSCRestViewModel responses = new AuthenticateSCRestViewModel();
                responses = authenticationSellercloud.AuthenticateSC(_getChannelCredViewModel, ApiURL);
                foreach (var item in Orders)
                {
                    GetUpdateFromSellercloud(item, responses);
                }


            }

            await Task.CompletedTask;
        }

        private void GetUpdateFromSellercloud(int POOrderID, AuthenticateSCRestViewModel sCRestViewModel)
        {
            bool Issent = false;

            try
            {
                PurchaseOrderViewModel.PurchaseOrderData purchaseOrderData = new PurchaseOrderViewModel.PurchaseOrderData();
                PurchaseOrderDataViewModel purchaseOrderDataViewModel = new PurchaseOrderDataViewModel();
                List<PurchaseOrderItemsDataViewModel> purchaseOrderItemsDataViewModel = new List<PurchaseOrderItemsDataViewModel>();

                 purchaseOrderData = GetPurchaseOrderByIdFromSellerCloud(POOrderID.ToString(),sCRestViewModel);
                string POOrderItemsCommaSeprate = string.Join(",", purchaseOrderData.Items.Select(e => e.ProductID));

                _PODataAccess.DeleteRemovedPOItems(POOrderItemsCommaSeprate, POOrderID);

                purchaseOrderDataViewModel.CompanyId = purchaseOrderData.Purchase.CompanyId;
                purchaseOrderDataViewModel.CurrencyCode = purchaseOrderData.Purchase.CurrencyCode;
                purchaseOrderDataViewModel.OrderedOn = purchaseOrderData.Purchase.OrderedOn;
                purchaseOrderDataViewModel.POId = purchaseOrderData.Purchase.POId;
                purchaseOrderDataViewModel.DefaultWarehouseID = purchaseOrderData.Purchase.DefaultWarehouseID;
                purchaseOrderDataViewModel.VendorId = purchaseOrderData.Purchase.VendorId;
                purchaseOrderDataViewModel.POStatus = 1;
                foreach (var item in purchaseOrderData.Items)
                {
                    PurchaseOrderItemsDataViewModel ItemsDataViewModel = new PurchaseOrderItemsDataViewModel();
                    ItemsDataViewModel.ID = item.ID;
                    ItemsDataViewModel.ProductID = item.ProductID;
                    ItemsDataViewModel.PurchaseID = item.PurchaseID;
                    ItemsDataViewModel.QtyOnHand = item.QtyOnHand;
                    ItemsDataViewModel.QtyOrdered = item.QtyOrdered;
                    ItemsDataViewModel.QtyReceived = item.QtyReceived;
                    ItemsDataViewModel.UnitPrice = item.UnitPrice;
                    ItemsDataViewModel.SkuStatus = 1;
                    ItemsDataViewModel.ProductTitle = item.ProductName;
                    purchaseOrderItemsDataViewModel.Add(ItemsDataViewModel);
                }
                purchaseOrderDataViewModel.items = purchaseOrderItemsDataViewModel;

                _PODataAccess.UpdatePurchaseOrders(purchaseOrderDataViewModel);


            }
            catch (Exception ex)
            {
                throw;

            }
        }

        public PurchaseOrderViewModel.PurchaseOrderData GetPurchaseOrderByIdFromSellerCloud(string OrderID, AuthenticateSCRestViewModel sCRestViewModel )
        {
            PurchaseOrderViewModel.PurchaseOrderData responses = new PurchaseOrderViewModel.PurchaseOrderData();
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ApiURL + "/PurchaseOrders/" + OrderID);
                request.Method = "GET";
                request.Accept = "application/json;";
                request.ContentType = "application/json";
                request.Headers["Authorization"] = "Bearer " + sCRestViewModel.access_token;
                string strResponse = "";

                var response = (HttpWebResponse)request.GetResponse();

                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    strResponse = sr.ReadToEnd();
                }


                responses = JsonConvert.DeserializeObject<PurchaseOrderViewModel.PurchaseOrderData>(strResponse);

            }
            catch (WebException ex)
            {
                return responses;
            }
            return responses;
        }
    }
}
