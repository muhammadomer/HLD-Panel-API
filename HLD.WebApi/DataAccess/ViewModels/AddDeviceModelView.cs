using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class AddDeviceModelView
    {
        public int ManufactureId { get; set; }
        public int ManufactureModelId { get; set; }
        public string DeviceModel { get; set; }
    }
}
