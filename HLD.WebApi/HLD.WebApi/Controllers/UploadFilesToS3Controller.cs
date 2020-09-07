using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.DataAccess;
using DataAccess.Helper;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HLD.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadFilesToS3Controller : ControllerBase
    {
        UploadFilesToS3DataAccess dataAccess;
        public UploadFilesToS3Controller(IConnectionString connectionString)
        {
            dataAccess = new UploadFilesToS3DataAccess(connectionString);
        }

        [HttpPost]
        [Authorize]
       
        public JobIdReturnViewModel PostJobDetail(UploadFilesToS3ViewModel viewModel)
        {
            JobIdReturnViewModel jobIdReturn = new JobIdReturnViewModel();
            jobIdReturn = dataAccess.SaveFileUploadJobDetail(viewModel);
            return jobIdReturn;


        }

        [HttpGet]
        [Authorize]
        public List<GetJobDetailViewModel> GetJobDetail()
        {
            List<GetJobDetailViewModel> getJobs = new List<GetJobDetailViewModel>();
            getJobs = dataAccess.GetJobsOfS3();
            return getJobs;
          
           
        }

        [HttpGet]
        [Authorize]
        [Route("{id}")]
        public S3LogViewModel GetJobLogs(int id)
        {
            S3LogViewModel getJobs = new S3LogViewModel();
            getJobs = dataAccess.GetS3JobLogsDetail(id);
            return getJobs;


        }
        



    }
}