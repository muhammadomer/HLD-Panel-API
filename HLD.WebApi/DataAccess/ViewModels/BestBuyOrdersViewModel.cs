using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class BestBuyOrdersViewModel
    {
        public DateTime? OrderDate { get; set; }
        public String SellerCloudOrderID { get; set; }
        public string OrderNumber { get; set; }
        public string ParentOrderID { get; set; }
        public string IsParent { get; set; }
        public string IsNotes { get; set; }
        public string ShippingPrice { get; set; }
        public string ShipmentAddress { get; set; }
        public int TotalQuantity { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TotalComission { get; set; }
        public decimal ProfitAndLossInDollar { get; set; }
        public decimal ProfitAndLossInPercentage { get; set; }
        public decimal TotalAverageCost { get; set; }

        public int ZincAccountId { get; set; }
        public int CreditCardId { get; set; }

        public List<BestBuyOrderDetailViewModel> BBProductDetail { get; set; }


    }
    public class BestBuyOrdersViewPageModel
    {
        public DateTime? OrderDate { get; set; }
        public string SellerCloudOrderID { get; set; }
        public string OrderNumber { get; set; }
        public string CustomerName { get; set; }
        public string ParentOrderID { get; set; }
        public string IsParent { get; set; }
        public string IsNotes { get; set; }
        public string ShippingPrice { get; set; }
        public string Street { get; set; }
        public string State { get; set; }
        public string tracking_number { get; set; }
        public string Country { get; set; }
        public int TotalQuantity { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TotalComission { get; set; }
        public decimal ProfitAndLossInDollar { get; set; }
        public decimal ProfitAndLossInPercentage { get; set; }
        public decimal TotalAverageCost { get; set; }
        public string PaymentStatus { get; set; }
        public int ZincAccountId { get; set; }
        public int CreditCardId { get; set; }
        public List<BestBuyOrderDetailViewModel> BBProductDetail { get; set; }
    }
}
