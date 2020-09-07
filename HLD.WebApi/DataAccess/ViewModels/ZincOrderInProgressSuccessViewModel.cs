using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class ZincOrderInProgressSuccessViewModel
    {
        public string SCOrderId { get; set; }
        public string Qty { get; set; }
        public string RequestID { get; set; }
        public string ProductSKU { get; set; }
        public string ZincOrderLogID { get; set; }
        public string ZincOrderLogDetailID { get; set; }
    }
}
