using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class GetAddressViewModel
    {
        public string WH_Name { get; set; }
    }
    public class GetSendToZincOrderViewModel
    {
        public string Asin { get; set; }
        public string Sku { get; set; }
        public string AccountDetail { get; set; }
        public DateTime Date { get; set; }
        public string TrackingNumber { get; set; }
        public string Response { get; set; }
        public DateTime LastUpdate { get; set; }
        public int Qty { get; set; }
    }
}
