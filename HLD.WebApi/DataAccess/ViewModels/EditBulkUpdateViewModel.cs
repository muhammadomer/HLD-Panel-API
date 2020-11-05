using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class EditBulkUpdateViewModel
    {
        public string AmazonMerchantSKU { get; set; }
        public string AmazonEnabled { get; set; }
        public string ASIN { get; set; }
        public string FulfilledBy { get; set; }
        public string AmazonFBASKU { get; set; }
        public string WebsiteEnabled { get; set; }
        public string ShadowSku { get; set; }
    }
}
