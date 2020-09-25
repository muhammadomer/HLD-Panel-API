using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class ProductwareHousesViewModel
    {
        public string SKU { get; set; }
        public int DropShip_Canada { get; set; }
        public int DropShip_USA { get; set; }
        public int FBA_Canada { get; set; }
        public int FBA_USA { get; set; }
        public int HLD_CA1 { get; set; }
        public int HLD_CA2 { get; set; }
        public int HLD_CN1 { get; set; }
        public int HLD_Interim { get; set; }
        public int HLD_Tech1 { get; set; }
        public int Interim_FBA_CA { get; set; }
        public int Interim_FBA_USA { get; set; }
        public int NY_14305 { get; set; }
        public int Shipito { get; set; }
    }
}
