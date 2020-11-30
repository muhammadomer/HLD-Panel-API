using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class GetShipedAndRecQtyViewModel
    {
        public double ShipedQty { get; set; }
        public double RecivedQty { get; set; }
        public double TotalCount { get; set; }
    }
}
