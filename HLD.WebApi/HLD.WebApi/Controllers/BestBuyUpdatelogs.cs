using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.DataAccess;
using DataAccess.Helper;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HLD.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BestBuyUpdatelogs : ControllerBase
    {


        BestBuyUpdateLogsDataAccess dataAccess;
        IConnectionString _connectionString = null;
        public BestBuyUpdatelogs(IConnectionString connectionString)
        {
            _connectionString = connectionString;
            dataAccess = new BestBuyUpdateLogsDataAccess(connectionString);
        }

        [HttpGet]
        [Route("Getlogs")]
        public List<BestBuyUpdateLogsViewModel> GetWatchlistLogs(int JobId, int limit, int offset)// get Logs 
        {
            List<BestBuyUpdateLogsViewModel> status = new List<BestBuyUpdateLogsViewModel>();
            status = dataAccess.GetWatchlistLogs(JobId,limit,offset);
            return status;
        }

        [HttpGet]
        [Route("GetlogsCount")]
        public int GetWatchlistLogsCount(int JobId)// get Logs 
        {
            int status = 0;
            status = dataAccess.GetWatchlistLogsCount(JobId);
            return status;
        }
    }

}
