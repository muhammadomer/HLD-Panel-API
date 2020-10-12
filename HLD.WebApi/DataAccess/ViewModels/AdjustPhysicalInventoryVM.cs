using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
  public class AdjustPhysicalInventoryVM
    {
        public int WarehouseID { get; set; }
        public string ProductID { get; set; }
        public int Qty { get; set; }
        public int AdjustmentType { get; set; }
        public string Reason { get; set; }
        public decimal InventoryCost { get; set; }
        public int SiteCost { get; set; }
        public string PinCode { get; set; }
    }
}
