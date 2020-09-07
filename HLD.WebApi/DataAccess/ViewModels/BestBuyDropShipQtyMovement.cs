using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class BestBuyDropShipQtyMovement
    {
        public string ProductSku { get; set; }
        public string DropShipStatus { get; set; }
        public int DropShipQuantity { get; set; }
        public string OrderQuantity { get; set; }
        public DateTime OrderDate { get; set; }
        public string IsDropshipStatusUpdate_Name { get; set; }
        public int IsDropshipStatusUpdate_id { get; set; }
    }
}
