using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
  public  class ZincOrderIDModelforWebhooks
    {
        public string request_id { get; set; }
        public int zinc_order_log_id { get; set; }
    }
}
