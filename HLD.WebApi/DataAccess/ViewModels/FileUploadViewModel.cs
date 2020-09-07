using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class FileUploadViewModel
    {  
        public string FileName { get; set; }
        public string FileType { get; set; }
        public DateTime? UploadDate { get; set; }
        public string NoOfErrors { get; set; }
        public string NoOfSuccess { get; set; }
        public string IsUploaded { get; set; }
        public string FileExtension { get; set; }
    }
}
