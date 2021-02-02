using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class GetTemZincDataViewModel
    {
        public int zinc_order_log_id { get; set; }
        public int zinc_order_log_detail_id { get; set; }
        public string order_type { get; set; }
        public string request_id { get; set; }
        public string sc_order_id { get; set; }
        public string zinc_order_status_internal { get; set; }
        public string order_code { get; set; }
        public string order_message { get; set; }
    }
}
