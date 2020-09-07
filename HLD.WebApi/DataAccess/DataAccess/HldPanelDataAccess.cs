using DataAccess.Helper;
using DataAccess.ViewModels;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace DataAccess.DataAccess
{
    public class HldPanelDataAccess
    {
        public string connStr { get; set; }
        public HldPanelDataAccess(IConnectionString connectionString)
        {
            connStr = connectionString.GetConnectionString();
        }

        public List<HldPanelViewModel> GetHldPanelOrderList(int endLimit)
        {
            List<HldPanelViewModel> _hldPanelModelResult = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_hldPanel_GetOrderList", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            _hldPanelModelResult = new List<HldPanelViewModel>();
                            while (reader.Read())
                            {
                                HldPanelViewModel hldPanelModel = new HldPanelViewModel();
                                hldPanelModel.Order_orderId = Convert.ToString(reader["order_id"]);
                                hldPanelModel.Order_sellerCloudID = Convert.ToString(reader["sellerCloudID"]);
                                hldPanelModel.Order_createdDate = Convert.ToDateTime(reader["created_date"]);
                                hldPanelModel.Order_acceptanceDecisionDate = Convert.ToDateTime(reader["acceptance_decision_date"]);
                                hldPanelModel.OrderLine_offer_sku = Convert.ToString(reader["offer_sku"]);
                                hldPanelModel.OrderLine_order_line_state = Convert.ToString(reader["order_line_state"]);
                                hldPanelModel.OrderLine_product_title = Convert.ToString(reader["product_title"]);
                                hldPanelModel.OrderLine_received_date = Convert.ToString(reader["received_date"]);
                                hldPanelModel.OrderLine_shipped_date = Convert.ToString(reader["shipped_date"]);
                                hldPanelModel.OrderLine_total_commission = Convert.ToString(reader["total_commission"]);
                                hldPanelModel.OrderLine_total_price = Convert.ToString(reader["total_price"]);
                                hldPanelModel.Shipping_shippingCompany = Convert.ToString(reader["shipping_company"]);
                                hldPanelModel.Shipping_shippingTracking = Convert.ToString(reader["shipping_tracking"]);
                                hldPanelModel.Customer_firstName = Convert.ToString(reader["firstname"]);
                                hldPanelModel.Customer_lastName = Convert.ToString(reader["lastname"]);
                                hldPanelModel.Customer_city = Convert.ToString(reader["city"]);
                                hldPanelModel.Customer_country = Convert.ToString(reader["country"]);
                                _hldPanelModelResult.Add(hldPanelModel);
                            }
                        }
                    }
                }
            }


            catch (Exception)
            {
                throw;
            }
            return _hldPanelModelResult;
        }
    }
}
