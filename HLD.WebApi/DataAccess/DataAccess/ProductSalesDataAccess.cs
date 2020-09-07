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
    public class ProductSalesDataAccess
    {
         
        public string connStr { get; set; }
        public ProductSalesDataAccess(IConnectionString connectionString)
        {
            connStr = connectionString.GetConnectionString();
        }
        public List<ProductSalesViewModel> GetProductSalesPredict(string dateFrom, string dateTo, int startLimit, int endLimit, string SortColumn,string SortType)
        {
            List<ProductSalesViewModel> listViewModel = null;
            try
            {
                System.Data.DataSet ds = new System.Data.DataSet();
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetProductSalesRecord", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("dateFrom", dateFrom);
                    cmd.Parameters.AddWithValue("dateTo", dateTo);
                    cmd.Parameters.AddWithValue("startLimit", startLimit);
                    cmd.Parameters.AddWithValue("endLimit", endLimit);
                    cmd.Parameters.AddWithValue("SortColumn", SortColumn);
                    cmd.Parameters.AddWithValue("SortType", SortType);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            listViewModel = new List<ProductSalesViewModel>();
                            while (reader.Read())
                            {
                                ProductSalesViewModel ViewModel = new ProductSalesViewModel();
                                ViewModel.No_of_Orders = Convert.ToInt32(reader["No_of_Orders"]);
                                ViewModel.Qty = Convert.ToInt32(reader["Qty"]);
                                ViewModel.offer_sku = Convert.ToString(reader["offer_sku"]);
                                ViewModel.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                                ViewModel.total_Sales = Convert.ToDecimal(reader["total_Sales"]);
                                ViewModel.Total_Amount = Convert.ToDecimal(reader["Total_Amount"]);
                                ViewModel.P_L = Convert.ToDecimal(reader["P_L"]);
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

        public List<int> GetSellerCloudOrderWhichAreExistsPredict(string sellerCloudOrderList)
        {
            List<int> ordersList = new List<int>();
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("P_GetAllExistingOrders", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("sellerCloudIds", sellerCloudOrderList);

                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ordersList.Add(Convert.ToInt32(reader["OrderID"]));
                    }
                }

            }
            return ordersList;
        }

        //public bool SaveAllMarketPlacesOrdersFromSC(List<SellerProductDataViewModel> viewModel)
        //{
        //    bool status = false;
        //    try
        //    {
        //        using (MySqlConnection conn = new MySqlConnection(connStr))
        //        {
        //            conn.Open();
        //            MySqlCommand cmd = new MySqlCommand("P_SaveOrderRelation", conn);
        //            cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //            cmd.Parameters.AddWithValue("SC_ParentID", viewModel.SC_ParentID);
        //            cmd.Parameters.AddWithValue("SC_ChildID", viewModel.SC_ChildID);
        //            cmd.Parameters.AddWithValue("BB_OrderID", viewModel.BB_OrderID);
        //            cmd.ExecuteNonQuery();
        //            status = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return status;
        //}

        public void SaveAllMarketPlacesOrdersFromSC(List<SellerProductDataViewModel> list)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("ProductID", typeof(System.String)));
                dt.Columns.Add(new DataColumn("Qty", typeof(System.Int32)));
                dt.Columns.Add(new DataColumn("AdjustedSitePrice", typeof(System.Decimal)));
                dt.Columns.Add(new DataColumn("FinalShippingFee", typeof(System.Decimal)));
                dt.Columns.Add(new DataColumn("ShippingStatus", typeof(System.Int32)));
                dt.Columns.Add(new DataColumn("PaymentStatus", typeof(System.Int32)));
                dt.Columns.Add(new DataColumn("StatusCode", typeof(System.Int32)));
                dt.Columns.Add(new DataColumn("GrandTotal", typeof(System.Decimal)));
                dt.Columns.Add(new DataColumn("AverageCost", typeof(System.Decimal)));
                dt.Columns.Add(new DataColumn("FinalValueFee", typeof(System.Decimal)));
                dt.Columns.Add(new DataColumn("OrderID", typeof(System.Int32)));
                dt.Columns.Add(new DataColumn("OrderSourceOrderID", typeof(System.String)));
                dt.Columns.Add(new DataColumn("TimeOfOrder", typeof(System.DateTime)));
                dt.Columns.Add(new DataColumn("CompanyID", typeof(System.Int32)));
                dt.Columns.Add(new DataColumn("CompanyName", typeof(System.String)));
                dt.Columns.Add(new DataColumn("DestinationCountry", typeof(System.String)));
                dt.Columns.Add(new DataColumn("OrderCurrencyCode", typeof(System.Int32)));
                dt.Columns.Add(new DataColumn("DisplayName", typeof(System.String)));

                foreach (SellerProductDataViewModel item in list)
                {
                    DataRow dr = dt.NewRow();
                    dr["ProductID"] = item.ProductID;
                    dr["Qty"] = item.Qty;
                    dr["AdjustedSitePrice"] = item.AdjustedSitePrice;
                    dr["FinalShippingFee"] = item.FinalShippingFee;
                    dr["ShippingStatus"] = item.ShippingStatus;
                    dr["PaymentStatus"] = item.PaymentStatus;
                    dr["StatusCode"] = item.StatusCode;
                    dr["GrandTotal"] = item.GrandTotal;
                    dr["AverageCost"] = item.AverageCost;
                    dr["FinalValueFee"] = item.FinalValueFee;
                    dr["OrderID"] = item.OrderID;
                    dr["OrderSourceOrderID"] = item.OrderSourceOrderID;
                    dr["TimeOfOrder"] = item.TimeOfOrder;
                    dr["CompanyID"] = item.CompanyID;
                    dr["CompanyName"] = item.CompanyName;
                    dr["DestinationCountry"] = item.DestinationCountry;
                    dr["OrderCurrencyCode"] = item.OrderCurrencyCode;
                    dr["DisplayName"] = item.DisplayName;
                    dt.Rows.Add(dr);
                }

                MySqlConnection con = new MySqlConnection(connStr);
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                con.Open();
                MySqlCommand cmd = new MySqlCommand("P_saveProducrPrediction", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.UpdatedRowSource = UpdateRowSource.None;

                cmd.Parameters.Add("?_ProductID", MySqlDbType.String).SourceColumn = "ProductID";
                cmd.Parameters.Add("?_Qty", MySqlDbType.Int32).SourceColumn = "Qty";
                cmd.Parameters.Add("?_AdjustedSitePrice", MySqlDbType.Decimal).SourceColumn = "AdjustedSitePrice";
                cmd.Parameters.Add("?_FinalShippingFee", MySqlDbType.Decimal).SourceColumn = "FinalShippingFee";
                cmd.Parameters.Add("?_ShippingStatus", MySqlDbType.Int32).SourceColumn = "ShippingStatus";
                cmd.Parameters.Add("?_PaymentStatus", MySqlDbType.Int32).SourceColumn = "PaymentStatus";
                cmd.Parameters.Add("?_StatusCode", MySqlDbType.Int32).SourceColumn = "StatusCode";
                cmd.Parameters.Add("?_GrandTotal", MySqlDbType.Decimal).SourceColumn = "GrandTotal";
                cmd.Parameters.Add("?_AverageCost", MySqlDbType.Decimal).SourceColumn = "AverageCost";
                cmd.Parameters.Add("?_FinalValueFee", MySqlDbType.Decimal).SourceColumn = "FinalValueFee";
                cmd.Parameters.Add("?_OrderID", MySqlDbType.Int32).SourceColumn = "OrderID";

                cmd.Parameters.Add("?_TimeOfOrder", MySqlDbType.DateTime).SourceColumn = "TimeOfOrder";
                cmd.Parameters.Add("?_OrderSourceOrderID", MySqlDbType.String).SourceColumn = "OrderSourceOrderID";
                cmd.Parameters.Add("?_CompanyID", MySqlDbType.Int32).SourceColumn = "CompanyID";
                cmd.Parameters.Add("?_CompanyName", MySqlDbType.String).SourceColumn = "CompanyName";
                cmd.Parameters.Add("?_DestinationCountry", MySqlDbType.String).SourceColumn = "DestinationCountry";
                cmd.Parameters.Add("?_OrderCurrencyCode", MySqlDbType.Int32).SourceColumn = "OrderCurrencyCode";
                cmd.Parameters.Add("?_DisplayName", MySqlDbType.String).SourceColumn = "DisplayName";

                MySqlDataAdapter da = new MySqlDataAdapter();
                da.InsertCommand = cmd;
                da.UpdateBatchSize = 100;
                int records = da.Update(dt);
                con.Close();

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public void UpdateAllMarketPlacesOrdersFromSC(List<SellerProductDataViewModel> list)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("ProductID", typeof(System.String)));
                dt.Columns.Add(new DataColumn("Qty", typeof(System.Int32)));
                dt.Columns.Add(new DataColumn("AdjustedSitePrice", typeof(System.Decimal)));
                dt.Columns.Add(new DataColumn("FinalShippingFee", typeof(System.Decimal)));
                dt.Columns.Add(new DataColumn("ShippingStatus", typeof(System.Int32)));
                dt.Columns.Add(new DataColumn("PaymentStatus", typeof(System.Int32)));
                dt.Columns.Add(new DataColumn("StatusCode", typeof(System.Int32)));
                dt.Columns.Add(new DataColumn("GrandTotal", typeof(System.Decimal)));
                dt.Columns.Add(new DataColumn("AverageCost", typeof(System.Decimal)));
                dt.Columns.Add(new DataColumn("FinalValueFee", typeof(System.Decimal)));
                dt.Columns.Add(new DataColumn("OrderID", typeof(System.Int32)));
                dt.Columns.Add(new DataColumn("OrderSourceOrderID", typeof(System.String)));
                dt.Columns.Add(new DataColumn("TimeOfOrder", typeof(System.DateTime)));
                dt.Columns.Add(new DataColumn("CompanyID", typeof(System.Int32)));
                dt.Columns.Add(new DataColumn("CompanyName", typeof(System.String)));
                dt.Columns.Add(new DataColumn("DestinationCountry", typeof(System.String)));
                dt.Columns.Add(new DataColumn("OrderCurrencyCode", typeof(System.Int32)));
                dt.Columns.Add(new DataColumn("DisplayName", typeof(System.String)));

                foreach (SellerProductDataViewModel item in list)
                {
                    DataRow dr = dt.NewRow();
                    dr["ProductID"] = item.ProductID;
                    dr["Qty"] = item.Qty;
                    dr["AdjustedSitePrice"] = item.AdjustedSitePrice;
                    dr["FinalShippingFee"] = item.FinalShippingFee;
                    dr["ShippingStatus"] = item.ShippingStatus;
                    dr["PaymentStatus"] = item.PaymentStatus;
                    dr["StatusCode"] = item.StatusCode;
                    dr["GrandTotal"] = item.GrandTotal;
                    dr["AverageCost"] = item.AverageCost;
                    dr["FinalValueFee"] = item.FinalValueFee;
                    dr["OrderID"] = item.OrderID;
                    dr["OrderSourceOrderID"] = item.OrderSourceOrderID;
                    dr["TimeOfOrder"] = item.TimeOfOrder;
                    dr["CompanyID"] = item.CompanyID;
                    dr["CompanyName"] = item.CompanyName;
                    dr["DestinationCountry"] = item.DestinationCountry;
                    dr["OrderCurrencyCode"] = item.OrderCurrencyCode;
                    dr["DisplayName"] = item.DisplayName;
                    dt.Rows.Add(dr);
                }

                MySqlConnection con = new MySqlConnection(connStr);
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                con.Open();
                MySqlCommand cmd = new MySqlCommand("P_UpdateProducrPrediction", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.UpdatedRowSource = UpdateRowSource.None;

                cmd.Parameters.Add("?_ProductID", MySqlDbType.String).SourceColumn = "ProductID";
                cmd.Parameters.Add("?_Qty", MySqlDbType.Int32).SourceColumn = "Qty";
                cmd.Parameters.Add("?_AdjustedSitePrice", MySqlDbType.Decimal).SourceColumn = "AdjustedSitePrice";
                cmd.Parameters.Add("?_FinalShippingFee", MySqlDbType.Decimal).SourceColumn = "FinalShippingFee";
                cmd.Parameters.Add("?_ShippingStatus", MySqlDbType.Int32).SourceColumn = "ShippingStatus";
                cmd.Parameters.Add("?_PaymentStatus", MySqlDbType.Int32).SourceColumn = "PaymentStatus";
                cmd.Parameters.Add("?_StatusCode", MySqlDbType.Int32).SourceColumn = "StatusCode";
                cmd.Parameters.Add("?_GrandTotal", MySqlDbType.Decimal).SourceColumn = "GrandTotal";
                cmd.Parameters.Add("?_AverageCost", MySqlDbType.Decimal).SourceColumn = "AverageCost";
                cmd.Parameters.Add("?_FinalValueFee", MySqlDbType.Decimal).SourceColumn = "FinalValueFee";
                cmd.Parameters.Add("?_OrderID", MySqlDbType.Int32).SourceColumn = "OrderID";

                cmd.Parameters.Add("?_TimeOfOrder", MySqlDbType.DateTime).SourceColumn = "TimeOfOrder";
                cmd.Parameters.Add("?_OrderSourceOrderID", MySqlDbType.String).SourceColumn = "OrderSourceOrderID";
                cmd.Parameters.Add("?_CompanyID", MySqlDbType.Int32).SourceColumn = "CompanyID";
                cmd.Parameters.Add("?_CompanyName", MySqlDbType.String).SourceColumn = "CompanyName";
                cmd.Parameters.Add("?_DestinationCountry", MySqlDbType.String).SourceColumn = "DestinationCountry";
                cmd.Parameters.Add("?_OrderCurrencyCode", MySqlDbType.Int32).SourceColumn = "OrderCurrencyCode";
                cmd.Parameters.Add("?_DisplayName", MySqlDbType.String).SourceColumn = "DisplayName";

                MySqlDataAdapter da = new MySqlDataAdapter();
                da.InsertCommand = cmd;
                da.UpdateBatchSize = 100;
                int records = da.Update(dt);
                con.Close();

            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }

}

