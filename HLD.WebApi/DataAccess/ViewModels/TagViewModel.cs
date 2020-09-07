using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hld.WebApi.ViewModel
{
    public class TagViewModel
    {
        public int? TagId { get; set; }
        [Display(Name ="Tag Name")]
        [Required(ErrorMessage ="Tag Name is required")]
        public string TagName { get; set; }
        [Display(Name = "Tag Color")]
        public string TagColor { get; set; }
    }
}
