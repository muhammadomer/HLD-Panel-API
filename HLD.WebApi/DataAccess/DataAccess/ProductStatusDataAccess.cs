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
  public  class ProductStatusDataAccess
    {
        public string connStr { get; set; }
        public ProductStatusDataAccess(IConnectionString connectionString)
        {
            connStr = connectionString.GetConnectionString();
        }

        public List<ProductStatusViewModel> GetAllProductStatus()
        {
            List<ProductStatusViewModel> listViewModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetProductStatus", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            listViewModel = new List<ProductStatusViewModel>();
                            while (reader.Read())
                            {
                                ProductStatusViewModel  ViewModel = new ProductStatusViewModel();
                                ViewModel.ProductStatusId = Convert.ToInt32(reader["product_status_id"]);
                                ViewModel.ProductStatusName = Convert.ToString(reader["product_status_name"]);
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
