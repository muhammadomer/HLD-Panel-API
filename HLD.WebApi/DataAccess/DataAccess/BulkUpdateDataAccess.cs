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
   public class BulkUpdateDataAccess
    {
        public string connStr { get; set; }
        public BulkUpdateDataAccess(IConnectionString connectionString)
        {
            connStr = connectionString.GetConnectionString();
           
        }
        public List<BulkUpdateViewModel> GetBulkUpdate(List<string> shadowSku)
        {
            List<BulkUpdateViewModel> listModel = new List<BulkUpdateViewModel>();
            try
            {
                foreach (var item in shadowSku)
                {
                    using (MySqlConnection conn = new MySqlConnection(connStr))
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand("P_GetAllBulkUpdates", conn);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("_shadowSku", item);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {

                                while (reader.Read())
                                {
                                    BulkUpdateViewModel model = new BulkUpdateViewModel();
                                    model.AmazonMerchantSKU = Convert.ToString(reader["AmazonMerchantSKU"] != DBNull.Value ? reader["AmazonMerchantSKU"] : "");
                                    model.AmazonEnabled = Convert.ToString(reader["AmazonEnabled"] != DBNull.Value ? reader["AmazonEnabled"] : "");
                                    model.ASIN = Convert.ToString(reader["ASIN"] != DBNull.Value ? reader["ASIN"] : "");
                                    model.FulfilledBy = Convert.ToString(reader["FulfilledBy"] != DBNull.Value ? reader["FulfilledBy"] : "");
                                    model.AmazonFBASKU = Convert.ToString(reader["AmazonFBASKU"] != DBNull.Value ? reader["AmazonFBASKU"] : "");
                                    model.WebsiteEnabled = Convert.ToString(reader["WebsiteEnabled"] != DBNull.Value ? reader["WebsiteEnabled"] : "");
                                    listModel.Add(model);
                                }
                            }
                        }
                    }
                }
                
            }


            catch (Exception ex)
            {

                throw ex;
            }
            return listModel;
        }

        public bool EditBulkUpdate(EditBulkUpdateViewModel viewModel)
        {
            bool status = false;
            try
            {
               
                    using (MySqlConnection conn = new MySqlConnection(connStr))
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand("P_EditBulkUpdate", conn);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("_amazonMerchantSKU", viewModel.AmazonMerchantSKU);
                        cmd.Parameters.AddWithValue("_amazonEnabled", viewModel.AmazonEnabled);
                        cmd.Parameters.AddWithValue("_aSIN", viewModel.ASIN);
                        cmd.Parameters.AddWithValue("_fulfilledBy", viewModel.FulfilledBy);
                        cmd.Parameters.AddWithValue("_amazonFBASKU", viewModel.AmazonFBASKU);
                        cmd.Parameters.AddWithValue("_websiteEnabled", viewModel.WebsiteEnabled);
                        cmd.Parameters.AddWithValue("_shadowSku", viewModel.ShadowOffSKU);
                        cmd.Parameters.AddWithValue("_childSku", viewModel.ChildSKU);
                        cmd.ExecuteNonQuery();
                        status = true;
                    }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
    }
}
