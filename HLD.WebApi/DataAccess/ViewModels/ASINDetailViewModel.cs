using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class ASINDetailViewModel
    {
        public DateTime? asin_date { get; set; }
        public string product_sku { get; set; }
        public string AmazonImagesListCombined { get; set; }
        public string ASIN { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string amazon_price { get; set; }
        public string S3BucketURL_mainImage { get; set; }
        public string S3BucketULR_image1 { get; set; }
        public string S3BucketURL_image2 { get; set; }
        public string AsinMainImage_Url { get; set; }
        public string AsinImage1_Url { get; set; }
        public string AsinImage2_Url { get; set; }
        public string OtherImagesURL { get; set; }
        public string ASINList { get; set; }
        public string BrandName { get; set; }
        public string Color { get; set; }
        public string feature_bullets { get; set; }
        public string MainImage_imgPath { get; set; }
        public string Image1_imgPath { get; set; }
        public string Image2_imgPath { get; set; }
        public List<string> AmazonImagesList { get; set; }
        public int AsinProductDetailID { get; set; }


    }
}
