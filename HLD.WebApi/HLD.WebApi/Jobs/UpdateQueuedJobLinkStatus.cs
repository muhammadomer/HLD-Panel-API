using DataAccess.DataAccess;
using DataAccess.Helper;
using DataAccess.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quartz;
using ServiceReference1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;


namespace HLD.WebApi.Jobs
{
    [DisallowConcurrentExecution]
    public class UpdateQueuedJobLinkStatus : IJob
    {
        ProductDataAccess DataAccess;
        IConnectionString _connectionString = null;
        EncDecChannel _EncDecChannel = null;
        GetChannelCredViewModel _getChannelCredViewModel = null;
        public UpdateQueuedJobLinkStatus(IConnectionString connectionString)
        {
            DataAccess = new ProductDataAccess(connectionString);
            _connectionString = connectionString;
            _EncDecChannel = new EncDecChannel(_connectionString);
        }
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var DataForJobs = DataAccess.GetQuedfJobStatus();
                if (DataForJobs != null && DataForJobs.Count() > 0)
                {
                    _getChannelCredViewModel = new GetChannelCredViewModel();
                    _getChannelCredViewModel = _EncDecChannel.DecryptedData("sellercloud");

                    AuthenticateSCRestViewModel authenticate = new AuthenticateSCRestViewModel();
                    // Get Taken Fro SC
                    authenticate = _EncDecChannel.AuthenticateSCForIMportOrder(_getChannelCredViewModel, "https://lp.api.sellercloud.com/rest/api");
                    foreach (var item in DataForJobs)
                    {
                        var StausRES = "";
                        GetQuedJobStatusViewModel statusFromSc = new GetQuedJobStatusViewModel();
                        try
                        {
                            string[] getId = item.QuedJobLink.Split("=");
                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://lp.api.sellercloud.com/rest/api/QueuedJobs/" + getId[1]);
                            request.Method = "GET";
                            request.Accept = "application/json;";
                            request.ContentType = "application/json";
                            request.Headers["Authorization"] = "Bearer " + authenticate.access_token;
                            var response = (HttpWebResponse)request.GetResponse();
                            string strResponse = "";
                            using (var sr = new StreamReader(response.GetResponseStream()))
                            {
                                strResponse = sr.ReadToEnd();
                            }
                            var responseFromSC = JObject.Parse(strResponse);
                            StausRES = responseFromSC["Basic"]["Status"].ToString();
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }

                        int Id = item.QuedJobId;
                        var Status = StausRES;
                        if (Status == "0")
                        {
                            DataAccess.UpdateQuedJob(Id, "Submitted");
                        }
                        else if (Status == "1")
                        {
                            DataAccess.UpdateQuedJob(Id, "Processing");
                        }
                        else if (Status == "3")
                        {
                            DataAccess.UpdateQuedJob(Id, "Completed");
                        }
                        else if (Status == "4")
                        {
                            DataAccess.UpdateQuedJob(Id, "Failed");
                        }
                        else if (Status == "5")
                        {
                            DataAccess.UpdateQuedJob(Id, "PartialSuccess");
                        }
                        else if (Status == "6")
                        {
                            DataAccess.UpdateQuedJob(Id, "OnHold");
                        }
                        else if (Status == "7")
                        {
                            DataAccess.UpdateQuedJob(Id, "Cancelled");
                        }
                        else if (Status == "8")
                        {
                            DataAccess.UpdateQuedJob(Id, "Cancelled_Service_Restarted");
                        }
                        else if (Status == "9")
                        {
                            DataAccess.UpdateQuedJob(Id, "Aborted_Too_Much_Time_Consumed");
                        }
                        else if (Status == "10")
                        {
                            DataAccess.UpdateQuedJob(Id, "Cancelled_While_Running");
                        };

                    }
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
