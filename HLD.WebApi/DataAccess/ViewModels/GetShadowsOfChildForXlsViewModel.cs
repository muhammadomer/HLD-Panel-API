using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class GetShadowsOfChildForXlsViewModel
    {

        public string sku { get; set; }
        public string title { get; set; }
        public int CompanyId { get; set; }
    }
    public class FileContents
    {
        public string ParentSKU { get; set; }
        public string ShadowSKU { get; set; }
        public int CompanyID { get; set; }
    }
}
