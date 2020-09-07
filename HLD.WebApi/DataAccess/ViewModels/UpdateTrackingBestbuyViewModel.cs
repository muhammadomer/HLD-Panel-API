using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class UpdateTrackingBestbuyViewModel
    {
        public string scOrderID { get; set; }
        public string bbOrderID { get; set; }
        public string trackingNumber { get; set; }
        public string shippingServiceCode { get; set; }
        public string shippingMethodName { get; set; }
        public string shippingCost { get; set; }
        public string declaredValue { get; set; }
        public DateTime? shipDate { get; set; }
        public DateTime? estimatedDeliveryDate { get; set; }
        public string packageWeight { get; set; }
        public string packageID { get; set; }
        public int inBestbuy { get; set; }
        public int bbe2TrackingId { get; set; }
    }
}
