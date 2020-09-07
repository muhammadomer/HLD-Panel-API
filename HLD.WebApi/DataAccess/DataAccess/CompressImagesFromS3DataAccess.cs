using DataAccess.Helper;
using DataAccess.ViewModels;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataAccess
{
    public class CompressImagesFromS3DataAccess
    {
        public string connStr { get; set; }
        
        public CompressImagesFromS3DataAccess(IConnectionString connectionString)
        {
            connStr = connectionString.GetConnectionString();
            
        }


        public List<CompressImageViewModel> GetImagestoCompress()
        {
            List<CompressImageViewModel> listModel = new List<CompressImageViewModel>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmdd = new MySqlCommand(@"Select product_sku,image_name,Compress_image from bestBuyE2.product_images where  Compress_image is null and image_name is not null and image_name <> '';", conn);
                    cmdd.CommandType = System.Data.CommandType.Text;
                    MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(cmdd);
                    DataTable dt = new DataTable();
                    mySqlDataAdapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                       
                        foreach (DataRow dr in dt.Rows)
                        {
                            CompressImageViewModel model = new CompressImageViewModel();
                            model.sku = Convert.ToString(dr["product_sku"]);
                            model.imageName = Convert.ToString(dr["image_name"]);
                            model.CompressedImage = Convert.ToString(dr["Compress_image"]);
                            listModel.Add(model);
                        }
                    }


                }
                return listModel;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public bool UpdateASCompressedImage(List<CompressImageViewModel> item)
        {
          
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    foreach (var listModel in item)
                    {
                        MySqlCommand cmd = new MySqlCommand("P_UpdateImageCompressed", conn);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("_sku", listModel.sku);
                        cmd.Parameters.AddWithValue("_imageName", listModel.imageName);
                        cmd.ExecuteNonQuery();
                    }
                  
                }
                  
                return true;
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
