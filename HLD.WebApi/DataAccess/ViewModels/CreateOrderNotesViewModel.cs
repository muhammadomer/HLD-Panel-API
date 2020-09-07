using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class CreateOrderNotesViewModel
    {
        public int EntityID { get; set; }
        public int Category { get; set; }
        public int NoteID { get; set; }
        public string Note { get; set; }
        public DateTime? AuditDate { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedByEmail { get; set; }
    }
}
