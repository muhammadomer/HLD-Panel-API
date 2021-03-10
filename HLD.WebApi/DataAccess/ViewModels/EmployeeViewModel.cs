using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int EmployeeRole { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedOn { get; set; }
        public string EmployeeName { get; set; }
    }
}
