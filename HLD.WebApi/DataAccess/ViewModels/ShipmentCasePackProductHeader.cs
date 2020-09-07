using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class ShipmentCasePackProductHeader
    {
        public string ShipmentId { get; set; }
        public string ShipmentName { get; set; }
        public int Status { get; set; }
        public int ShipedQty { get; set; }
        public int SKUs { get; set; }
        public int POs { get; set; }
        public int NoOfBoxes { get; set; }
    }
}
