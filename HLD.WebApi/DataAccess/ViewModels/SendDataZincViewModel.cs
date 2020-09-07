using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class SendDataZincViewModel
    {
        public string Asin { get; set; }
        public string OrderlineId { get; set; }
        public double MaxPrice { get; set; }
    }
}
