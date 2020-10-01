using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class UpdateZincOrderViewModel
    {
        public int OrderId { get; set; }
        public int RecievedOrderQty { get; set; }
        public DateTime RecievedOrderDate { get; set; }
    }
}
