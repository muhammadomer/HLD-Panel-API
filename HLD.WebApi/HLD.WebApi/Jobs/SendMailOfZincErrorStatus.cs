using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace HLD.WebApi.Jobs
{
    public class SendMailOfZincErrorStatus : IJob
    {
        public async Task   Execute(IJobExecutionContext context)
        {
           
            await Task.CompletedTask;
        }
    }
}

