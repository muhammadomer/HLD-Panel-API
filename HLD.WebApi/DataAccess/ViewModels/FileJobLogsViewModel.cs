using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class FileJobLogsViewModel
    {
        public int Success { get; set; }
        public int Fail { get; set; }
        public string FailMessage { get; set; }
    }
}
