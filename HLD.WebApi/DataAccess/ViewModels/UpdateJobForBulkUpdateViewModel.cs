using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class UpdateJobIdForBulkUpdateViewModel
    {
        public string JobId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Sku { get; set; }
        public string S3FilePath { get; set; }
    }
}
