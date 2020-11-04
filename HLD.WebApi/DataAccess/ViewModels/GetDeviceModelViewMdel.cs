using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class GetDeviceModelViewMdel
    {
        public int ManufactureId { get; set; }
        public string Manufacture { get; set; }
        public string ManufactureModel { get; set; }
        public string DeviceModel { get; set; }
    }
}
