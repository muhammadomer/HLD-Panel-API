using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
  public  class ColorViewModel
    {
        public int ColorId { get; set; }
        public string  ColorName { get; set; }
        public string ColorCode { get; set; }
        public string ColorAlias { get; set; }
    }
}
