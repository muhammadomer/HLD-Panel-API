using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class UpdatePOAcceptedViewModel
    {
        public int IsAccepted { get; set; }
        public int POId { get; set; }
       
        public DateTime POLastUpdate { get; set; }
        public DateTime POArrivalDate { get; set; }
       
    }

    public class UpdatePOExchangeViewModel
    {
     
        public int POId { get; set; }
        public decimal ExchangeRate { get; set; }
    }
}
