using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class ShipmentProductHeaderViewModel
    {
        public string ShipmentId { get; set; }
        public string ShipmentName { get; set; }
        public string Notes { get; set; }
        public string BoxId { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal Length { get; set; }
        public decimal Weight { get; set; }
        public int Status { get; set; }
        public int SKUs { get; set; }
        public int POs { get; set; }

        public List<ShipmentProductListViewModel> list { get; set; }
    }
}
