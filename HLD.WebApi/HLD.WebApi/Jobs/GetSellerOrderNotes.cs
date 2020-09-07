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
    public class GetSellerOrderNotes : IJob
    {
        IConnectionString _connectionString = null;
        EncDecChannel _EncDecChannel = null;
        string ApiURL = null;
        SellerCloudOrderDataAccess _sellerCloudOrderDataAccess = null;
        OrderNotesDataAccess orderNotes = null;
        GetChannelCredViewModel _getChannelCredViewModel = null;
        public GetSellerOrderNotes(IConnectionString connectionString)
        {
            _connectionString = connectionString;
            _sellerCloudOrderDataAccess = new SellerCloudOrderDataAccess(_connectionString);
            orderNotes = new OrderNotesDataAccess(_connectionString);
            ApiURL = "https://lp.api.sellercloud.com/rest/api";
            _EncDecChannel = new EncDecChannel(_connectionString);

        }
        public async Task Execute(IJobExecutionContext context)
        {
            List<int> orderList = _sellerCloudOrderDataAccess.GetSellerCloudOrderForUpdateOrderStatus();
            _getChannelCredViewModel = new GetChannelCredViewModel();
            _getChannelCredViewModel = _EncDecChannel.DecryptedData("sellercloud");
            AuthenticateSCRestViewModel authenticate = new AuthenticateSCRestViewModel();
            // Get Taken Fro SC
            authenticate = _EncDecChannel.AuthenticateSCForIMportOrder(_getChannelCredViewModel, ApiURL);
            foreach (var item in orderList)
            {

                GetOrderNotes(item, authenticate).Wait();

            }

            await Task.CompletedTask;
        }

        private async Task GetOrderNotes(int item, AuthenticateSCRestViewModel authenticate)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ApiURL + "/Orders/Notes?id=" + item);
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
                List<CreateOrderNotesViewModel> responses = JsonConvert.DeserializeObject<List<CreateOrderNotesViewModel>>(strResponse);
                if (responses.Count > 0)
                {
                    List<CreateOrderNotesViewModel> data = (List<CreateOrderNotesViewModel>)responses.Select(p=>p).Where(p=>p.NoteID!=0).ToList();
                    if (data.Count > 0)
                    {
                        bool status = orderNotes.SaveOrderNotes(data);
                        if (status == true)
                        {
                            orderNotes.UpdateOrderAsHavingNotes(item);
                        }
                    }
                   

                }


            }
            catch (Exception ex)
            {

            }
        


        }
    }
}
