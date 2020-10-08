using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class SaveAndEditShipmentCourierVM
    {
        public int ShipmentCourier_ID { get; set; }
        public string CourierCode { get; set; }
        public string CourierURL { get; set; }
    }
}
