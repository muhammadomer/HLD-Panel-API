using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class ZincOrderLogViewModel
    {
        public int ZincOrderLogID { get; set; }
        public string SellerCloudOrderId { get; set; }
        public string RequestIDOfZincOrder { get; set; }
        public DateTime OrderDatetime { get; set; }
    }
}
