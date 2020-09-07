using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class CurrencyExchangeViewModel
    {
        public DateTime dateTime { get; set; }
        public decimal USD_To_CAD { get; set; }
        public decimal USD_To_CNY { get; set; }
        public bool IsActive { get; set; }
        public int CurrencyExchangeID { get; set; }
    }
}
