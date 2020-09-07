using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class AssignTagViewModel
    {
        public string SKu { get; set; }
        public List<TagsList> tags { get; set; }
    }
    public class TagsList
    {
        public int TagId { get; set; }
    }
}
