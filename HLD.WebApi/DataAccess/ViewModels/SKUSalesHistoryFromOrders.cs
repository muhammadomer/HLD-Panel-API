using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class SKUSalesHistoryFromOrders
    {
        public string SellerCloudID { get; set; }
        public string MPID { get; set; } // market palce ID
        public DateTime? InSellerCloud { get; set; }
        public string OfferSku { get; set; }
        public int TotalQuantity { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalComission { get; set; }
        public decimal TaxGST { get; set; }
        public decimal TaxPst { get; set; }        
        public decimal UnitPrice { get; set; }
        public decimal AverageCost { get; set; }
        public int RowID { get; set; }
        public decimal ProductAvgCost { get; set; }


        // calculations
        public decimal calculation_TotalAmountOfUnitPrice { get; set; }
        public decimal calculation_TotalAvgCost { get; set; }
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
