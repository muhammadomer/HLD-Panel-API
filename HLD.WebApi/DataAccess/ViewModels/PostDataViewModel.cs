using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class PostDataViewModel
    {
        public int? idPostEditor { get; set; }
        public string postdata { get; set; }
        public string posttitle { get; set; }
        public int catagoryid { get; set; }
        public DateTime? Post_date { get; set; }
        public List<PostData> listdatacatagory { get; set; }

    }
    public class PostData
    {
        public string catagory { get; set; }
        public int catagoryid { get; set; }
    }

}
