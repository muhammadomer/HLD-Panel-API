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
    public class BestBuyProductQtyMovementDataAcces
    {
        public string connStr { get; set; }
        public BestBuyProductQtyMovementDataAcces(IConnectionString connectionString)
        {
            connStr = connectionString.GetConnectionString();
        }

        public List<BestBuyQTYLogsDetailViewModel> GetAllBestBuyQtyMovementDetail()
        {
            List<BestBuyQTYLogsDetailViewModel> listViewModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetBestBuyQtyMovementDetailToShowOnWebPage", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            listViewModel = new List<BestBuyQTYLogsDetailViewModel>();
                            while (reader.Read())
                            {
                                BestBuyQTYLogsDetailViewModel ViewModel = new BestBuyQTYLogsDetailViewModel();
                                ViewModel.product_sku = Convert.ToString(reader["product_sku"]);
                                ViewModel.ds_qty = Convert.ToInt32(reader["ds_qty"]);
                                ViewModel.ds_status = Convert.ToString(reader["ds_status"]);
                                ViewModel.order_date = Convert.ToDateTime(reader["order_date"]);
                                ViewModel.update_status = Convert.ToString(reader["update_status"]);
                                ViewModel.bb_import_id = Convert.ToInt32(reader["bb_import_id"] != DBNull.Value ? reader["bb_import_id"] : 0);
                                ViewModel.comments = Convert.ToString(reader["comments"] != DBNull.Value ? reader["comments"] : "");
                                ViewModel.BBProductID = Convert.ToString(reader["BBProductID"] != DBNull.Value ? reader["BBProductID"] : "");
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

        public List<BestBuyQTYLogsDetailViewModel> GetAllBestBuyQtyQuery(string query)
        {
            List<BestBuyQTYLogsDetailViewModel> listModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query + " Order by order_date desc", conn);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    da.Fill(dataTable);
                    if (dataTable.Rows.Count > 0)
                    {
                        listModel = new List<BestBuyQTYLogsDetailViewModel>();
                        foreach (DataRow dataRow in dataTable.Rows)
                        {
                            BestBuyQTYLogsDetailViewModel ViewModel = new BestBuyQTYLogsDetailViewModel();
                            ViewModel.product_sku = Convert.ToString(dataRow["product_sku"]);
                            ViewModel.ds_qty = Convert.ToInt32(dataRow["ds_qty"]);
                            ViewModel.ds_status = Convert.ToString(dataRow["ds_status"]);
                            ViewModel.order_date = Convert.ToDateTime(dataRow["order_date"]);
                            ViewModel.update_status = Convert.ToString(dataRow["update_status"]);
                            ViewModel.bb_import_id = Convert.ToInt32(dataRow["bb_import_id"] != DBNull.Value ? dataRow["bb_import_id"] : 0);
                            ViewModel.comments = Convert.ToString(dataRow["comments"] != DBNull.Value ? dataRow["comments"] : "");
                            ViewModel.BBProductID = Convert.ToString(dataRow["BBProductID"] != DBNull.Value ? dataRow["BBProductID"] : "");
                            listModel.Add(ViewModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return listModel;
        }

    }
}




