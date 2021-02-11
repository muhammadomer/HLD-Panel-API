using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class ZincOrdersLogViewModel
    {
        public string SC_Order_ID { get; set; }
        public string BB_Order_ID { get; set; }
        public string Zinc_Order_ID { get; set; }
        public string Zinc_Status { get; set; }
      
        public DateTime order_datetime { get; set; }
        public string Amazon_AcName { get; set; }
        public string order_message { get; set; }
       
    }
}
