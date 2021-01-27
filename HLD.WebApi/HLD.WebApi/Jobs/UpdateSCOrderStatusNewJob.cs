using DataAccess.DataAccess;
using DataAccess.Helper;
using DataAccess.ViewModels;
using HLD.WebApi.Enum;
using Quartz;
using ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HLD.WebApi.Jobs
{
    public class UpdateSCOrderStatusNewJob
    {
        IConnectionString _connectionString = null;
        ServiceReference1.AuthHeader authHeader = null;
        SellerCloudOrderDataAccessNew _sellerCloudOrderDataAccess = null;
        BestBuyOrderDataAccess _bestBuyOrderDataAccess = null;
        EncDecChannel _EncDecChannel = null;
        GetChannelCredViewModel _getChannelCredViewModel = null;
        public UpdateSCOrderStatusNewJob(IConnectionString connectionString)
        {
            _connectionString = connectionString;
            _EncDecChannel = new EncDecChannel(_connectionString);
            _sellerCloudOrderDataAccess = new SellerCloudOrderDataAccessNew(_connectionString);
            _bestBuyOrderDataAccess = new BestBuyOrderDataAccess(_connectionString);
        }
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                _getChannelCredViewModel = new GetChannelCredViewModel();
                _getChannelCredViewModel = _EncDecChannel.DecryptedData("sellercloud");
                authHeader = new AuthHeader();
                authHeader.ValidateDeviceID = false;
                authHeader.UserName = _getChannelCredViewModel.UserName;
                authHeader.Password = _getChannelCredViewModel.Key;

                // send request to sc for orders 
                ServiceReference1.SCServiceSoapClient sCServiceSoap =
                          new ServiceReference1.SCServiceSoapClient(ServiceReference1.SCServiceSoapClient.EndpointConfiguration.SCServiceSoap12);
                // select which are not completed and cancelled from sellecloud order details , seller cloud status category
                List<int> orderList = _sellerCloudOrderDataAccess.GetSellerCloudOrderForUpdateOrderStatus();

                foreach (var item in orderList)
                {
                    var data = await sCServiceSoap.Orders_GetOrderStateAsync(authHeader, null, Convert.ToInt32(item));
                    ServiceReference1.OrderStatusCode orderStatusCode = data.Orders_GetOrderStateResult.StatusCode;
                    ServiceReference1.DropShipStatusType dropshipStatus = data.Orders_GetOrderStateResult.DropShipStatus;
                    ServiceReference1.OrderPaymentStatus2 paymentStatus = data.Orders_GetOrderStateResult.PaymentStatus;
                    //if (orderStatusCode.ToString() != "InProcess")
                    //{
                    int status = 0;
                    if (orderStatusCode.ToString() == "Canceled")
                    {
                        status = (int)SellerCloudOrderStatusCategory.Canceled;
                    }
                    else if (orderStatusCode.ToString() == "ShoppingCart")
                    {
                        status = (int)SellerCloudOrderStatusCategory.ShoppingCart;
                    }
                    else if (orderStatusCode.ToString() == "InProcess")
                    {
                        status = (int)SellerCloudOrderStatusCategory.InProcess;
                    }
                    else if (orderStatusCode.ToString() == "ProblemOrder")
                    {
                        status = (int)SellerCloudOrderStatusCategory.ProblemOrder;
                    }
                    else if (orderStatusCode.ToString() == "OnHold")
                    {
                        status = (int)SellerCloudOrderStatusCategory.OnHold;
                    }
                    else if (orderStatusCode.ToString() == "Quote")
                    {
                        status = (int)SellerCloudOrderStatusCategory.Quote;
                    }
                    else if (orderStatusCode.ToString() == "Void")
                    {
                        status = (int)SellerCloudOrderStatusCategory.Void;
                    }
                    else if (orderStatusCode.ToString() == "InProcess or Completed")
                    {
                        status = (int)SellerCloudOrderStatusCategory.InProcess_or_Completed;
                    }
                    else if (orderStatusCode.ToString() == "InProcess or Hold")
                    {
                        status = (int)SellerCloudOrderStatusCategory.InProcess_or_Hold;
                    }
                    else if (orderStatusCode.ToString() == "Completed")
                    {
                        status = (int)SellerCloudOrderStatusCategory.Completed;
                    }
                    _sellerCloudOrderDataAccess.SaveSellerCloudOrderStatus(item.ToString(), status.ToString(), paymentStatus.ToString());

                    //  }
                    //else
                    //{
                    //    _sellerCloudOrderDataAccess.UpdatePaymentOrderStatus(paymentStatus.ToString(), item.ToString());
                    //}
                    // update dropship status
                    _sellerCloudOrderDataAccess.UpdateSCOrderDropShipStatus(new UpdateSCDropshipStatusViewModel()
                    {
                        IsTrackingUpdate = false,
                        LogDate = DateTimeExtensions.ConvertToEST(DateTime.Now),
                        SCOrderID = Convert.ToInt32(item),
                        StatusName = dropshipStatus.ToString()
                    });

                }
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
