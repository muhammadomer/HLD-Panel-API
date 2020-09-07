using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class WareHouseProductQuantitylistViewModel
    {
        public string warehouseName { get; set; }
        public string sku { get; set; }
        public int AvailableQty { get; set; }
        public int physical_qty { get; set; }
        public int warehouseId { get; set; }
    }
}
