using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class Order_SKU_Profit_History
    {
        public string SellerCloudID { get; set; }
        public string BBOrderID { get; set; } 
        public DateTime? InSellerCloud { get; set; }
        public string OfferSku { get; set; }
        public int TotalQuantity { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalComission { get; set; }
        public decimal TaxGST { get; set; }
        public decimal TaxPst { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal AverageCost_CAD { get; set; }
        public decimal Exchange { get; set; }
        public string Currency_CA_USA { get; set; }
        public DateTime UpdatedDate { get; set; }
        public decimal ProductAvgCost_USD { get; set; }
        public string MarketPlace { get; set; } = "BestBuy";
        public decimal ShippingCost { get; set; }
        public decimal SellingFee { get; set; }
         

        // calculations
        public decimal calculation_TotalAmountOfUnitPrice { get; set; }
        public decimal calculation_TotalTax { get; set; }
        public decimal calculation_TotalTacPercentage { get; set; }
        public decimal calculation_Comission { get; set; }
        public decimal caculation_TotalAvgCost { get; set; }
        public decimal calculation_comissionPercentage { get; set; }
        public decimal calculation_ProfitLoss { get; set; }
        public decimal calculation_ProfitLossPercentage { get; set; }
        public decimal calculation_SumTotal { get; set; }
    }
}
