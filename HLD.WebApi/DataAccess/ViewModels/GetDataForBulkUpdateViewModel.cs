using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class GetDataForBulkUpdateJobViewModel
    {
        public int JobId { get; set; }
        public string JobType { get; set; }
        public string File { get; set; }
        public int Status { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime CompletionTime { get; set; }
    }
}
