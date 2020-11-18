using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class GetDataForBulkUpdateJobViewModel
    {
         public int BulkUpdateId { get; set; }
        public string ProductSku { get; set; }
        public string FileDirectory { get; set; }
        public string FileName { get; set; }
        public DateTime CreationDate { get; set; }
        public string JobType { get; set; }
        public string Status { get; set; }
        public string QueuedJobLink { get; set; }
        public int QueuedJobLinkId { get; set; }
    }
}
