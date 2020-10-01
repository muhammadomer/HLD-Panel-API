using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class SaveQuotationSubVM
    {
        public int Quotation_Sub_Id { get; set; }
        public string MainSku { get; set; }
        public string SubSku { get; set; }
        public decimal Price { get; set; }
        public int latestqouteId { get; set; }
    }
}
