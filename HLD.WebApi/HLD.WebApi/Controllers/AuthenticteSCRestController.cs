using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using DataAccess.Helper;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace HLD.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticteSCRestController : ControllerBase
    {

        IConnectionString _connectionString = null;
      
       
        EncDecChannel _EncDecChannel = null;
        string ApiURL = null;
        GetChannelCredViewModel _getChannelCredViewModel = null;
        private readonly IConfiguration _configuration;
        public AuthenticteSCRestController(IConfiguration configuration,IConnectionString connectionString)
        {

            _connectionString = connectionString;
            this._configuration = configuration;
            ApiURL = _configuration.GetValue<string>("SCURL:URL");
            _EncDecChannel = new EncDecChannel(_connectionString);

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