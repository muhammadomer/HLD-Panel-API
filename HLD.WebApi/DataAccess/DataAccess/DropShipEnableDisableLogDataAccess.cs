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
    public class DropShipEnableDisableLogDataAccess
    {
        public string connStr { get; set; }

        public DropShipEnableDisableLogDataAccess(IConnectionString connectionString)
        {
            connStr = connectionString.GetConnectionString();
        }
        public List<DropShipEnableDisableLogViewModel> GetAll_DS_Log(string sku)
        {
            List<DropShipEnableDisableLogViewModel> listViewModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetDropshipEnableDisableLog_SKU", conn);
                    cmd.Parameters.AddWithValue("sku", sku);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            listViewModel = new List<DropShipEnableDisableLogViewModel>();
                            while (reader.Read())
                            {
                                DropShipEnableDisableLogViewModel ViewModel = new DropShipEnableDisableLogViewModel();
                                ViewModel.ID = Convert.ToInt32(reader["ID"]);
                                ViewModel.LastUpdate = Convert.ToDateTime(reader["last_update"]);
                                ViewModel.ProductSku = Convert.ToString(reader["product_sku"]);
                                ViewModel.Qty = Convert.ToInt32(reader["Qty"] != DBNull.Value ? reader["Qty"] :"0");
                                ViewModel.Comments = Convert.ToString(reader["comments"] != DBNull.Value ? reader["comments"] :""  );
                                ViewModel.EnableDisable = Convert.ToBoolean(reader["dropship_enable_disable"] != DBNull.Value ? reader["dropship_enable_disable"] :"false" );
                                if (reader["offer_start_date"].ToString() != string.Empty)
                                {
                                    ViewModel.OfferStartDate = Convert.ToDateTime(reader["offer_start_date"].ToString());
                                }
                                if (reader["offer_end_date"].ToString() != string.Empty)
                                {
                                    ViewModel.OfferEndDate = Convert.ToDateTime(reader["offer_end_date"].ToString());
                                }
                                ViewModel.BBProductID = Convert.ToString(reader["bb_product_id"] != DBNull.Value ? reader["bb_product_id"]:0 );
                                ViewModel.MSRP = Convert.ToDecimal(reader["MSRP"] != DBNull.Value ? reader["MSRP"] :0 );
                                ViewModel.SellingFee = Convert.ToDecimal(reader["selling_fee"] != DBNull.Value ? reader["selling_fee"] :0 );
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
