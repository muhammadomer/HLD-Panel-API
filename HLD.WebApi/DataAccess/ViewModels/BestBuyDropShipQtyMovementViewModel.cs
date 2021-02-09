using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class BestBuyDropShipQtyMovementViewModel
    {
        public int DropShipQtyMovementID { get; set; }
        public string ProductSku { get; set; }
        public bool DropShipStatus { get; set; }
        public int DropShipQuantity { get; set; }
        public string OrderQuantity { get; set; }
        public DateTime OrderDate { get; set; }
        public string IsDropshipStatusUpdate_Name { get; set; }
        public int IsDropshipStatusUpdate_id { get; set; }
        public int BestBuyImportID { get; set; }
        public string DropShipStatusString { get; set; } //fecting this from database
        public string comments { get; set; }
        public string BBProductID { get; set; }
        public string DropshipComments { get; set; }
    }
    public class BestBuyQTYLogsDetailViewModel
    {
        public string product_sku { get; set; }
        //   public bool ds_status { get; set; }
        public int ds_qty { get; set; }
        public string OrderQuantity { get; set; }
        public DateTime order_date { get; set; }
        public DateTime UpdatedOnBB { get; set; }
        public string update_status { get; set; }
        public int IsDropshipStatusUpdate_id { get; set; }
        public int bb_import_id { get; set; }
        public string ds_status { get; set; }
        public string comments { get; set; }
        public string BBProductID { get; set; }
    }
}
