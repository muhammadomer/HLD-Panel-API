using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityModel
{
    public class OrderLinesModel
    {
        public long Bbe2LineId { get; set; }
        public string Bbe2OrdersId { get; set; }
        public string OrderId { get; set; }
        public string OrderLineId { get; set; }
        public string OfferSku { get; set; }
        public string Quantity { get; set; }
        public string TotalPrice { get; set; }
        public string TotalCommission { get; set; }
        public string OrderLineState { get; set; }
        public string ReceivedDate { get; set; }
        public string ShippedDate { get; set; }
        public string ProductTitle { get; set; }
        public string TaxGst { get; set; }
        public string TaxPst { get; set; }
    }
}
