using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class GetFileJobsToRunViewModel
    {
        public int Job_Id { get; set; }
        public string Job_Type { get; set; }
        public string File_Bucket { get; set; }
        public string File_Name { get; set; }
       
    }
}
