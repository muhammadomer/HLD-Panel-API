using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class S3LogViewModel
    {
        public int Success { get; set; }
        public int job_id { get; set; }
        public int Fail { get; set; }
       public List<Messagelog> message { get; set; }
        
    }
    public class Messagelog
    {
        public string message { get; set; }
        public int row { get; set; }
        public string sku { get; set; }
    }
}
