using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class Order_SKU_ProfitHistory_CalculationViewmodel
    {
        public decimal TaxesPercentage { get; set; }
        public decimal Taxes { get; set; }
        public decimal Percentage { get; set; }
        public decimal SellingFeePercentage { get; set; }
        public decimal SellingFee { get; set; }
        public decimal GrossRevnue { get; set; }
        public decimal ProfitPercentage { get; set; }
        public decimal Profit { get; set; }
        public int UnitsSold { get; set; }
        public int OrderQuantity { get; set; }
        public decimal ItemCost { get; set; }
        public string SKU { get; set; }
        public string Duration { get; set; }
        public DateTime? DateTime { get; set; }
        public DateTime? StartDate { get; set; }
    }
}
