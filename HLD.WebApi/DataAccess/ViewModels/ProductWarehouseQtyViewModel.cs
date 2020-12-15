using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class ProductWarehouseQtyViewModel
    {
        public int ProductWarehouseID { get; set; }
        public int WarehouseID { get; set; }
        public int AvailableQty { get; set; }
        public string ProductSku { get; set; }
        public string WarehouseName { get; set; }
        public string LocationNotes { get; set; }
        public decimal OnOrder { get; set; }
        public List<ApprovedPriceViewModel> approvedPrices { get; set; }
    }
}
