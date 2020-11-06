using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class ProductManufactureListViewModel
    {
        public int ManufactureId { get; set; }
        public int ParentID { get; set; }

        public string ManufactureName { get; set; }
        public string ManufactureModel { get; set; }

        public string DeviceModel { get; set; }
    }
}
