using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class ShipmentCourierInfoViewModel
    {
        public string ShipmentId { get; set; }
        public string CourierCode { get; set; }
        public string TrakingNumber { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
