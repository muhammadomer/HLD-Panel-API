using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class EmployeeRoleViewModel
    {
        public int RollId { get; set; }
        public string EmployeeRole { get; set; }
        public string Permissions { get; set; }
        public DateTime CreatedOn { get; set; }

    }
}
