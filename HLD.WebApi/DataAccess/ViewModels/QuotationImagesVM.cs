using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class QuotationImagesVM
    {
        public int Quotation_Images_Id { get; set; }
        public int LastSubQouteId { get; set; }
        public string Sku { get; set; }
        public string Images { get; set; }

    }
}
