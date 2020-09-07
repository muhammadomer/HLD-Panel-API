using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class ProductWarehouseQtyForDS_None_SKU_ViewModel
    {
        public int BB_ds_none_warehouse_qty_id { get; set; }
        public string product_sku { get; set; }
        public int warehouse_qty { get; set; }
        public int warehouse_id { get; set; }
        public int warehouse_qty_difference { get; set; }
        public DateTime insert_datetime { get; set; }
        public bool update_status { get; set; }
        public DateTime update_datetime { get; set; }
    }
}
