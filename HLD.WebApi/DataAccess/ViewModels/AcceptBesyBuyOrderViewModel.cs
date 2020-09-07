using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
  public  class AcceptBesyBuyOrderViewModel
    {
        public List<OrderLine_accept> order_lines { get; set; }
    }
    public class OrderLine_accept
    {
        public bool accepted { get; set; }
        public string id { get; set; }
    }
}
