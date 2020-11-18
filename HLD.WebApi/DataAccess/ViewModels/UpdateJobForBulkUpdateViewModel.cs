using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class UpdateJobIdForBulkUpdateViewModel
    {
        public string QueuedJobLink { get; set; }
        public int ID { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Sku { get; set; }
        public string S3FileDirectoryPath { get; set; }
        public string FileNames { get; set; }
        public string JobType { get; set; }
        public string Status { get; set; }
    }
}
