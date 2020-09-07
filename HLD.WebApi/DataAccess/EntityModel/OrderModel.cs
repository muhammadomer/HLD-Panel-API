using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityModel
{
   public class OrderModel
    {
        public long Bbe2OrdersId { get; set; }
        public string OrderId { get; set; }
        public int? CommercialId { get; set; }
        public string CustomerId { get; set; }
        public int? ShippingId { get; set; }
        public string InSellerCloud { get; set; }
        public string SellerCloudId { get; set; }
        public byte? CanCancel { get; set; }
        public string OrderState { get; set; }
        public decimal? TotalCommission { get; set; }
        public decimal? TotalPrice { get; set; }
        public DateTime? AcceptanceDecisionDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
