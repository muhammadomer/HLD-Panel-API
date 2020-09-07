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
using System.Threading.Tasks;

namespace HLD.WebApi.Jobs
{
    [DisallowConcurrentExecution]
    public class UpdateOrdersFromBestBuyJob : IJob
    {

        IConnectionString _connectionString = null;


        BestBuyOrderFromBBDataAccess _bestBuytDataAccess = null;
        EncDecChannel _EncDecChannel = null;
        GetChannelCredViewModel _getChannelCredViewModel = null;
        ChannelDecrytionDataAccess channelDecrytionDataAccess = null;
        public UpdateOrdersFromBestBuyJob(IConnectionString connectionString)
        {
            _connectionString = connectionString;

            _EncDecChannel = new EncDecChannel(_connectionString);
            _bestBuytDataAccess = new BestBuyOrderFromBBDataAccess(_connectionString);
            _EncDecChannel = new EncDecChannel(_connectionString);
            channelDecrytionDataAccess = new ChannelDecrytionDataAccess(_connectionString);
        }
        public async Task Execute(IJobExecutionContext context)
        {
            int status = channelDecrytionDataAccess.CheckZincJobsStatus("neworders");
            if (status == 1)
            {
                _getChannelCredViewModel = new GetChannelCredViewModel();
                _getChannelCredViewModel = _EncDecChannel.DecryptedData("bestbuy");
                GetOrdersFromBestBuyViewModel.BestBuyRootObjectBB bestBuyRootObject = new GetOrdersFromBestBuyViewModel.BestBuyRootObjectBB();
              List<string> orderList = _bestBuytDataAccess.GetBestBuyOrderIdsToUpdate();
                if (orderList.Count > 0)
                {
                    bestBuyRootObject = GetBestBuyOrdersByOrderID(_getChannelCredViewModel.Key, orderList);

                    foreach (var result in bestBuyRootObject.orders)
                    {

                        _bestBuytDataAccess.UpdateBestBuyOrderINOrder(result);
                        _bestBuytDataAccess.UpdateBestBuyOrderINOrderLines(result);

                        _bestBuytDataAccess.UpdateBestBuyOrderINCustomerShipping(result);

                    }
                }
                

              
            }
        }

        public GetOrdersFromBestBuyViewModel.BestBuyRootObjectBB GetBestBuyOrdersByOrderID(string token,List<string> orderList)
        {
            GetOrdersFromBestBuyViewModel.BestBuyRootObjectBB responses = new GetOrdersFromBestBuyViewModel.BestBuyRootObjectBB();
            try
            {
                string orderIdsTobeSend = String.Join(",", orderList);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://marketplace.bestbuy.ca/api/orders?paginate=false&order_ids="+ orderIdsTobeSend);
                request.Method = "GET";
                request.Accept = "application/json;";
                request.ContentType = "application/json";
                request.Headers["Authorization"] = token;

                string strResponse = "";
                using (WebResponse webResponse = request.GetResponse())
                {
                    using (StreamReader stream = new StreamReader(webResponse.GetResponseStream()))
                    {
                        strResponse = stream.ReadToEnd();
                    }
                }

                responses = JsonConvert.DeserializeObject<GetOrdersFromBestBuyViewModel.BestBuyRootObjectBB>(strResponse);
            }
            catch (Exception ex)
            {

                throw;
            }
            return responses;
        }
    }
}
