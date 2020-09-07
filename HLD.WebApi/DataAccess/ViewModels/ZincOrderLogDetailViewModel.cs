using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class ZincOrderLogDetailViewModel
    {
        public int ZincOrderLogDetailID { get; set; }
        public int ZincOrderLogID { get; set; }
        public string Type { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }
        public string ShppingDate { get; set; }
        public string TrackingNumber { get; set; }
        public string Carrier { get; set; }
        public string AmazonTracking { get; set; }
        public string _17Tracking { get; set; }
        public DateTime OrderDatetime { get; set; }
        public string ZincOrderStatusInternal { get; set; }
        public string MerchantOrderId { get; set; }
        public string OurInternalOrderId { get; set; }
    }
}
