using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class UpdateTrackingShippingJsonViewModel
    {
        public string carrier_code { get; set; }
        public string carrier_name { get; set; }
        public string carrier_url { get; set; }
        public string tracking_number { get; set; }
    }
}
