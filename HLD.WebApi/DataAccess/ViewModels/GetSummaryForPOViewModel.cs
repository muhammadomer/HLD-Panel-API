using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class GetSummaryForPOViewModel
    {
        public int ordQty { get; set; }
        public int recQty { get; set; }
        public int OpenQty { get; set; }
        public int shiQty { get; set; }
        public decimal orderamount { get; set; }
        public decimal ReceviedAmount { get; set; }
        public decimal OpenAmount { get; set; }
        public decimal shipedamount { get; set; }
    }
}
