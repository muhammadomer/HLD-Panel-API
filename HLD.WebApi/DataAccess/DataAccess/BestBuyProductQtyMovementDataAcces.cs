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
        public int GetLogsCount(string product_sku, string ds_status, string BBProductID, string CurrentDate, string PreviousDate, string update_status)
        {
            int counter = 0;
            try
            {
                if (string.IsNullOrEmpty(BBProductID) || BBProductID == "undefined")
                    BBProductID = "";
                if (string.IsNullOrEmpty(CurrentDate) || BBProductID == "undefined")
                    CurrentDate = "";

                if (string.IsNullOrEmpty(PreviousDate) || BBProductID == "undefined")
                    PreviousDate = "";
                if (string.IsNullOrEmpty(ds_status) || ds_status == "undefined")
                    ds_status = "";
                if (string.IsNullOrEmpty(product_sku) || product_sku == "undefined")
                    product_sku = "";
                if (string.IsNullOrEmpty(update_status) || product_sku == "undefined")
                    update_status = "";

                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetDropshipCount", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_fromDate", PreviousDate);
                    cmd.Parameters.AddWithValue("_toDate", CurrentDate);
                    cmd.Parameters.AddWithValue("_BBProductID", BBProductID);
                    cmd.Parameters.AddWithValue("_product_sku", product_sku);
                    cmd.Parameters.AddWithValue("_ds_status", ds_status);
                    cmd.Parameters.AddWithValue("_update_status", update_status);
                    counter = Convert.ToInt32(cmd.ExecuteScalar());                  
                    conn.Close();
                   
                }
            }
            catch (Exception exp)
            {

            }
            return counter;
        }
        public List<BestBuyQTYLogsDetailViewModel> DropshipQtyList(string DateTo, string DateFrom, int limit, int offset, string product_sku, string ds_status, string BBProductID,string update_status)
        {
            List<BestBuyQTYLogsDetailViewModel> listModel = new List<BestBuyQTYLogsDetailViewModel>();
            try
            {

                if (string.IsNullOrEmpty(BBProductID) || BBProductID == "undefined")
                    BBProductID = "";
               
                if (string.IsNullOrEmpty(DateFrom) || BBProductID == "undefined")
                    DateFrom = "";
                if (string.IsNullOrEmpty(DateTo) || BBProductID == "undefined")
                    DateTo = "";
                if (string.IsNullOrEmpty(ds_status) || ds_status == "undefined")
                    ds_status = "";
                if (string.IsNullOrEmpty(product_sku) || product_sku == "undefined")
                    product_sku = "";
                if (string.IsNullOrEmpty(update_status) || product_sku == "undefined")
                    update_status = "";
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetDropshipCountList", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_fromDate", DateFrom);
                    cmd.Parameters.AddWithValue("_toDate", DateTo);
                    cmd.Parameters.AddWithValue("_BBProductID", BBProductID);
                    cmd.Parameters.AddWithValue("_product_sku", product_sku);
                    cmd.Parameters.AddWithValue("_ds_status", ds_status);             
                    cmd.Parameters.AddWithValue("_limit", limit);
                    cmd.Parameters.AddWithValue("_offset", offset);
                    cmd.Parameters.AddWithValue("_update_status", update_status);
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
                            ViewModel.UpdatedOnBB = Convert.ToDateTime(dataRow["UpdatedOnBB"] != DBNull.Value ? dataRow["UpdatedOnBB"] : (DateTime?)null);                           
                            ViewModel.update_status = Convert.ToString(dataRow["is_ds_status_updated_id"]);
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
                throw ex;
            }
            return listModel;
        }
    }
}




