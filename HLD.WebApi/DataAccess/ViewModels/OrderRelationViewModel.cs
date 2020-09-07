using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
  public class OrderRelationViewModel
    {
        public int SC_ParentID { get; set; }
        public int SC_ChildID { get; set; }
        public string BB_OrderID { get; set; }
    }
    public class OrderRelationToSaveViewModel
    {
     
        public int SC_ChildID { get; set; }
      
    }
}
