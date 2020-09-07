using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class ProductDisplayViewModel
    {
        public int ProductId { get; set; }
        public string ProductSKU { get; set; }
        public string ProductTitle { get; set; }
        public string ConditionName { get; set; }
        public string ColorName { get; set; }
        public string BrandName { get; set; }
        public string Upc { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public string AvgCost { get; set; }
        public string ShipmentWeight { get; set; }
        public string ShipmentLWH { get; set; }
        public byte[] Image { get; set; }
        public DateTime? DiscountStartDate { get; set; }
        public DateTime? DiscountEndDate { get; set; }
        public decimal BBMSRP { get; set; }
        public decimal BBSellingPrice { get; set; }
        public string BestBuyProductSKU { get; set; }
        public string ImageURL { get; set; }
        public bool dropship_status { get; set; }
        public int dropship_Qty { get; set; }


        public List<ProductWarehouseQtyViewModel> ProductrWarehouseQtyViewModel { get; set; }
    }
}
