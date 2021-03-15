using DataAccess.Helper;
using DataAccess.ViewModels;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataAccess.DataAccess
{
    public class HLDHistoryDataAccess
    {
        public string connStr { get; set; }
        DateTime CurrentDate;
        DateTime Yesterday;
        DateTime LastMonth;
        DateTime Week;
        DateTime LastThirtyDays;
        DateTime CurrentMonth;


        public HLDHistoryDataAccess(IConnectionString connectionString)
        {
            connStr = connectionString.GetConnectionString();
              CurrentDate = DateTimeExtensions.ConvertToEST(DateTime.Now);
              Yesterday = CurrentDate.AddDays(-1);
              LastMonth = CurrentDate.AddMonths(-1);
              Week = CurrentDate.AddDays(-6);
              LastThirtyDays = CurrentDate.AddDays(-30);
              CurrentMonth = new DateTime(CurrentDate.Year, CurrentDate.Month, 1);
        }

        public bool SaveOrder_SKU_ProfitHistory(Order_SKU_Profit_History model)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_SaveSku_OrderProfitHistory", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_OrderDate", model.InSellerCloud);
                    cmd.Parameters.AddWithValue("_MarketPlace", model.MarketPlace);
                    cmd.Parameters.AddWithValue("_SKU", model.OfferSku);
                    cmd.Parameters.AddWithValue("_SellerCloudOrderID", model.SellerCloudID);
                    cmd.Parameters.AddWithValue("_BBOrderID", model.BBOrderID);
                    cmd.Parameters.AddWithValue("_UnitsSold", model.TotalQuantity);
                    cmd.Parameters.AddWithValue("_GrossRevnue", model.calculation_TotalAmountOfUnitPrice);
                    cmd.Parameters.AddWithValue("_ShippintCostPaidByCustomer", 0);
                    cmd.Parameters.AddWithValue("_ItemAvgCost_CAD", model.AverageCost_CAD);
                    cmd.Parameters.AddWithValue("_ShippingAvgCost", 0);
                    cmd.Parameters.AddWithValue("_SellingFees", model.calculation_Comission);
                    cmd.Parameters.AddWithValue("_TaxesPercentage", model.calculation_TotalTacPercentage);
                    cmd.Parameters.AddWithValue("_SellingFeePercentage", model.calculation_comissionPercentage);
                    cmd.Parameters.AddWithValue("_Profit", model.calculation_ProfitLoss);
                    cmd.Parameters.AddWithValue("_ProfitPercentage", model.calculation_ProfitLossPercentage);
                    cmd.Parameters.AddWithValue("_Taxes", model.calculation_TotalTax);
                    cmd.Parameters.AddWithValue("_Currency_CAD_USA", "CAD");
                    cmd.Parameters.AddWithValue("_ExchangeRate", model.Exchange);
                    cmd.Parameters.AddWithValue("_ItemAvgCost_USD", model.ProductAvgCost_USD);
                    cmd.Parameters.AddWithValue("_CreatedDate", DateTimeExtensions.ConvertToEST(DateTime.Now));
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {

            }
            return true;
        }


        public List<SKUSalesHistoryFromOrders> GetSKU_OrderHistoryBy_SKU(String productSKU)
        {
            List<SKUSalesHistoryFromOrders> listSkuSlaesHistory = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                   // MySqlCommand cmd = new MySqlCommand("p_GetSkuSalesHistoryFromOrders", conn);
                    MySqlCommand cmd = new MySqlCommand("p_GetSCSkuSalesHistoryFromOrders", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("productSKU", productSKU);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            listSkuSlaesHistory = new List<SKUSalesHistoryFromOrders>();
                            while (reader.Read())
                            {
                                SKUSalesHistoryFromOrders model = new SKUSalesHistoryFromOrders();
                                model.SellerCloudID = Convert.ToString(reader["seller_cloud_order_id"] != DBNull.Value ? reader["seller_cloud_order_id"] : string.Empty);
                                model.MPID = Convert.ToString(reader["order_id"] != DBNull.Value ? reader["order_id"] : string.Empty);
                                model.InSellerCloud = Convert.ToDateTime(reader["last_update"] != DBNull.Value ? reader["last_update"] : DateTime.Now);
                                model.TotalQuantity = Convert.ToInt32(reader["quantity"] != DBNull.Value ? reader["quantity"] : 0);
                                model.TotalPrice = Convert.ToDecimal(reader["total_price"] != DBNull.Value ? reader["total_price"] : 0);
                                model.TotalComission = Convert.ToDecimal(reader["Commission_Fee"] != DBNull.Value ? reader["Commission_Fee"] : 0);
                                model.TaxGST = Convert.ToDecimal(reader["TaxGST"] != DBNull.Value ? reader["TaxGST"] : 0);
                                model.TaxPst = Convert.ToDecimal(reader["TaxPST"] != DBNull.Value ? reader["TaxPST"] : 0);
                                model.AverageCost = Convert.ToDecimal(reader["avg_cost"] != DBNull.Value ? reader["avg_cost"] : 0);
                                model.RowID = Convert.ToInt32(reader["TotalRecords"] != DBNull.Value ? reader["TotalRecords"] : 0);
                                model.ProductAvgCost = Convert.ToDecimal(reader["avg_cost"] != DBNull.Value ? reader["avg_cost"] : 0);
                                model.ShippingFee = Convert.ToDecimal(reader["ShippingFee"] != DBNull.Value ? reader["ShippingFee"] : 0);


                                model.UnitPrice = Math.Round(model.TotalPrice / model.TotalQuantity, 2);

                                model.calculation_TotalAmountOfUnitPrice = model.TotalQuantity * model.UnitPrice;
                                model.calculation_TotalAvgCost = model.TotalQuantity * model.ProductAvgCost; 
                                model.calculation_TotalTax = model.TaxGST + model.TaxPst;
                                model.calculation_TotalTacPercentage = Math.Round((model.calculation_TotalTax / model.calculation_TotalAmountOfUnitPrice) * 100, 2);
                                model.calculation_Comission = Math.Round((model.TotalComission), 2);
                                model.caculation_TotalAvgCost = Math.Round(Math.Round(Convert.ToDecimal(model.AverageCost), 2) * model.TotalQuantity, 2);
                                model.calculation_SumTotal = Math.Round(model.calculation_TotalTax + model.calculation_TotalAmountOfUnitPrice, 2);
                                model.calculation_comissionPercentage = Math.Round(((model.calculation_Comission / model.calculation_TotalAmountOfUnitPrice) * 100), 2);
                                model.calculation_ProfitLoss = Math.Round((model.calculation_TotalAmountOfUnitPrice+model.ShippingFee) - model.caculation_TotalAvgCost - model.calculation_Comission, 2);
                                model.calculation_ProfitLossPercentage = Math.Round((model.calculation_ProfitLoss / model.calculation_TotalAmountOfUnitPrice) * 100, 2);

                                listSkuSlaesHistory.Add(model);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return listSkuSlaesHistory;
        }


        public List<SC_BB_OrderIDsViewModel> GetSCOrderID_SKU_Profit_Calculation_History()
        {
            List<SC_BB_OrderIDsViewModel> list = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetSCOrderID_SKU_Profit_Calculation_History", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            list = new List<SC_BB_OrderIDsViewModel>();

                            while (reader.Read())
                            {
                                SC_BB_OrderIDsViewModel model = new SC_BB_OrderIDsViewModel();
                                model.BBOrderID = Convert.ToString(reader["order_id"] != DBNull.Value ? reader["order_id"] : 0);
                                model.SCOrderID = Convert.ToString(reader["sellerCloudID"] != DBNull.Value ? reader["sellerCloudID"] : 0);
                                list.Add(model);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return list;
        }


        public List<Order_SKU_Profit_History> GetSCOrderDetail_SKU_Profit_Calculation_History(SC_BB_OrderIDsViewModel SCBBmodel)
        {
            List<Order_SKU_Profit_History> list = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetOrderDetail_Order_sku_ProfitHistory", conn);
                    cmd.Parameters.AddWithValue("_OrderID", SCBBmodel.BBOrderID);
                    cmd.Parameters.AddWithValue("_SCOrderID", SCBBmodel.SCOrderID);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            list = new List<Order_SKU_Profit_History>();
                            while (reader.Read())
                            {
                                Order_SKU_Profit_History model = new Order_SKU_Profit_History();
                                model.SellerCloudID = Convert.ToString(reader["sellerCloudID"] != DBNull.Value ? reader["sellerCloudID"] : string.Empty);
                                model.BBOrderID = Convert.ToString(reader["order_id"] != DBNull.Value ? reader["order_id"] : string.Empty);
                                model.InSellerCloud = Convert.ToDateTime(reader["SCDate"] != DBNull.Value ? reader["SCDate"] : DateTime.Now);
                                model.TotalQuantity = Convert.ToInt32(reader["quantity"] != DBNull.Value ? reader["quantity"] : 0);
                                model.TotalPrice = Convert.ToDecimal(reader["total_price"] != DBNull.Value ? reader["total_price"] : 0);
                                model.TotalComission = Convert.ToDecimal(reader["total_commission"] != DBNull.Value ? reader["total_commission"] : 0);
                                model.TaxGST = Convert.ToDecimal(reader["TaxGST"] != DBNull.Value ? reader["TaxGST"] : 0);
                                model.TaxPst = Convert.ToDecimal(reader["TaxPST"] != DBNull.Value ? reader["TaxPST"] : 0);
                                model.AverageCost_CAD = Convert.ToDecimal(reader["avg_cost_CAD"] != DBNull.Value ? reader["avg_cost_CAD"] : 0);
                                model.Exchange = Convert.ToDecimal(reader["dollarRate"] != DBNull.Value ? reader["dollarRate"] : 0);
                                model.ProductAvgCost_USD = Convert.ToDecimal(reader["avg_cost_USD"] != DBNull.Value ? reader["avg_cost_USD"] : 0);
                                model.OfferSku = Convert.ToString(reader["offer_sku"] != DBNull.Value ? reader["offer_sku"] : "");
                                model.UnitPrice = Math.Round(model.TotalPrice / model.TotalQuantity, 2);

                                model.calculation_TotalAmountOfUnitPrice = model.TotalQuantity * model.UnitPrice;
                                model.calculation_TotalTax = model.TaxGST + model.TaxPst;
                                model.calculation_TotalTacPercentage = Math.Round((model.calculation_TotalTax / model.calculation_TotalAmountOfUnitPrice) * 100, 2);
                                model.calculation_Comission = Math.Round((model.TotalComission) / (1 + model.calculation_TotalTacPercentage / 100), 2);
                                model.caculation_TotalAvgCost = Math.Round(Math.Round(Convert.ToDecimal(model.AverageCost_CAD), 2) * model.TotalQuantity, 2);
                                model.calculation_SumTotal = Math.Round(model.calculation_TotalTax + model.calculation_TotalAmountOfUnitPrice, 2);
                                model.calculation_comissionPercentage = Math.Round(((model.calculation_Comission / model.calculation_TotalAmountOfUnitPrice) * 100), 2);
                                model.calculation_ProfitLoss = Math.Round(model.calculation_TotalAmountOfUnitPrice - 0 - model.caculation_TotalAvgCost - model.calculation_Comission, 2);
                                model.calculation_ProfitLossPercentage = Math.Round((model.calculation_ProfitLoss / model.calculation_TotalAmountOfUnitPrice) * 100, 2);
                                list.Add(model);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return list;
        }

        public List<Order_SKU_ProfitHistory_CalculationViewmodel> GetProfitHistoryDetailByDate(string sku)
        {
            List<Order_SKU_ProfitHistory_CalculationViewmodel> Listmodel = new List<Order_SKU_ProfitHistory_CalculationViewmodel>();


            var month = new DateTime(CurrentDate.Year, CurrentDate.Month, 1);
            var first = month.AddMonths(-1);
            var last = month.AddDays(-1);
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    Listmodel.Add(PopulateListOfCalculation(conn, sku, CurrentDate, CurrentDate, "Today"));
                    Listmodel.Add(PopulateListOfCalculation(conn, sku, Yesterday, Yesterday, "Yesterday"));
                    Listmodel.Add(PopulateListOfCalculation(conn, sku, CurrentDate, Week, "Last 7 Days"));
                    Listmodel.Add(PopulateListOfCalculation(conn, sku, CurrentDate, LastThirtyDays, "Last 30 Days"));
                    Listmodel.Add(PopulateListOfCalculation(conn, sku, CurrentDate, CurrentMonth, "This Month"));
                    Listmodel.Add(PopulateListOfCalculation(conn, sku, last, first, "Last Month"));
                }
            }
            catch (Exception ex)
            {
            }
            return Listmodel;
        }

        private Order_SKU_ProfitHistory_CalculationViewmodel PopulateListOfCalculation(MySqlConnection conn, string sku, DateTime dateFrom, DateTime dateTo, string Duration)
        {
            Order_SKU_ProfitHistory_CalculationViewmodel model = new Order_SKU_ProfitHistory_CalculationViewmodel();
            MySqlCommand cmd = new MySqlCommand("p_GetOrder_SKU_ProfitHistory_BYSKU", conn);
            cmd.Parameters.AddWithValue("_sku", sku);
            cmd.Parameters.AddWithValue("_dateFrom", dateFrom);
            cmd.Parameters.AddWithValue("_dateTo", dateTo);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            using (var reader = cmd.ExecuteReader())
            {
                model.Duration = Duration;
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        model.GrossRevnue = Convert.ToDecimal(reader["GrossRevnue"] != DBNull.Value ? reader["GrossRevnue"] : 0);
                        model.ItemCost = Convert.ToDecimal(reader["ItemAvgCost_CAD"] != DBNull.Value ? reader["ItemAvgCost_CAD"] : 0);
                        model.OrderQuantity = Convert.ToInt32(reader["TotalOrders"] != DBNull.Value ? reader["TotalOrders"] : 0);
                        model.Profit = Convert.ToDecimal(reader["Profit"] != DBNull.Value ? reader["Profit"] : 0);
                        model.ProfitPercentage = Convert.ToDecimal(reader["ProfitPercentage"] != DBNull.Value ? reader["ProfitPercentage"] : 0);
                        model.SellingFee = Convert.ToDecimal(reader["SellingFee"] != DBNull.Value ? reader["SellingFee"] : 0);
                        model.SellingFeePercentage = Convert.ToDecimal(reader["SellingFeePercentage"] != DBNull.Value ? reader["SellingFeePercentage"] : 0);
                        model.SKU = Convert.ToString(reader["sku"] != DBNull.Value ? reader["sku"] : 0);
                        model.TaxesPercentage = Convert.ToDecimal(reader["TaxPercentage"] != DBNull.Value ? reader["TaxPercentage"] : 0);
                        model.Taxes = Convert.ToDecimal(reader["Taxes"] != DBNull.Value ? reader["Taxes"] : 0);
                        model.UnitsSold = Convert.ToInt32(reader["UnitsSold"] != DBNull.Value ? reader["UnitsSold"] : 0);

                    }
                }
            }
            return model;
        }


        private Order_SKU_ProfitHistory_CalculationViewmodel SaleProfitHistoyByDate(MySqlConnection conn, DateTime date, string Duration)
        {
            Order_SKU_ProfitHistory_CalculationViewmodel model = new Order_SKU_ProfitHistory_CalculationViewmodel();
            MySqlCommand cmd = new MySqlCommand("P_GetProfitHistory_Datewise", conn);
            cmd.Parameters.AddWithValue("_OrderDate", date);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            using (var reader = cmd.ExecuteReader())
            {
                model.Duration = Duration;
                model.DateTime = date;
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        model.GrossRevnue = Convert.ToDecimal(reader["GrossRevnue"] != DBNull.Value ? reader["GrossRevnue"] : 0);
                        model.ItemCost = Convert.ToDecimal(reader["ItemAvgCost_CAD"] != DBNull.Value ? reader["ItemAvgCost_CAD"] : 0);
                        model.OrderQuantity = Convert.ToInt32(reader["TotalOrders"] != DBNull.Value ? reader["TotalOrders"] : 0);
                        model.Profit = Convert.ToDecimal(reader["Profit"] != DBNull.Value ? reader["Profit"] : 0);
                        model.ProfitPercentage = Convert.ToDecimal(reader["ProfitPercentage"] != DBNull.Value ? reader["ProfitPercentage"] : 0);
                        model.SellingFee = Convert.ToDecimal(reader["SellingFee"] != DBNull.Value ? reader["SellingFee"] : 0);
                        model.SellingFeePercentage = Convert.ToDecimal(reader["SellingFeePercentage"] != DBNull.Value ? reader["SellingFeePercentage"] : 0);
                        model.TaxesPercentage = Convert.ToDecimal(reader["TaxPercentage"] != DBNull.Value ? reader["TaxPercentage"] : 0);
                        model.Taxes = Convert.ToDecimal(reader["Taxes"] != DBNull.Value ? reader["Taxes"] : 0);
                        model.UnitsSold = Convert.ToInt32(reader["UnitsSold"] != DBNull.Value ? reader["UnitsSold"] : 0);

                    }
                }
            }
            return model;
        }
        private Order_SKU_ProfitHistory_CalculationViewmodel SaleProfitHistoyByDateRange(MySqlConnection conn, DateTime dateFrom, DateTime dateTo, string Duration)
        {
            Order_SKU_ProfitHistory_CalculationViewmodel model = new Order_SKU_ProfitHistory_CalculationViewmodel();
            MySqlCommand cmd = new MySqlCommand("p_GetProfitHistory_DateRange", conn);
            cmd.Parameters.AddWithValue("_dateFrom", dateFrom);
            cmd.Parameters.AddWithValue("_dateTo", dateTo);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            using (var reader = cmd.ExecuteReader())
            {
                model.Duration = Duration;
                model.DateTime = dateFrom;
                model.StartDate = CurrentDate;
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        model.GrossRevnue = Convert.ToDecimal(reader["GrossRevnue"] != DBNull.Value ? reader["GrossRevnue"] : 0);
                        model.ItemCost = Convert.ToDecimal(reader["ItemAvgCost_CAD"] != DBNull.Value ? reader["ItemAvgCost_CAD"] : 0);
                        model.OrderQuantity = Convert.ToInt32(reader["TotalOrders"] != DBNull.Value ? reader["TotalOrders"] : 0);
                        model.Profit = Convert.ToDecimal(reader["Profit"] != DBNull.Value ? reader["Profit"] : 0);
                        model.ProfitPercentage = Convert.ToDecimal(reader["ProfitPercentage"] != DBNull.Value ? reader["ProfitPercentage"] : 0);
                        model.SellingFee = Convert.ToDecimal(reader["SellingFee"] != DBNull.Value ? reader["SellingFee"] : 0);
                        model.SellingFeePercentage = Convert.ToDecimal(reader["SellingFeePercentage"] != DBNull.Value ? reader["SellingFeePercentage"] : 0);
                        model.TaxesPercentage = Convert.ToDecimal(reader["TaxPercentage"] != DBNull.Value ? reader["TaxPercentage"] : 0);
                        model.Taxes = Convert.ToDecimal(reader["Taxes"] != DBNull.Value ? reader["Taxes"] : 0);
                        model.UnitsSold = Convert.ToInt32(reader["UnitsSold"] != DBNull.Value ? reader["UnitsSold"] : 0);

                    }
                }
            }
            return model;
        }

        public List<Order_SKU_ProfitHistory_CalculationViewmodel> GetSalesProfitHistoryDashBoard()
        {
            List<Order_SKU_ProfitHistory_CalculationViewmodel> listModel = new List<Order_SKU_ProfitHistory_CalculationViewmodel>();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connStr))
                {
                    connection.Open();
                    listModel.Add(SaleProfitHistoyByDate(connection, CurrentDate, "Today"));
                    listModel.Add(SaleProfitHistoyByDate(connection, Yesterday, "Yesterday"));
                    listModel.Add(SaleProfitHistoyByDateRange(connection, Week, CurrentDate, "7 Days"));
                }
            }
            catch (Exception ex)
            {                
            }
            return listModel;
        }

        public  Order_SKU_ProfitHistory_CalculationViewmodel  GetSalesProfitHistoryDashBoard(DateTime dateFrom,DateTime dateTo)
        {
             Order_SKU_ProfitHistory_CalculationViewmodel  model = new  Order_SKU_ProfitHistory_CalculationViewmodel();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connStr))
                {
                    connection.Open();

                    model= SaleProfitHistoyByDateRange(connection, dateFrom, dateTo, "Custom");
                }
            }
            catch (Exception ex)
            {
            }
            return model;
        }
    }
}



// GET SKU PROFIT  p_GetOrder_SKU_ProfitHistory_BYSKU
