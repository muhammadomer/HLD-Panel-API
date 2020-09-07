using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class searchPOViewModel
    {
 
            public int startLimit { get; set; }
            public int endLimit { get; set; }
            public int vendorID { get; set; }
    } 
    public class searchPOitemViewModel
    {
        public int POId { get; set; }
        public int vendorID { get; set; }
        public string SKU { get; set; }
        public string Title { get; set; }
        public Boolean OpenQty { get; set; }

    }
}
