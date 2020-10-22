using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class SaveSkuShadowViewModel
    {
        public SaveSkuShadowViewModel()
        {
            list = new List<SaveChildShadowViewModel>();
        }
        public int ParentId { get; set; }
        public List<SaveChildShadowViewModel> list { get; set; }
    }
    public class SaveChildShadowViewModel
    {
        public string Sku { get; set; }
        public int ChildId { get; set; }
    }
}
