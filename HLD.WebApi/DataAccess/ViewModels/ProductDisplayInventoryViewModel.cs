using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class ProductDisplayInventoryViewModel
    {

        public int ProductId { get; set; }
        public string ProductSKU { get; set; }
        public string ProductTitle { get; set; }

        public string ColorName { get; set; }

        public string AvgCost { get; set; }
        public string ShipmentWeight { get; set; }
        public string ShipmentLWH { get; set; }
        public byte[] Image { get; set; }

        public decimal BBMSRP { get; set; }
        public decimal BBSellingPrice { get; set; }
        public string BestBuyProductSKU { get; set; }
        public string ImageURL { get; set; }
        public bool dropship_status { get; set; }
        public int dropship_Qty { get; set; }
        public int AggregatedQty { get; set; }
        public bool Continue { get; set; }
        public int best_buy_product_id { get; set; }
        public int BBProductID { get; set; }
        public decimal ApprovedUnitPrice { get; set; }

        public List<ProductWarehouseQtyViewModel> ProductrWarehouseQtyViewModel { get; set; }
        public List<SkuTagOrderViewModel> skuTags { get; set; }


    }
}
