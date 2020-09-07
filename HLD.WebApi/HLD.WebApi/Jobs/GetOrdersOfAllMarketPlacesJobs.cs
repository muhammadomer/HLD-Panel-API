
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
    public class GetOrdersOfAllMarketPlacesJobs : IJob
    {
        ProductSalesDataAccess productSalesData = null;
        IConnectionString _connectionString = null;


        AddOrderToSCDataAccess _addOrderToSCDataAccess = null;
        private readonly IConfiguration _configuration;
        private readonly ILogger logger;
        GetChannelCredViewModel _getChannelCredViewModel = null;
        ChannelDecrytionDataAccess channelDecrytionDataAccess = null;
        EncDecChannel _EncDecChannel = null;
        string ApiURL = null;
        ServiceReference1.AuthHeader authHeader = null;
        public GetOrdersOfAllMarketPlacesJobs(IConnectionString connectionString, ILogger<CreateOrderInSellerCloud> _logger, IConfiguration configuration)
        {

            _connectionString = connectionString;

            this._configuration = configuration;
            ApiURL = _configuration.GetValue<string>("SCURL:URL");
            _EncDecChannel = new EncDecChannel(_connectionString);
            productSalesData = new ProductSalesDataAccess(_connectionString);
            channelDecrytionDataAccess = new ChannelDecrytionDataAccess(_connectionString);
            this.logger = _logger;

        }

        public async Task Execute(IJobExecutionContext context)

        {

            JobMainMethod();

            await Task.CompletedTask;

        }



        private void JobMainMethod()
        {
            try
            {


                AuthenticateSCRestViewModel authenticateSCRestViewModel;
                List<SellerProductDataViewModel> sellerProductDataViewList = new List<SellerProductDataViewModel>();
                List<SellerProductDataViewModel> UpdatesellerProductDataViewList = new List<SellerProductDataViewModel>();
                authenticateSCRestViewModel = AuthenticateSC();
                SellerProductAllOrdersViewModel allOrdersViewModel = GetOrderFromSellerCloudAllMarketPlaces(authenticateSCRestViewModel);

                string AllOrderCommaSeprate = string.Join(",", allOrdersViewModel.Items.Select(p => p.ID));

                List<int> AllreadyExistOrder = productSalesData.GetSellerCloudOrderWhichAreExistsPredict(AllOrderCommaSeprate);
                var NewOrders = allOrdersViewModel.Items.Select(e => e.ID).Except(AllreadyExistOrder);
                foreach (var item in NewOrders)
                {
                    var result = allOrdersViewModel.Items.Where(e => e.ID == item).FirstOrDefault();

                    foreach (var ProductItem in result.Items)
                    {
                        SellerProductDataViewModel sellerProductDataView = new SellerProductDataViewModel();
                        sellerProductDataView.OrderID = ProductItem.OrderID;
                        sellerProductDataView.OrderSourceOrderID = result.OrderSourceOrderID;
                        sellerProductDataView.AdjustedSitePrice = ProductItem.AdjustedSitePrice;
                        sellerProductDataView.AverageCost = ProductItem.AverageCost;
                        sellerProductDataView.CompanyID = result.CompanyID;
                        sellerProductDataView.CompanyName = result.CompanyName;
                        sellerProductDataView.DestinationCountry = result.DestinationCountry;
                        sellerProductDataView.DisplayName = ProductItem.DisplayName;
                        sellerProductDataView.FinalShippingFee = result.FinalShippingFee;
                        sellerProductDataView.FinalValueFee = ProductItem.FinalValueFee;
                        sellerProductDataView.GrandTotal = result.GrandTotal;
                        sellerProductDataView.OrderCurrencyCode = result.OrderCurrencyCode;
                        sellerProductDataView.OrderSource = result.OrderSource;
                        sellerProductDataView.PaymentStatus = result.PaymentStatus;
                        sellerProductDataView.ProductID = ProductItem.ProductID;
                        sellerProductDataView.Qty = ProductItem.Qty;
                        sellerProductDataView.ShippingStatus = result.ShippingStatus;
                        sellerProductDataView.StatusCode = result.StatusCode;
                        sellerProductDataView.TimeOfOrder = result.TimeOfOrder;

                        sellerProductDataViewList.Add(sellerProductDataView);
                    }


                }

                if (sellerProductDataViewList.Count > 0)
                {
                    productSalesData.SaveAllMarketPlacesOrdersFromSC(sellerProductDataViewList);
                }


                //foreach (var item in AllreadyExistOrder)
                //{
                //    var result = allOrdersViewModel.Items.Where(e => e.ID == item).FirstOrDefault();

                //    foreach (var ProductItem in result.Items)
                //    {
                //        SellerProductDataViewModel sellerProductDataView = new SellerProductDataViewModel();
                //        sellerProductDataView.OrderID = ProductItem.OrderID;
                //        sellerProductDataView.OrderSourceOrderID = result.OrderSourceOrderID;
                //        sellerProductDataView.AdjustedSitePrice = ProductItem.AdjustedSitePrice;
                //        sellerProductDataView.AverageCost = ProductItem.AverageCost;
                //        sellerProductDataView.CompanyID = result.CompanyID;
                //        sellerProductDataView.CompanyName = result.CompanyName;
                //        sellerProductDataView.DestinationCountry = result.DestinationCountry;
                //        sellerProductDataView.DisplayName = ProductItem.DisplayName;
                //        sellerProductDataView.FinalShippingFee = result.FinalShippingFee;
                //        sellerProductDataView.FinalValueFee = ProductItem.FinalValueFee;
                //        sellerProductDataView.GrandTotal = result.GrandTotal;
                //        sellerProductDataView.OrderCurrencyCode = result.OrderCurrencyCode;
                //        sellerProductDataView.OrderSource = result.OrderSource;
                //        sellerProductDataView.PaymentStatus = result.PaymentStatus;
                //        sellerProductDataView.ProductID = ProductItem.ProductID;
                //        sellerProductDataView.Qty = ProductItem.Qty;
                //        sellerProductDataView.ShippingStatus = result.ShippingStatus;
                //        sellerProductDataView.StatusCode = result.StatusCode;
                //        sellerProductDataView.TimeOfOrder = result.TimeOfOrder;

                //        UpdatesellerProductDataViewList.Add(sellerProductDataView);
                //    }


                //}
                //if (UpdatesellerProductDataViewList.Count > 0)
                //{
                //    productSalesData.UpdateAllMarketPlacesOrdersFromSC(UpdatesellerProductDataViewList);
                //}

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        private SellerProductAllOrdersViewModel GetOrderFromSellerCloudAllMarketPlaces(AuthenticateSCRestViewModel authenticateSCRest)
        {
            SellerProductAllOrdersViewModel ordersViewModel = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ApiURL + "/Orders?model.dateRange=Last3Days&model.paymentStatus=Charged&model.pageNumber=1&model.pageSize=1000");
                request.Method = "GET";
                request.Accept = "application/json;";
                request.ContentType = "application/json";
                request.Headers["Authorization"] = "Bearer " + authenticateSCRest.access_token;

                string strResponse = "";
                using (WebResponse webResponse = request.GetResponse())
                {
                    using (StreamReader stream = new StreamReader(webResponse.GetResponseStream()))
                    {
                        strResponse = stream.ReadToEnd();
                    }
                }

                ordersViewModel = JsonConvert.DeserializeObject<SellerProductAllOrdersViewModel>(strResponse);
            }
            catch (WebException ex)
            {

            }
            return ordersViewModel;
        }
        public AuthenticateSCRestViewModel AuthenticateSC()
        {
            AuthenticateSCRestViewModel responses = new AuthenticateSCRestViewModel();
            try
            {
                _getChannelCredViewModel = new GetChannelCredViewModel();
                _getChannelCredViewModel = _EncDecChannel.DecryptedData("sellercloud");

                RestSCCredViewModel Data = new RestSCCredViewModel();
                Data.Username = _getChannelCredViewModel.UserName;
                Data.Password = _getChannelCredViewModel.Key;

                var data = JsonConvert.SerializeObject(Data);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ApiURL + "/token");
                request.Method = "POST";
                request.Accept = "application/json;";
                request.ContentType = "application/json";
                request.ContentLength = data.Length;
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(data);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                var response = (HttpWebResponse)request.GetResponse();
                string strResponse = "";
                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    strResponse = sr.ReadToEnd();
                }

                responses = JsonConvert.DeserializeObject<AuthenticateSCRestViewModel>(strResponse);

            }
            catch (WebException ex)
            {
                return responses;
            }
            return responses;
        }
    }
}
