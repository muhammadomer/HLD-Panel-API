using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class BestBuyOrderDetailViewModel
    {
        public string ProductSKU { get; set; }
        public string ProductTitle { get; set; }
        public string WarehouseQuantity { get; set; }
        public int TotalQuantity { get; set; }
        public string Location { get; set; }
        public string ZincASIN { get; set; }
        public string Prime { get; set; }
        public string ZincStatus { get; set; }
        public string ZincLink { get; set; }
        public string OrderStatus { get; set; }
        public String ImageUrl { get; set; }
        public decimal UnitPrice { get; set; }
        public string ProfitLoss { get; set; }
        public int OrderDetailID { get; set; }
        public decimal Comission { get; set; }
        public decimal TaxGST { get; set; }
        public decimal TaxPST { get; set; }
        public string SCOrderStatus { get; set; }
        public string AverageCost { get; set; }
        public string ZincCode { get; set; }
        public string ZincMessage { get; set; }
        public string ZincRequestID { get; set; }
        public string ZincOrderLogID { get; set; }
        public string ZincOrderLogDetailID { get; set; }
        public string ZincOrderStatusInternal { get; set; }
        public bool IsTrackingUpdateToSC { get; set; }
        public bool DropshipStatus { get; set; }
        public int DropshipQty { get; set; }
        public string BestBuyPorductID { get; set; }
        public string PaymentStatus { get; set; }
        public string ShadowOf { get; set; }
        public string LocationNotes { get; set; }
        //calculation properties
        public decimal ShippingFee { get; set; }
        public decimal calculation_TotalAmountOfUnitPrice { get; set; }
        public decimal calculation_TotalTax { get; set; }
        public decimal calculation_TotalTacPercentage { get; set; }
        public decimal calculation_Comission { get; set; }
        public decimal caculation_TotalAvgCost { get; set; }
        public decimal calculation_comissionPercentage { get; set; }
        public decimal calculation_ProfitLoss { get; set; }
        public decimal calculation_ProfitLossPercentage { get; set; }
        public decimal calculation_SumTotal { get; set; }
        public decimal ApprovedUnitPriceCAD { get; set; }

        public int ZincAccountId { get; set; }
        public int CreditCardId { get; set; }

        public List<ApprovedPriceViewModel> approvedPrices { get; set; }
        public List<ProductZinAsinDetail> ZincAsinDetail { get; set; }
        public List<ProductWarehouseQtyViewModel> ProductrWarehouseQtyViewModel { get; set; }
        public List<SkuTagOrderViewModel> skuTags { get; set; }
    }
}
