using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class FileUploadStatusLogViewModel
    {
        public int FileUploadJobDetail_id { get; set; }
        public int FileUploadJob_id { get; set; }
        public string Sku { get; set; }
        public string Status { get; set; }
        public string ErrorMessage { get; set; }
    }
}
