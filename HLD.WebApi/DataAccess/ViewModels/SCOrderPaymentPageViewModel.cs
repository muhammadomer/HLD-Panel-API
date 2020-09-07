using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class SCOrderPaymentPageViewModel
    {
        public decimal TotalCommission { get; set; }
        public string sku { get; set; }
        public int? Qty { get; set; }
        public decimal? TaxGST { get; set; }
        public decimal? TaxPST { get; set; }
        public decimal? AvgCost { get; set; }
        public int? TotalQty { get; set; }
        public decimal? OrderLineCommission { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? TotalAvgCost { get; set; }
        public decimal? TotalTax { get; set; }
        public decimal? ProfitAndLossInDollar { get; set; }
        public decimal? ProfitAndLossInPercentage { get; set; }
        public string SellerCloudID { get; set; }
    }
}
