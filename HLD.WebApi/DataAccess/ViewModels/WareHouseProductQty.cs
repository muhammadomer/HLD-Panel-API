using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class WareHouseProductQty
    {
        
            public string ProductID { get; set; }
            public int AvailableQty { get; set; }
            public int PhysicalQty { get; set; }
            public int WarehouseID { get; set; }
        
    }
}
