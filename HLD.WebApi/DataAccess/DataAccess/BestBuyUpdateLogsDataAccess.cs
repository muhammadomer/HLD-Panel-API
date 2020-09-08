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
    public class BestBuyUpdateLogsDataAccess
    {

        public string connStr { get; set; }
        public BestBuyUpdateLogsDataAccess(IConnectionString connectionString)
        {
            connStr = connectionString.GetConnectionString();
        }
        public int GetWatchlistLogsCount(int JobId)
        {
            int Records = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetBestBuyUpdatelogsCount", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    
                    cmd.Parameters.AddWithValue("_JobId", JobId);
                    Records = Convert.ToInt32(cmd.ExecuteScalar());
                    return Records;
                }
            }
            catch (Exception exp)
            {
                throw;
            }
        }

        public List<BestBuyUpdateLogsViewModel> GetWatchlistLogs(int JobId ,int limit,int offset)
        {
            List<BestBuyUpdateLogsViewModel> modelLog = new List<BestBuyUpdateLogsViewModel>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetBestBuyUpdatelogs", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_JobId", JobId);
                    cmd.Parameters.AddWithValue("_offset", offset);
                    cmd.Parameters.AddWithValue("_limit", limit);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            while (reader.Read())
                            {
                                BestBuyUpdateLogsViewModel ViewModel = new BestBuyUpdateLogsViewModel();
                                ViewModel.SKU= Convert.ToString(reader["SKU"]);
                                ViewModel.ProductId = Convert.ToInt32(reader["ProductId"]);
                                ViewModel.JobId = Convert.ToInt32(reader["JobId"]);
                                ViewModel.MSRP = Convert.ToDecimal(reader["MSRP"]);
                                ViewModel.UpdateSelllingPrice = Convert.ToDecimal(reader["UpdateSellingPrice"] != DBNull.Value ? reader["UpdateSellingPrice"] : "0");
                                ViewModel.Compress_image= Convert.ToString(reader["Compress_image"] != DBNull.Value ? reader["Compress_image"] : "");
                                ViewModel.image_name = Convert.ToString(reader["image_name"] != DBNull.Value ? reader["image_name"] : "");
                                ViewModel.importId = Convert.ToString(reader["importId"] != DBNull.Value ? reader["importId"] : "0");
                                modelLog.Add(ViewModel);
                            }
                        }
                    }
                }
                return modelLog;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}
