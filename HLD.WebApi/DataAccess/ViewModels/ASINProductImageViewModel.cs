using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class ASINProductImageViewModel
    {
        public string ASIN { get; set; }
        public string BucketName { get; set; }
        public string KeyName { get; set; }
    }
}
