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
   public class BestBuyOrderFromBBDataAccessNew
    {
        public string connStr { get; set; }
      //  public string DOTconnStr { get; set; }
        public BestBuyOrderFromBBDataAccessNew(IConnectionString connectionString)
        {
            // connStr = connectionString.GetPhpConnectionString();
              connStr = connectionString.GetConnectionString();
        }
        public List<string> GetOrderAlreadyExist(string scOrderIds)
        {
            List<string> sclist = new List<string>();
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_CheckExistingordersForBB", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("SCId", scOrderIds);
                    MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    mySqlDataAdapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {

                        foreach (DataRow dr in dt.Rows)
                        {
                            string scid;
                            scid = Convert.ToString(dr["order_id"].ToString());
                            sclist.Add(scid);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return sclist;
        }
        public int SaveBestBuyOrderINOrder(ViewModels.GetOrdersFromBestBuyViewModel.OrderBB order)
        {
            int BBOrderID = 0;
            int IsBox = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    MySqlCommand cmd = new MySqlCommand("P_SaveBestBuyOrdersFromBestBuyV2", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_order_id", order.order_id);
                    cmd.Parameters.AddWithValue("_commercial_id", order.commercial_id);
                    cmd.Parameters.AddWithValue("_customer_id", order.customer.customer_id);
                    cmd.Parameters.AddWithValue("_shipping_id", order.shipping_tracking);
                    cmd.Parameters.AddWithValue("_can_cancel", order.can_cancel);
                    cmd.Parameters.AddWithValue("_order_state", order.order_state);
                    cmd.Parameters.AddWithValue("_total_commission", order.total_commission);
                    cmd.Parameters.AddWithValue("_total_price", order.total_price);
                    cmd.Parameters.AddWithValue("_created_date", order.created_date);
                    cmd.Parameters.AddWithValue("_acceptance_decision_date", order.acceptance_decision_date);
                    cmd.Parameters.AddWithValue("_shipping_price", order.shipping_price);
                    
                    cmd.Parameters.AddWithValue("_firstname", order.customer.shipping_address != null ? order.customer.shipping_address.firstname : "");
                    cmd.Parameters.AddWithValue("_lastname", order.customer.shipping_address != null ? order.customer.shipping_address.lastname : "");
                    cmd.Parameters.AddWithValue("_phone", order.customer.shipping_address != null ? order.customer.shipping_address.phone : "");
                    cmd.Parameters.AddWithValue("_phone_secondary", order.customer.shipping_address != null ? order.customer.shipping_address.phone_secondary : "");
                    cmd.Parameters.AddWithValue("_state", order.customer.shipping_address != null ? order.customer.shipping_address.state : "");
                    cmd.Parameters.AddWithValue("_street_1", order.customer.shipping_address != null ? order.customer.shipping_address.street_1 : "");
                    cmd.Parameters.AddWithValue("_street_2", order.customer.shipping_address != null ? order.customer.shipping_address.street_2 : "");
                    cmd.Parameters.AddWithValue("_zip_code", order.customer.shipping_address != null ? order.customer.shipping_address.zip_code : "");
                    cmd.Parameters.AddWithValue("_city", order.customer.shipping_address != null ? order.customer.shipping_address.city : "");
                    cmd.Parameters.AddWithValue("_country", order.customer.shipping_address != null ? order.customer.shipping_address.country : "");
                    if (order.customer.shipping_address != null && order.customer.shipping_address.street_1.ToLower().Contains("box"))
                    {
                       IsBox = 1;
                    }
                    cmd.Parameters.AddWithValue("_IsBox", IsBox);
                    cmd.Parameters.Add("_bbe2_orders_id", MySqlDbType.Int32, 500);
                    cmd.Parameters["_bbe2_orders_id"].Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    BBOrderID = Convert.ToInt32(cmd.Parameters["_bbe2_orders_id"].Value);
                }
            }
            catch (Exception ex)
            {

            }
            return BBOrderID;
        }
        public bool SaveBestBuyOrderINOrderLines(ViewModels.GetOrdersFromBestBuyViewModel.OrderBB order, int bbe2_orders_id)
        {
            bool BBOrderID = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    foreach (var item in order.order_lines)
                    {


                        MySqlCommand cmd = new MySqlCommand("P_SaveOrderLinesFromBBV2", conn);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("_bbe2_orders_id", bbe2_orders_id);
                        cmd.Parameters.AddWithValue("_order_line_id", item.order_line_id);
                        cmd.Parameters.AddWithValue("_order_id", order.order_id);
                        cmd.Parameters.AddWithValue("_offer_sku", item.offer_sku);
                        cmd.Parameters.AddWithValue("_quantity", item.quantity);
                        cmd.Parameters.AddWithValue("_received_date", item.received_date);
                        cmd.Parameters.AddWithValue("_shipped_date", item.shipped_date);
                        cmd.Parameters.AddWithValue("_product_title", item.product_title);
                        cmd.Parameters.AddWithValue("_total_price", item.price_unit);
                        cmd.Parameters.AddWithValue("_total_commission", item.total_commission);
                        cmd.Parameters.AddWithValue("_order_line_state", item.order_line_state);
                        cmd.Parameters.AddWithValue("_ShippingFee", item.shipping_price);
                        cmd.Parameters.AddWithValue("_Commission_Fee", item.commission_fee);
                        cmd.Parameters.AddWithValue("_commission_rate_vat", item.commission_rate_vat);
                        cmd.Parameters.AddWithValue("_TaxGST", item.taxes.Where(p => p.code == "GST").Select(p => p.amount).FirstOrDefault());
                        cmd.Parameters.AddWithValue("_TaxPST", item.taxes.Where(p => p.code != "GST").Select(p => p.amount).FirstOrDefault());

                        cmd.ExecuteNonQuery();

                    }



                    BBOrderID = true;
                }

            }
            catch (Exception ex)
            {

            }
            return BBOrderID;
        }
        public bool SaveBestBuyOrderINCustomerShipping(ViewModels.GetOrdersFromBestBuyViewModel.OrderBB order, int bbe2_orders_id)
        {
            bool BBOrderID = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    MySqlCommand comd = new MySqlCommand("P_SaveBestBuyCustomerAndShippingFromBB", conn);
                    comd.CommandType = System.Data.CommandType.StoredProcedure;
                    comd.Parameters.AddWithValue("_bbe2_orders_id", bbe2_orders_id);
                    comd.Parameters.AddWithValue("_commercial_id", order.commercial_id);
                    comd.Parameters.AddWithValue("_shipping_carrier_code", order.shipping_carrier_code);
                    comd.Parameters.AddWithValue("_shipping_company", order.shipping_company);
                    comd.Parameters.AddWithValue("_shipping_price", order.shipping_price);
                    comd.Parameters.AddWithValue("_shipping_tracking", order.shipping_tracking);
                    comd.Parameters.AddWithValue("_shipping_tracking_url", order.shipping_tracking_url);
                    comd.Parameters.AddWithValue("_shipping_type_code", order.shipping_type_code);
                    comd.Parameters.AddWithValue("_shipping_type_label", order.shipping_type_label);
                    comd.Parameters.AddWithValue("_shipping_zone_code", order.shipping_zone_code);
                    comd.Parameters.AddWithValue("_shipping_zone_label", order.shipping_zone_label);
                    comd.ExecuteNonQuery();


                    BBOrderID = true;
                }

            }
            catch (Exception ex)
            {

            }
            return BBOrderID;
        }
        public bool SaveBestBuyOrderINOrderLookUp(ViewModels.GetOrdersFromBestBuyViewModel.OrderBB order, int bbe2_orders_id)
        {
            bool BBOrderID = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    MySqlCommand comand = new MySqlCommand("P_SaveBestBuyOrderLookup", conn);
                    comand.CommandType = System.Data.CommandType.StoredProcedure;
                    comand.Parameters.AddWithValue("_order_id", order.order_id);
                    comand.Parameters.AddWithValue("_created_date", order.created_date);

                    comand.ExecuteNonQuery();

                    BBOrderID = true;
                }

            }
            catch (Exception ex)
            {

            }
            return BBOrderID;
        }
        public List<DropShipAndQtyOrderViewModel> GeQtyAndDropShip(List<string> _orderSKU)
        {
            List<DropShipAndQtyOrderViewModel> sclist = new List<DropShipAndQtyOrderViewModel>();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    foreach (var item in _orderSKU)
                    {
                        MySqlCommand cmd = new MySqlCommand("P_CheckDropShipAndQuantityOFSKUCopy", conn);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("_orderSKU", item);
                        MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        mySqlDataAdapter.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {

                            foreach (DataRow dr in dt.Rows)
                            {
                                DropShipAndQtyOrderViewModel scid = new DropShipAndQtyOrderViewModel();
                                scid.Qty = dr["qty_avaiable"] != DBNull.Value ? Convert.ToInt32(dr["qty_avaiable"].ToString()) : 0;
                                scid.Status = Convert.ToString(dr["dropship_status"].ToString());
                                scid.image = Convert.ToString(dr["image_name"].ToString());
                                scid.SKU = item;
                                sclist.Add(scid);
                            }
                        }
                        else
                        {
                            DropShipAndQtyOrderViewModel scid = new DropShipAndQtyOrderViewModel();
                            scid.Qty = 0;
                            scid.Status = "0";
                            scid.SKU = item;
                            scid.image = "";
                            sclist.Add(scid);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return sclist;
        }
        public bool UpdateBestBuyOrderINOrder(ViewModels.GetOrdersFromBestBuyViewModel.OrderBB order)
        {
            int IsBox = 0;
            bool BBOrderID = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    MySqlCommand cmd = new MySqlCommand("P_UpdateBestBuyOrdersFromBestBuyV1", conn);
                   // MySqlCommand cmd = new MySqlCommand("P_UpdateBestBuyOrdersFromBestBuy", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("_commercial_id", order.commercial_id);
                
                    cmd.Parameters.AddWithValue("_shipping_id", order.shipping_tracking);
                    cmd.Parameters.AddWithValue("_can_cancel", order.can_cancel);
                    cmd.Parameters.AddWithValue("_order_state", order.order_state);
                    cmd.Parameters.AddWithValue("_customer_id", order.customer.customer_id);
                    cmd.Parameters.AddWithValue("_firstname", order.customer.shipping_address != null ? order.customer.shipping_address.firstname : "");
                    cmd.Parameters.AddWithValue("_lastname", order.customer.shipping_address != null ? order.customer.shipping_address.lastname : "");
                    cmd.Parameters.AddWithValue("_phone", order.customer.shipping_address != null ? order.customer.shipping_address.phone : "");
                    cmd.Parameters.AddWithValue("_phone_secondary", order.customer.shipping_address != null ? order.customer.shipping_address.phone_secondary : "");
                    cmd.Parameters.AddWithValue("_state", order.customer.shipping_address != null ? order.customer.shipping_address.state : "");
                    cmd.Parameters.AddWithValue("_street_1", order.customer.shipping_address != null ? order.customer.shipping_address.street_1 : "");
                    cmd.Parameters.AddWithValue("_street_2", order.customer.shipping_address != null ? order.customer.shipping_address.street_2 : "");
                    cmd.Parameters.AddWithValue("_zip_code", order.customer.shipping_address != null ? order.customer.shipping_address.zip_code : "");
                    cmd.Parameters.AddWithValue("_city", order.customer.shipping_address != null ? order.customer.shipping_address.city : "");
                    cmd.Parameters.AddWithValue("_country", order.customer.shipping_address != null ? order.customer.shipping_address.country : "");
                    cmd.Parameters.AddWithValue("_acceptance_decision_date", order.acceptance_decision_date);
                    if (order.customer.shipping_address != null && order.customer.shipping_address.street_1.ToLower().Contains("box"))
                    {
                        IsBox = 1;
                    }
                    cmd.Parameters.AddWithValue("_IsBox", IsBox);

                    cmd.ExecuteNonQuery();
                    BBOrderID = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return BBOrderID;
        }
        public bool UpdateBestBuyOrderINOrderLines(ViewModels.GetOrdersFromBestBuyViewModel.OrderBB order)
        {
            bool BBOrderID = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    foreach (var item in order.order_lines)
                    {


                        MySqlCommand cmd = new MySqlCommand("P_UpdateBestBuyOrderLinesFromBBV2", conn);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("_order_line_id", item.order_line_id);
                        cmd.Parameters.AddWithValue("_order_id", order.order_id);
                        cmd.Parameters.AddWithValue("_offer_sku", item.offer_sku);
                        cmd.Parameters.AddWithValue("_quantity", item.quantity);
                        cmd.Parameters.AddWithValue("_received_date", item.received_date);
                        cmd.Parameters.AddWithValue("_shipped_date", item.shipped_date);
                        cmd.Parameters.AddWithValue("_product_title", item.product_title);
                        cmd.Parameters.AddWithValue("_total_price", item.price_unit);
                        cmd.Parameters.AddWithValue("_total_commission", item.total_commission);
                        cmd.Parameters.AddWithValue("_order_line_state", item.order_line_state);
                        cmd.Parameters.AddWithValue("_Commission_Fee", item.commission_fee);
                        cmd.Parameters.AddWithValue("_commission_rate_vat", item.commission_rate_vat);
                        cmd.Parameters.AddWithValue("_TaxGST", item.taxes.Where(p => p.code == "GST").Select(p => p.amount).FirstOrDefault());
                        cmd.Parameters.AddWithValue("_TaxPST", item.taxes.Where(p => p.code != "GST").Select(p => p.amount).FirstOrDefault());

                        cmd.ExecuteNonQuery();
                    }
                    BBOrderID = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return BBOrderID;
        }
        public bool UpdateBestBuyOrderINCustomerShipping(ViewModels.GetOrdersFromBestBuyViewModel.OrderBB order)
        {
            bool BBOrderID = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    MySqlCommand comd = new MySqlCommand("P_UpdateBestBuyCustomerAndShippingFromBB", conn);
                    comd.CommandType = System.Data.CommandType.StoredProcedure;

                    comd.Parameters.AddWithValue("_commercial_id", order.commercial_id);
                   
                    comd.Parameters.AddWithValue("_shipping_carrier_code", order.shipping_carrier_code);
                    comd.Parameters.AddWithValue("_shipping_company", order.shipping_company);
                    comd.Parameters.AddWithValue("_shipping_price", order.shipping_price);
                    comd.Parameters.AddWithValue("_shipping_tracking", order.shipping_tracking);
                    comd.Parameters.AddWithValue("_shipping_tracking_url", order.shipping_tracking_url);
                    comd.Parameters.AddWithValue("_shipping_type_code", order.shipping_type_code);
                    comd.Parameters.AddWithValue("_shipping_type_label", order.shipping_type_label);
                    comd.Parameters.AddWithValue("_shipping_zone_code", order.shipping_zone_code);
                    comd.Parameters.AddWithValue("_shipping_zone_label", order.shipping_zone_label);
                    comd.ExecuteNonQuery();
                    BBOrderID = true;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return BBOrderID;
        }

        public List<string> GetBestBuyOrderIdsToUpdate()
        {
            List<string> list = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetOrderInWaiting", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            list = new List<string>();
                            while (reader.Read())
                            {


                                string BBOrderID = Convert.ToString(reader["order_id"]);

                                list.Add(BBOrderID);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }
    }
}
