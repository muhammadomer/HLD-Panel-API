using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class ProductImagesViewModel
    {
        public int ProductImageId { get; set; }
        public byte[] Image { get; set; }
        public int ProductId { get; set; }
        public string ImageURL { get; set; }
    }
}
