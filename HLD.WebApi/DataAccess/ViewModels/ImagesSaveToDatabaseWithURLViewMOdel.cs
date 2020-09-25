using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class ImagesSaveToDatabaseWithURLViewMOdel
    {
        public string product_Sku { get; set; }
        public string FileName { get; set; }
        public string ImageURL { get; set; }
        public bool isImageExistInSC { get; set; }
    }
}
