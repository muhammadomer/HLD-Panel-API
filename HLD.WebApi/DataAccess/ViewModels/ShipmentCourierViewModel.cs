using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class ShipmentCourierViewModel
    {
        public int ShipmentCourierId { get; set; }
        public string ShipingCompany { get; set; }
        public string CourierCode { get; set; }
        public string TrakingNumber { get; set; }
        public string ShipmentId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
