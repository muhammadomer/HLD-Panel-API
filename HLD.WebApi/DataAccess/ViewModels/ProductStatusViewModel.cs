using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class ProductStatusViewModel
    {
        public int ProductStatusId { get; set; }
        public string ProductStatusName { get; set; }
        public bool IsActive { get; set; }
    }
}
