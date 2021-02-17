using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class ZincWatclistViewModel
    {
        public string ASIN { get; set; }
        public string ProductSKU { get; set; }
        public int ValidStatus { get; set; }
        public string Compress_image { get; set; }
        public string image_name { get; set; }
        public string Active_Inactive { get; set; }
        public string Enabled_Disabled { get; set; }
        public string EmptyFirstTime { get; set; }
        public DateTime orderDateTimeFrom { get; set; }
        public DateTime orderDateTimeTo { get; set; }
        public DateTime NextUpdateDate { get; set; }
     
    }
}
