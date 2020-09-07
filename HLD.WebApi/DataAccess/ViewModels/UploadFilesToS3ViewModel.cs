using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class UploadFilesToS3ViewModel
    {
        public string JobType { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        
    }
}
