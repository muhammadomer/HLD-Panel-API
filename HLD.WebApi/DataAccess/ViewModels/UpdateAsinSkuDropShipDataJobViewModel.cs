using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class UpdateAsinSkuDropShipDataJobViewModel
    {
        public string ASIN { get; set; }
        public string SKU { get; set; }
        public string MAXPrice { get; set; }
        public string AmzPrice { get; set; }
        public string AvgCost { get; set; }
        public string DropShip { get; set; }
        public string DropShipQty { get; set; }
        public string DropShipComments { get; set; }
        public string RowNumber { get; set; }
    }
}
