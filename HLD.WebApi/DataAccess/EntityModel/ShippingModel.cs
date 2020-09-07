using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityModel
{
  public  class ShippingModel
    {
        public long Bbe2ShippingId { get; set; }
        public long? Bbe2OrdersId { get; set; }
        public int? CommercialId { get; set; }
        public string ShippingCarrierCode { get; set; }
        public string ShippingCompany { get; set; }
        public long? ShippingPrice { get; set; }
        public string ShippingTracking { get; set; }
        public string ShippingTrackingUrl { get; set; }
        public string ShippingTypeCode { get; set; }
        public string ShippingTypeLabel { get; set; }
        public string ShippingZoneCode { get; set; }
        public string ShippingZoneLabel { get; set; }
    }
}
