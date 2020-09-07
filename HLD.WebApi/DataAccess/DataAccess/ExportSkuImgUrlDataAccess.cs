using DataAccess.Helper;
using DataAccess.ViewModels;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataAccess
{
   public class ExportSkuImgUrlDataAccess
    {
        public string connStr { get; set; }
        public ExportSkuImgUrlDataAccess(IConnectionString connectionString)
        {
            connStr = connectionString.GetConnectionString();
        }
        public List<ExportSkuImgUrlViewModel> ExportSkuImgUrl()
        {
            List<ExportSkuImgUrlViewModel> listViewModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetExportImgUrl", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            listViewModel = new List<ExportSkuImgUrlViewModel>();
                            while (reader.Read())
                            {
                                ExportSkuImgUrlViewModel ViewModel = new ExportSkuImgUrlViewModel();
                                ViewModel.Productsku = Convert.ToString(reader["product_sku"]);
                                ViewModel.Large_URL = Convert.ToString("https://s3.us-east-2.amazonaws.com/upload.hld.erp.images/" + reader["Large_URL"]);
                                ViewModel.Small_URL = Convert.ToString("https://s3.us-east-2.amazonaws.com/upload.hld.erp.images.thumbnail/" + reader["Small_URL"]);
                                listViewModel.Add(ViewModel);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return listViewModel;
        }
    }
}

