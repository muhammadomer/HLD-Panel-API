using DataAccess.Helper;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HLD.WebApi.Jobs
{
  public  interface IQuartzJob 
    {
        Task<bool> ImportDataFromSCandBBJob(CancellationToken cancellation);
    }
}
