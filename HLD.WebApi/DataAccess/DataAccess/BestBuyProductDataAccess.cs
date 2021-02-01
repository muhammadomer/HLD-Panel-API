using DataAccess.Helper;
using DataAccess.ViewModels;
using Hld.WebApplication.ViewModel;
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
    public class BestBuyProductDataAccess
    {
        public string connStr { get; set; }
        ProductWarehouseQtyDataAccess dataAccess = null;
        public BestBuyProductDataAccess(IConnectionString connectionString)
        {
            connStr = connectionString.GetConnectionString();
            dataAccess = new ProductWarehouseQtyDataAccess(connectionString);
        }

        public bool SaveBBProduct(BBProductViewModel ViewModel)
        {
            int BBProductId = 0;
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_SaveBestBuyProduct", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("BB_Product_Id", MySqlDbType.Int32, 10);
                    cmd.Parameters["BB_Product_Id"].Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("discount_end_date", ViewModel.DiscountEndDate);
                    cmd.Parameters.AddWithValue("discount_start_date", ViewModel.DiscountStartDate);
                    cmd.Parameters.AddWithValue("unit_discount_price_selling_price", ViewModel.UnitDiscountPrice_SellingPrice);
                    cmd.Parameters.AddWithValue("unit_origin_price_MSRP", ViewModel.UnitOriginPrice_MSRP);
                    cmd.Parameters.AddWithValue("category_code", ViewModel.CategoryCode);
                    cmd.Parameters.AddWithValue("category_name", ViewModel.CategoryName);
                    cmd.Parameters.AddWithValue("logistic_class_code", ViewModel.LogisticClass_Code);
                    cmd.Parameters.AddWithValue("reference_UPC", ViewModel.Reference_UPC);
                    cmd.Parameters.AddWithValue("product_sku", ViewModel.Product_Sku);
                    cmd.Parameters.AddWithValue("product_title", ViewModel.Product_Title);
                    cmd.Parameters.AddWithValue("quantity_BBQ", ViewModel.Quantity_BBQ);
                    cmd.Parameters.AddWithValue("shop_sku_offer_sku", ViewModel.ShopSKU_OfferSKU);
                    cmd.ExecuteNonQuery();
                    BBProductId = Convert.ToInt32(cmd.Parameters["BB_Product_Id"].Value);

                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public List<BBProductViewModel> GetAllBestBuyProducts()
        {
            List<BBProductViewModel> listBBProductViewModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetAllBestBuyProducts", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            listBBProductViewModel = new List<BBProductViewModel>();
                            while (reader.Read())
                            {
                                BBProductViewModel ViewModel = new BBProductViewModel();
                                ViewModel.BBProductID = Convert.ToInt32(reader["BB_Product_Id"]);
                                ViewModel.CategoryCode = Convert.ToString(reader["category_code"]);
                                ViewModel.CategoryName = Convert.ToString(reader["category_name"]);
                                ViewModel.DiscountEndDate = Convert.ToDateTime(reader["discount_end_date"]);
                                ViewModel.DiscountStartDate = Convert.ToDateTime(reader["discount_start_date"]);
                                ViewModel.LogisticClass_Code = Convert.ToString(reader["logistic_class_code"]);
                                ViewModel.Product_Sku = Convert.ToString(reader["product_sku"]);
                                ViewModel.Product_Title = Convert.ToString(reader["product_title"]);
                                ViewModel.Quantity_BBQ = Convert.ToInt32(reader["quantity_BBQ"]);
                                ViewModel.Reference_UPC = Convert.ToString(reader["reference_UPC"]);
                                ViewModel.ShopSKU_OfferSKU = Convert.ToString(reader["shop_sku_offer_sku"]);
                                ViewModel.UnitDiscountPrice_SellingPrice = Convert.ToDouble(reader["unit_discount_price_selling_price"]);
                                ViewModel.UnitOriginPrice_MSRP = Convert.ToDouble(reader["unit_origin_price_MSRP"]);
                                listBBProductViewModel.Add(ViewModel);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return listBBProductViewModel;
        }


        public BBProductViewModel GetBestBuyProductByProductID(string ProductId)
        {
            BBProductViewModel ViewModel = new BBProductViewModel();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {//mychange sp name
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetBestBuyProductDetailByProductIdCopy", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("productID", ProductId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                ViewModel.BBProductID = Convert.ToInt32(reader["BestBuyProductID"] != DBNull.Value ? reader["BestBuyProductID"] : "0");
                                ViewModel.CategoryCode = Convert.ToString(reader["category_code"] != DBNull.Value ? reader["category_code"] : "");
                                ViewModel.CategoryName = Convert.ToString(reader["category_name"] != DBNull.Value ? reader["category_name"] : "");
                                ViewModel.DiscountEndDate = Convert.ToDateTime(reader["discount_end_date"] != DBNull.Value ? reader["discount_end_date"] : (DateTime?)null);
                                ViewModel.DiscountStartDate = Convert.ToDateTime(reader["discount_start_date"] != DBNull.Value ? reader["discount_start_date"] : (DateTime?)null);
                                ViewModel.LogisticClass_Code = Convert.ToString(reader["logistic_class_code"] != DBNull.Value ? reader["logistic_class_code"] : "");
                                ViewModel.Product_Sku = Convert.ToString(reader["product_sku"] != DBNull.Value ? reader["product_sku"] : "");
                                ViewModel.Product_Title = Convert.ToString(reader["product_title"] != DBNull.Value ? reader["product_title"] : "");
                                ViewModel.Quantity_BBQ = Convert.ToInt32(reader["quantity_BBQ"] != DBNull.Value ? reader["quantity_BBQ"] : "0");
                                ViewModel.Reference_UPC = Convert.ToString(reader["reference_UPC"] != DBNull.Value ? reader["reference_UPC"] : "");
                                ViewModel.ShopSKU_OfferSKU = Convert.ToString(reader["shop_sku_offer_sku"] != DBNull.Value ? reader["shop_sku_offer_sku"] : "");
                                ViewModel.UnitDiscountPrice_SellingPrice = Convert.ToDouble(reader["unit_discount_price_selling_price"] != DBNull.Value ? reader["unit_discount_price_selling_price"] : "0");
                                ViewModel.UnitOriginPrice_MSRP = Convert.ToDouble(reader["unit_origin_price_MSRP"] != DBNull.Value ? reader["unit_origin_price_MSRP"] : "0");
                                ViewModel.ProductId = Convert.ToInt32(reader["product_id"] != DBNull.Value ? reader["product_id"] : "0");
                                ViewModel.BBCategory = Convert.ToString(reader["BestBuyCategory"] != DBNull.Value ? reader["BestBuyCategory"] : "");
                                ViewModel.dropship_Qty = Convert.ToInt32(reader["dropship_Qty"] != DBNull.Value ? reader["dropship_Qty"] : "0");
                                ViewModel.dropship_status = Convert.ToBoolean(reader["dropship_status"] != DBNull.Value ? reader["dropship_status"] : "0");
                                //ViewModel.BBQtyUpdate = Convert.ToBoolean(reader["BBQtyUpdate"] != DBNull.Value ? reader["BBQtyUpdate"] : "0");
                                ViewModel.BBQtyUpdate = Convert.ToBoolean(reader["BBQtyUpdate"] != DBNull.Value ? reader["BBQtyUpdate"] : false);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return ViewModel;
        }

        public BBProductViewModel GetBestBuyProductByBBSKU(string ProductId)
        {
            BBProductViewModel ViewModel = new BBProductViewModel();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetBestBuyProductDetailByBBSKU", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("productID", Convert.ToInt32(ProductId));
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                ViewModel.BBProductID = Convert.ToInt32(reader["BestBuyProductID"] != DBNull.Value ? reader["BestBuyProductID"] : "0");
                                ViewModel.CategoryCode = Convert.ToString(reader["category_code"] != DBNull.Value ? reader["category_code"] : "");
                                ViewModel.CategoryName = Convert.ToString(reader["category_name"] != DBNull.Value ? reader["category_name"] : "");
                                ViewModel.DiscountEndDate = Convert.ToDateTime(reader["discount_end_date"] != DBNull.Value ? reader["discount_end_date"] : (DateTime?)null);
                                ViewModel.DiscountStartDate = Convert.ToDateTime(reader["discount_start_date"] != DBNull.Value ? reader["discount_start_date"] : (DateTime?)null);
                                ViewModel.LogisticClass_Code = Convert.ToString(reader["logistic_class_code"] != DBNull.Value ? reader["logistic_class_code"] : "");
                                ViewModel.Product_Sku = Convert.ToString(reader["product_sku"] != DBNull.Value ? reader["product_sku"] : "");
                                ViewModel.Product_Title = Convert.ToString(reader["product_title"] != DBNull.Value ? reader["product_title"] : "");
                                ViewModel.Quantity_BBQ = Convert.ToInt32(reader["quantity_BBQ"] != DBNull.Value ? reader["quantity_BBQ"] : "0");
                                ViewModel.Reference_UPC = Convert.ToString(reader["reference_UPC"] != DBNull.Value ? reader["reference_UPC"] : "");
                                ViewModel.ShopSKU_OfferSKU = Convert.ToString(reader["shop_sku_offer_sku"] != DBNull.Value ? reader["shop_sku_offer_sku"] : "");
                                ViewModel.UnitDiscountPrice_SellingPrice = Convert.ToDouble(reader["unit_discount_price_selling_price"] != DBNull.Value ? reader["unit_discount_price_selling_price"] : "0");
                                ViewModel.UnitOriginPrice_MSRP = Convert.ToDouble(reader["unit_origin_price_MSRP"] != DBNull.Value ? reader["unit_origin_price_MSRP"] : "0");
                                ViewModel.ProductId = Convert.ToInt32(reader["product_id"] != DBNull.Value ? reader["product_id"] : "0");
                                ViewModel.BBCategory = Convert.ToString(reader["BestBuyCategory"] != DBNull.Value ? reader["BestBuyCategory"] : "");
                                ViewModel.dropship_Qty = Convert.ToInt32(reader["dropship_Qty"] != DBNull.Value ? reader["dropship_Qty"] : "0");
                                ViewModel.dropship_status = Convert.ToBoolean(reader["dropship_status"] != DBNull.Value ? reader["dropship_status"] : "0");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return ViewModel;
        }

        public List<BestBuyOrderDetailViewModel> GetAllBestBuyOrdersDetail(string OrderId, string connStr)
        {
            List<BestBuyOrderDetailViewModel> listBBProductViewModel = new List<BestBuyOrderDetailViewModel>();
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {

                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetBestBuyOrdersDetail", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("OrderId", OrderId);
                    using (var reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                    {
                        if (reader.HasRows)
                        {

                            while (reader.Read())
                            {
                                BestBuyOrderDetailViewModel ViewModel = new BestBuyOrderDetailViewModel();
                                ViewModel.Location = "";
                                ViewModel.OrderStatus = Convert.ToString(reader["order_line_state"] != DBNull.Value ? reader["order_line_state"] : string.Empty);
                                ViewModel.ProductSKU = Convert.ToString(reader["sku"] != DBNull.Value ? reader["sku"] : string.Empty);
                                ViewModel.Prime = Convert.ToString("Y");
                                if (reader["image_name"] != DBNull.Value)
                                {
                                    ViewModel.ImageUrl = Convert.ToString(reader["image_name"]);
                                }
                                else
                                {
                                    ViewModel.ImageUrl = "";
                                }
                                ViewModel.ProductTitle = Convert.ToString(reader["title"] != DBNull.Value ? reader["title"] : string.Empty);
                                ViewModel.TotalQuantity = Convert.ToInt32(reader["quantity"] != DBNull.Value ? reader["quantity"] : "0");
                                ViewModel.WarehouseQuantity = Convert.ToString("10");
                                ViewModel.ZincASIN = Convert.ToString("");
                                ViewModel.ZincLink = Convert.ToString("");
                                ViewModel.ZincStatus = Convert.ToString("");
                                ViewModel.ProfitLoss = "";
                                ViewModel.Comission = Convert.ToDecimal(reader["total_commission"] != DBNull.Value ? reader["total_commission"] : "0");
                                ViewModel.UnitPrice = Convert.ToDecimal(reader["total_price"] != DBNull.Value ? reader["total_price"] : "0");
                                ViewModel.OrderDetailID = Convert.ToInt32(reader["bbe2_line_id"] != DBNull.Value ? reader["bbe2_line_id"] : "0");
                                ViewModel.TaxGST = Convert.ToDecimal(reader["TaxGST"] != DBNull.Value ? reader["TaxGST"] : "0");
                                ViewModel.TaxPST = Convert.ToDecimal(reader["TaxPST"] != DBNull.Value ? reader["TaxPST"] : "0");
                                listBBProductViewModel.Add(ViewModel);
                            }
                        }
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {
                    conn.Close();
                }

            }
            return listBBProductViewModel;
        }

        public List<BestBuyOrdersViewModel> GetAllBestBuyOrders(int startLimit, int endLimit, string sort)
        {
            MySqlConnection mySqlConnection = null;
            List<BestBuyOrdersViewModel> listBBProductViewModel = null;
            try
            {
                System.Data.DataSet ds = new System.Data.DataSet();
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {

                    MySqlCommand cmd = new MySqlCommand("p_GetBestBuyOrdersCopy", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("startLimit", startLimit);
                    cmd.Parameters.AddWithValue("endLimit", endLimit);
                    cmd.Parameters.AddWithValue("sort", sort);


                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(ds);
                    listBBProductViewModel = new List<BestBuyOrdersViewModel>();
                    System.Data.DataView dataView = new System.Data.DataView(ds.Tables[0]);
                    System.Data.DataTable distinctValue = dataView.ToTable(true, "sellerCloudID");
                    DataTable dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        mySqlConnection = new MySqlConnection(connStr);
                        mySqlConnection.Open();
                    }
                    foreach (System.Data.DataRow reader in distinctValue.Rows)
                    {
                        List<BestBuyOrderDetailViewModel> objList = new List<BestBuyOrderDetailViewModel>();

                        var list = dt.AsEnumerable().Where(e => e.Field<string>("sellerCloudID") == reader["sellerCloudID"].ToString()).ToList();

                        var totalQuantity = list.Sum(e => Convert.ToDouble(e.Field<string>("quantity")));
                        var totalPrice = list.Sum(e => Convert.ToDouble(e.Field<string>("total_price")));
                        var total_commission = list.Sum(e => Convert.ToDouble(e.Field<string>("total_commission")));
                        var totalGst = list.Sum(e => Convert.ToDouble(e.Field<string>("TaxGST")));
                        var totalPst = list.Sum(e => Convert.ToDouble(e.Field<string>("TaxPST")));
                        var toalAverageCost = list.Sum(e => e.Field<double>("TotalAverageCost"));


                        var sumOfGstPst = totalGst + totalPst;

                        var total = (sumOfGstPst / totalPrice) * 100;

                        //var totalComission = Math.Round((total_commission) / (1 + total / 100), 2);
                        var totalComission = total_commission;
                        var ProfitLoss = Math.Round(totalPrice - toalAverageCost - total_commission, 2);
                        var profitAndLossInPercent = Math.Round((ProfitLoss / totalPrice) * 100);


                        BestBuyOrdersViewModel ViewModel = new BestBuyOrdersViewModel();
                        ViewModel.OrderNumber = Convert.ToString(list.Select(e => e.Field<string>("order_id")).FirstOrDefault());
                        ViewModel.SellerCloudOrderID = Convert.ToString(list.Select(e => e.Field<string>("sellerCloudID")).FirstOrDefault());
                        ViewModel.ParentOrderID = Convert.ToInt32(list.Select(e => e.Field<int?>("ParentOrderID")).FirstOrDefault() != null ? list.Select(e => e.Field<int>("ParentOrderID")).FirstOrDefault() : 0).ToString();
                        ViewModel.IsParent = Convert.ToString(list.Select(e => e.Field<string>("IsParent")).FirstOrDefault());
                        ViewModel.IsNotes = Convert.ToString(list.Select(e => e.Field<string>("IsNotes")).FirstOrDefault());
                        ViewModel.ShippingPrice = Convert.ToString(list.Select(e => e.Field<string>("ShippingPaidByCustomer")).FirstOrDefault());

                        ViewModel.ShipmentAddress = Convert.ToString(list.Select(e => e.Field<string>("CustomerAddress")).FirstOrDefault());
                        ViewModel.OrderDate = Convert.ToDateTime(list.Select(e => e.Field<DateTime>("acceptance_decision_date")).FirstOrDefault());

                        ViewModel.TotalTax = Convert.ToDecimal(0);
                        ViewModel.TotalQuantity = Convert.ToInt32(totalQuantity);
                        ViewModel.TotalComission = Convert.ToDecimal(total_commission);
                        ViewModel.TotalPrice = Convert.ToDecimal(totalPrice);
                        ViewModel.ProfitAndLossInDollar = Convert.ToDecimal(ProfitLoss);
                        ViewModel.ProfitAndLossInPercentage = Convert.ToDecimal(profitAndLossInPercent);
                        ViewModel.TotalAverageCost = Convert.ToDecimal(toalAverageCost);
                        foreach (DataRow dataRow in list)
                        {

                            BestBuyOrderDetailViewModel detailViewModel = new BestBuyOrderDetailViewModel();
                            detailViewModel.Location = "";
                            detailViewModel.OrderStatus = Convert.ToString(dataRow["order_line_state"] != DBNull.Value ? dataRow["order_line_state"] : string.Empty);
                            detailViewModel.ProductSKU = Convert.ToString(dataRow["sku"] != DBNull.Value ? dataRow["sku"] : string.Empty);
                            detailViewModel.Prime = Convert.ToString("Y");
                            if (dataRow["image_name"] != DBNull.Value)
                            {
                                detailViewModel.ImageUrl = Convert.ToString(dataRow["image_name"]);
                            }
                            else
                            {
                                detailViewModel.ImageUrl = "";
                            }

                            if (dataRow["CreditCardId"] != DBNull.Value)
                            {
                                ViewModel.CreditCardId = Convert.ToInt32(dataRow["CreditCardId"] != DBNull.Value ? dataRow["CreditCardId"] : 0);
                            }
                            if (dataRow["ZincAccountId"] != DBNull.Value)
                            {
                                ViewModel.ZincAccountId = Convert.ToInt32(dataRow["ZincAccountId"] != DBNull.Value ? dataRow["ZincAccountId"] : 0);
                            }

                            detailViewModel.ProductTitle = Convert.ToString(dataRow["title"] != DBNull.Value ? dataRow["title"] : string.Empty);
                            detailViewModel.TotalQuantity = Convert.ToInt32(dataRow["quantity"] != DBNull.Value ? dataRow["quantity"] : "0");
                            detailViewModel.AverageCost = Convert.ToString(dataRow["avg_cost"] != DBNull.Value ? dataRow["avg_cost"] : "0");
                            detailViewModel.WarehouseQuantity = Convert.ToString("10");
                            detailViewModel.ShippingFee = Convert.ToDecimal(dataRow["ShippingFee"] != DBNull.Value ? dataRow["ShippingFee"] : 0);
                            detailViewModel.ZincASIN = Convert.ToString("");
                            detailViewModel.ZincLink = Convert.ToString("");
                            detailViewModel.ZincStatus = Convert.ToString(dataRow["drop_ship_status"] != DBNull.Value ? dataRow["drop_ship_status"] : "");
                            detailViewModel.ProfitLoss = "";
                            detailViewModel.Comission = Convert.ToDecimal(dataRow["total_commission"] != DBNull.Value ? dataRow["total_commission"] : "0");
                            detailViewModel.UnitPrice = Convert.ToDecimal(dataRow["total_price"] != DBNull.Value ? dataRow["total_price"] : "0");
                            detailViewModel.UnitPrice = Math.Round((detailViewModel.UnitPrice - detailViewModel.ShippingFee) / detailViewModel.TotalQuantity, 2);
                            detailViewModel.OrderDetailID = Convert.ToInt32(dataRow["bbe2_line_id"] != DBNull.Value ? dataRow["bbe2_line_id"] : "0");
                            detailViewModel.TaxGST = Convert.ToDecimal(dataRow["TaxGST"] != DBNull.Value ? dataRow["TaxGST"] : "0");
                            detailViewModel.TaxPST = Convert.ToDecimal(dataRow["TaxPST"] != DBNull.Value ? dataRow["TaxPST"] : "0");
                            
                            detailViewModel.SCOrderStatus = Convert.ToString(dataRow["ScOrderStatus"] != DBNull.Value ? dataRow["ScOrderStatus"] : "");
                            detailViewModel.ZincCode = Convert.ToString(dataRow["order_code"] != DBNull.Value ? dataRow["order_code"] : "");
                            detailViewModel.ZincMessage = Convert.ToString(dataRow["order_message"] != DBNull.Value ? dataRow["order_message"] : "");
                            detailViewModel.PaymentStatus = Convert.ToString(dataRow["payment_staus"] != DBNull.Value ? dataRow["payment_staus"] : "");
                            if (detailViewModel.ZincMessage != "")
                            {
                                if (detailViewModel.ZincMessage.Contains("Guaranteed delivery date"))
                                {
                                    detailViewModel.ZincMessage = detailViewModel.ZincMessage.Replace("Guaranteed delivery date", "Delivery Date");
                                }
                            }
                            detailViewModel.ZincOrderLogID = Convert.ToString(dataRow["zinc_order_log_id"] != DBNull.Value ? dataRow["zinc_order_log_id"] : "0");
                            detailViewModel.ZincRequestID = Convert.ToString(dataRow["request_id"] != DBNull.Value ? dataRow["request_id"] : "");
                            detailViewModel.ZincOrderLogDetailID = Convert.ToString(dataRow["zinc_order_log_detail_id"] != DBNull.Value ? dataRow["zinc_order_log_detail_id"] : "");
                            detailViewModel.ZincOrderStatusInternal = Convert.ToString(dataRow["zinc_order_status_internal"] != DBNull.Value ? dataRow["zinc_order_status_internal"] : "");
                            detailViewModel.IsTrackingUpdateToSC = Convert.ToBoolean(dataRow["is_tracking_updated"] != DBNull.Value ? dataRow["is_tracking_updated"] : false);
                            detailViewModel.DropshipStatus = Convert.ToBoolean(dataRow["dropship_status"] != DBNull.Value ? dataRow["dropship_status"] : false);
                            detailViewModel.OnOrder = Convert.ToInt32(dataRow["OnOrder"] != DBNull.Value ? dataRow["OnOrder"] : 0);
                            detailViewModel.DropshipQty = Convert.ToInt32(dataRow["dropship_Qty"] != DBNull.Value ? dataRow["dropship_Qty"] : "0");
                            detailViewModel.BestBuyPorductID = Convert.ToString(dataRow["bb_product_ID"] != DBNull.Value ? dataRow["bb_product_ID"] : "0");

                            //calculations
                            detailViewModel.calculation_TotalAmountOfUnitPrice = detailViewModel.TotalQuantity * detailViewModel.UnitPrice;
                            detailViewModel.calculation_TotalTax = detailViewModel.TaxGST + detailViewModel.TaxPST;
                            detailViewModel.calculation_TotalTacPercentage = Math.Round((detailViewModel.calculation_TotalTax / detailViewModel.calculation_TotalAmountOfUnitPrice) * 100, 2);
                            detailViewModel.calculation_Comission = Math.Round((detailViewModel.Comission) / (1 + detailViewModel.calculation_TotalTacPercentage / 100), 2);
                            detailViewModel.caculation_TotalAvgCost = Math.Round(Math.Round(Convert.ToDecimal(detailViewModel.AverageCost), 2) * detailViewModel.TotalQuantity, 2);
                            detailViewModel.calculation_SumTotal = Math.Round(detailViewModel.calculation_TotalTax + detailViewModel.calculation_TotalAmountOfUnitPrice, 2);
                            detailViewModel.calculation_comissionPercentage = Math.Round(((detailViewModel.calculation_Comission / detailViewModel.calculation_TotalAmountOfUnitPrice) * 100), 2);
                            detailViewModel.calculation_ProfitLoss = Math.Round(detailViewModel.calculation_TotalAmountOfUnitPrice - 0 - detailViewModel.caculation_TotalAvgCost - detailViewModel.calculation_Comission, 2);
                            detailViewModel.calculation_ProfitLossPercentage = Math.Round((detailViewModel.calculation_ProfitLoss / detailViewModel.calculation_TotalAmountOfUnitPrice) * 100, 2);

                           // List<ProductWarehouseQtyViewModel> warehouseQty = dataAccess.GetProductQtyBySKU_ForOrdersPage(detailViewModel.ProductSKU, mySqlConnection);
                            List<ProductWarehouseQtyViewModel> warehouseQty = dataAccess.GetWareHousesQtyList(detailViewModel.ProductSKU);

                            detailViewModel.ProductrWarehouseQtyViewModel = warehouseQty;
                            objList.Add(detailViewModel);

                        }
                        //if (ViewModel.OrderNumber != string.Empty)
                        //{
                        //    ViewModel.BBProductDetail = GetAllBestBuyOrdersDetail(ViewModel.OrderNumber, connStr);
                        //    // ViewModel.BBProductDetail = null;
                        //}

                        ViewModel.BBProductDetail = objList;
                        listBBProductViewModel.Add(ViewModel);
                    }

                    if (mySqlConnection.State == ConnectionState.Open)
                    {
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
            }
           return listBBProductViewModel;

        }



        public List<BestBuyOrdersViewModel> GetAllBestBuyOrdersSearch(BestBuyOrderSearchTotalCountViewModel viewModel)
        {
            List<BestBuyOrdersViewModel> listBBProductViewModel = null;
            try
            {
                System.Data.DataSet ds = new System.Data.DataSet();
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {

                    MySqlCommand cmd = new MySqlCommand("forTestingPurposeDumyCopy", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("zincOrderStatus", viewModel.ZincStatus);
                    cmd.Parameters.AddWithValue("sort", viewModel.Sort);
                    cmd.Parameters.AddWithValue("startLimit", viewModel.StartIndex);
                    cmd.Parameters.AddWithValue("endLimit", viewModel.EndIndex);
                    cmd.Parameters.AddWithValue("OrderStatus", viewModel.OrderStatus);
                    cmd.Parameters.AddWithValue("sku", viewModel.Sku);
                    cmd.Parameters.AddWithValue("dateFrom", viewModel.DateFrom);
                    cmd.Parameters.AddWithValue("dateTo", viewModel.DateTo);
                    cmd.Parameters.AddWithValue("DSStatus", viewModel.DSStatus);
                    cmd.Parameters.AddWithValue("PaymentStatus", viewModel.PaymentStatus);
                    cmd.Parameters.AddWithValue("TagSearch", viewModel.ShippingTags);
                    cmd.Parameters.AddWithValue("_ShippingBoxContain", viewModel.ShippingBoxContain);

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(ds);
                    listBBProductViewModel = new List<BestBuyOrdersViewModel>();
                    System.Data.DataView dataView = new System.Data.DataView(ds.Tables[0]);
                    System.Data.DataTable distinctValue = dataView.ToTable(true, "sellerCloudID");
                    DataTable dt = ds.Tables[0];
                    foreach (System.Data.DataRow reader in distinctValue.Rows)
                    {
                        List<BestBuyOrderDetailViewModel> objList = new List<BestBuyOrderDetailViewModel>();

                        var list = dt.AsEnumerable().Where(e => e.Field<string>("sellerCloudID") == reader["sellerCloudID"].ToString()).ToList();

                        var totalQuantity = list.Sum(e => Convert.ToDouble(e.Field<string>("quantity")));
                        var totalPrice = list.Sum(e => Convert.ToDouble(e.Field<string>("total_price")));
                        var total_commission = list.Sum(e => Convert.ToDouble(e.Field<string>("total_commission")));
                        var totalGst = list.Sum(e => Convert.ToDouble(e.Field<string>("TaxGST")));
                        var totalPst = list.Sum(e => Convert.ToDouble(e.Field<string>("TaxPST")));
                        var toalAverageCost = list.Sum(e => e.Field<double>("TotalAverageCost"));


                        var sumOfGstPst = totalGst + totalPst;

                        var total = (sumOfGstPst / totalPrice) * 100;

                        //var totalComission = Math.Round((total_commission) / (1 + total / 100), 2);
                        var totalComission = total_commission;
                        var ProfitLoss = Math.Round(totalPrice - toalAverageCost - total_commission, 2);
                        var profitAndLossInPercent = Math.Round((ProfitLoss / totalPrice) * 100);


                        BestBuyOrdersViewModel ViewModel = new BestBuyOrdersViewModel();
                        ViewModel.OrderNumber = Convert.ToString(list.Select(e => e.Field<string>("order_id")).FirstOrDefault());
                        ViewModel.SellerCloudOrderID = Convert.ToString(list.Select(e => e.Field<string>("sellerCloudID")).FirstOrDefault());
                        ViewModel.ShipmentAddress = Convert.ToString(list.Select(e => e.Field<string>("CustomerAddress")).FirstOrDefault());
                        ViewModel.OrderDate = Convert.ToDateTime(list.Select(e => e.Field<DateTime>("acceptance_decision_date")).FirstOrDefault());
                        ViewModel.ParentOrderID = Convert.ToInt32(list.Select(e => e.Field<int?>("ParentOrderID")).FirstOrDefault() != null ? list.Select(e => e.Field<int>("ParentOrderID")).FirstOrDefault() : 0).ToString();
                        ViewModel.IsParent = Convert.ToString(list.Select(e => e.Field<string>("IsParent")).FirstOrDefault());
                        ViewModel.IsNotes = Convert.ToString(list.Select(e => e.Field<string>("IsNotes")).FirstOrDefault());
                        ViewModel.ShippingPrice = Convert.ToString(list.Select(e => e.Field<string>("ShippingPaidByCustomer")).FirstOrDefault());

                        ViewModel.TotalTax = Convert.ToDecimal(0);
                        ViewModel.TotalQuantity = Convert.ToInt32(totalQuantity);
                        ViewModel.TotalComission = Convert.ToDecimal(total_commission);
                        ViewModel.TotalPrice = Convert.ToDecimal(totalPrice);
                        ViewModel.ProfitAndLossInDollar = Convert.ToDecimal(ProfitLoss);
                        ViewModel.ProfitAndLossInPercentage = Convert.ToDecimal(profitAndLossInPercent);
                        ViewModel.TotalAverageCost = Convert.ToDecimal(toalAverageCost);
                        foreach (DataRow dataRow in list)
                        {

                            BestBuyOrderDetailViewModel detailViewModel = new BestBuyOrderDetailViewModel();
                            detailViewModel.Location = "";
                            detailViewModel.OrderStatus = Convert.ToString(dataRow["order_line_state"] != DBNull.Value ? dataRow["order_line_state"] : string.Empty);
                            detailViewModel.ProductSKU = Convert.ToString(dataRow["sku"] != DBNull.Value ? dataRow["sku"] : string.Empty);
                            detailViewModel.Prime = Convert.ToString("Y");
                            if (dataRow["image_name"] != DBNull.Value)
                            {
                                detailViewModel.ImageUrl = Convert.ToString(dataRow["image_name"]);
                            }
                            else
                            {
                                detailViewModel.ImageUrl = "";
                            }
                            detailViewModel.ProductTitle = Convert.ToString(dataRow["title"] != DBNull.Value ? dataRow["title"] : string.Empty);
                            detailViewModel.TotalQuantity = Convert.ToInt32(dataRow["quantity"] != DBNull.Value ? dataRow["quantity"] : "0");
                            detailViewModel.AverageCost = Convert.ToString(dataRow["avg_cost"] != DBNull.Value ? dataRow["avg_cost"] : "0");
                            detailViewModel.WarehouseQuantity = Convert.ToString("10");
                            if (dataRow["CreditCardId"] != DBNull.Value)
                            {
                                ViewModel.CreditCardId = Convert.ToInt32(dataRow["CreditCardId"] != DBNull.Value ? dataRow["CreditCardId"] : 0);
                            }
                            if (dataRow["ZincAccountId"] != DBNull.Value)
                            {
                                ViewModel.ZincAccountId = Convert.ToInt32(dataRow["ZincAccountId"] != DBNull.Value ? dataRow["ZincAccountId"] : 0);
                            }
                            detailViewModel.ShippingFee = Convert.ToDecimal(dataRow["ShippingFee"] != DBNull.Value ? dataRow["ShippingFee"] : 0);
                            detailViewModel.ZincASIN = Convert.ToString("");
                            detailViewModel.ZincLink = Convert.ToString("");
                            detailViewModel.ZincStatus = Convert.ToString(dataRow["drop_ship_status"] != DBNull.Value ? dataRow["drop_ship_status"] : "");
                            detailViewModel.ProfitLoss = "";
                            detailViewModel.Comission = Convert.ToDecimal(dataRow["total_commission"] != DBNull.Value ? dataRow["total_commission"] : "0");
                            detailViewModel.UnitPrice = Convert.ToDecimal(dataRow["total_price"] != DBNull.Value ? dataRow["total_price"] : "0");
                            detailViewModel.UnitPrice = Math.Round((detailViewModel.UnitPrice - detailViewModel.ShippingFee) / detailViewModel.TotalQuantity, 2);
                            detailViewModel.OrderDetailID = Convert.ToInt32(dataRow["bbe2_line_id"] != DBNull.Value ? dataRow["bbe2_line_id"] : "0");
                            detailViewModel.TaxGST = Convert.ToDecimal(dataRow["TaxGST"] != DBNull.Value ? dataRow["TaxGST"] : "0");
                            detailViewModel.TaxPST = Convert.ToDecimal(dataRow["TaxPST"] != DBNull.Value ? dataRow["TaxPST"] : "0");
                            detailViewModel.SCOrderStatus = Convert.ToString(dataRow["ScOrderStatus"] != DBNull.Value ? dataRow["ScOrderStatus"] : "");
                            detailViewModel.ZincCode = Convert.ToString(dataRow["order_code"] != DBNull.Value ? dataRow["order_code"] : "");
                            detailViewModel.ZincMessage = Convert.ToString(dataRow["order_message"] != DBNull.Value ? dataRow["order_message"] : "");
                            detailViewModel.PaymentStatus = Convert.ToString(dataRow["payment_staus"] != DBNull.Value ? dataRow["payment_staus"] : "");
                            if (detailViewModel.ZincMessage != "")
                            {
                                if (detailViewModel.ZincMessage.Contains("Guaranteed delivery date"))
                                {
                                    detailViewModel.ZincMessage = detailViewModel.ZincMessage.Replace("Guaranteed delivery date", "Delivery Date");
                                }
                            }
                            detailViewModel.ZincOrderLogID = Convert.ToString(dataRow["zinc_order_log_id"] != DBNull.Value ? dataRow["zinc_order_log_id"] : "0");
                            detailViewModel.ZincRequestID = Convert.ToString(dataRow["request_id"] != DBNull.Value ? dataRow["request_id"] : "");
                            detailViewModel.ZincOrderLogDetailID = Convert.ToString(dataRow["zinc_order_log_detail_id"] != DBNull.Value ? dataRow["zinc_order_log_detail_id"] : "");
                            detailViewModel.ZincOrderStatusInternal = Convert.ToString(dataRow["zinc_order_status_internal"] != DBNull.Value ? dataRow["zinc_order_status_internal"] : "");
                            detailViewModel.IsTrackingUpdateToSC = Convert.ToBoolean(dataRow["is_tracking_updated"] != DBNull.Value ? dataRow["is_tracking_updated"] : false);
                            detailViewModel.DropshipStatus = Convert.ToBoolean(dataRow["dropship_status"] != DBNull.Value ? dataRow["dropship_status"] : false);
                            detailViewModel.OnOrder = Convert.ToInt32(dataRow["OnOrder"] != DBNull.Value ? dataRow["OnOrder"] : 0);
                            detailViewModel.DropshipQty = Convert.ToInt32(dataRow["dropship_Qty"] != DBNull.Value ? dataRow["dropship_Qty"] : "0");
                            detailViewModel.BestBuyPorductID = Convert.ToString(dataRow["bb_product_ID"] != DBNull.Value ? dataRow["bb_product_ID"] : "0");

                            //calculations
                            detailViewModel.calculation_TotalAmountOfUnitPrice = detailViewModel.TotalQuantity * detailViewModel.UnitPrice;
                            detailViewModel.calculation_TotalTax = detailViewModel.TaxGST + detailViewModel.TaxPST;
                            detailViewModel.calculation_TotalTacPercentage = Math.Round((detailViewModel.calculation_TotalTax / detailViewModel.calculation_TotalAmountOfUnitPrice) * 100, 2);
                            detailViewModel.calculation_Comission = Math.Round((detailViewModel.Comission) / (1 + detailViewModel.calculation_TotalTacPercentage / 100), 2);
                            detailViewModel.caculation_TotalAvgCost = Math.Round(Math.Round(Convert.ToDecimal(detailViewModel.AverageCost), 2) * detailViewModel.TotalQuantity, 2);
                            detailViewModel.calculation_SumTotal = Math.Round(detailViewModel.calculation_TotalTax + detailViewModel.calculation_TotalAmountOfUnitPrice, 2);
                            detailViewModel.calculation_comissionPercentage = Math.Round(((detailViewModel.calculation_Comission / detailViewModel.calculation_TotalAmountOfUnitPrice) * 100), 2);
                            detailViewModel.calculation_ProfitLoss = Math.Round(detailViewModel.calculation_TotalAmountOfUnitPrice - 0 - detailViewModel.caculation_TotalAvgCost - detailViewModel.calculation_Comission, 2);
                            detailViewModel.calculation_ProfitLossPercentage = Math.Round((detailViewModel.calculation_ProfitLoss / detailViewModel.calculation_TotalAmountOfUnitPrice) * 100, 2);


                            //List<ProductWarehouseQtyViewModel> warehouseQty = dataAccess.GetProductQtyBySKU_ForOrdersPage(detailViewModel.ProductSKU, conn);
                            List<ProductWarehouseQtyViewModel> warehouseQty = dataAccess.GetWareHousesQtyList(detailViewModel.ProductSKU);
                            detailViewModel.ProductrWarehouseQtyViewModel = warehouseQty;
                            objList.Add(detailViewModel);



                        }
                        //if (ViewModel.OrderNumber != string.Empty)
                        //{
                        //    ViewModel.BBProductDetail = GetAllBestBuyOrdersDetail(ViewModel.OrderNumber, connStr);
                        //    // ViewModel.BBProductDetail = null;
                        //}

                        ViewModel.BBProductDetail = objList;
                        listBBProductViewModel.Add(ViewModel);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listBBProductViewModel;

        }


        public bool SaveBestBuyOrderDropShipMovement(BestBuyDropShipQtyMovementViewModel viewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_SaveBestBuyDropshipMovement", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_product_sku", viewModel.ProductSku);
                    cmd.Parameters.AddWithValue("_order_qty", viewModel.OrderQuantity);
                    cmd.Parameters.AddWithValue("_order_date", viewModel.OrderDate);
                    cmd.Parameters.AddWithValue("_is_ds_status_updated_id", false);
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public TotalCountWithBestBuyOrderViewModel GetAllBestBuyOrdersSearchTotalCount(BestBuyOrderSearchTotalCountViewModel viewModel)
        {
            TotalCountWithBestBuyOrderViewModel model = new TotalCountWithBestBuyOrderViewModel();
            try
            {
                System.Data.DataSet ds = new System.Data.DataSet();
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetBestBuyOrders_SearchTotalCountCopyOne", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("zincOrderStatus", viewModel.ZincStatus);
                    cmd.Parameters.AddWithValue("sort", viewModel.Sort);
                    cmd.Parameters.AddWithValue("OrderStatus", viewModel.OrderStatus);
                    cmd.Parameters.AddWithValue("sku", viewModel.Sku);
                    cmd.Parameters.AddWithValue("dateFrom", viewModel.DateFrom);
                    cmd.Parameters.AddWithValue("dateTo", viewModel.DateTo);
                    cmd.Parameters.AddWithValue("DSStatus", viewModel.DSStatus);
                    cmd.Parameters.AddWithValue("TagSearch", viewModel.ShippingTags);
                    cmd.Parameters.AddWithValue("PaymentStatus", viewModel.PaymentStatus);
                    cmd.Parameters.AddWithValue("_ShippingBoxContain", viewModel.ShippingBoxContain);


                    var da = new MySqlDataAdapter(cmd);

                    da.Fill(ds);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        model.count = Convert.ToString(ds.Tables[0].Rows[0][0] != DBNull.Value ? ds.Tables[0].Rows[0][0] : "0");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return model;
        }

        public TotalCountWithBestBuyOrderViewModel GetAllBestBuyOrdersTotalCount(string FilterName, string FilterValue)
        {
            TotalCountWithBestBuyOrderViewModel model = new TotalCountWithBestBuyOrderViewModel();
            try
            {
                System.Data.DataSet ds = new System.Data.DataSet();
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetBestBuyOrderTotalCountNew", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("FilterName", FilterName);
                    cmd.Parameters.AddWithValue("FilterValue", FilterValue);


                    var da = new MySqlDataAdapter(cmd);

                    da.Fill(ds);

                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        model.count = Convert.ToString(ds.Tables[0].Rows[0][0]);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return model;
        }

        //following call is orignal call 
        public TotalCountWithBestBuyOrderViewModel GetAllBestBuyOrdersTotalCounts(string FilterName, string FilterValue)
        {
            TotalCountWithBestBuyOrderViewModel model = new TotalCountWithBestBuyOrderViewModel();
            try
            {
                System.Data.DataSet ds = new System.Data.DataSet();
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetBestBuyOrderTotalCount", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("FilterName", FilterName);
                    cmd.Parameters.AddWithValue("FilterValue", FilterValue);


                    var da = new MySqlDataAdapter(cmd);

                    da.Fill(ds);

                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        model.count = Convert.ToString(ds.Tables[0].Rows[0][0]);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return model;
        }

        public List<BestBuyOrdersViewModel> GetAllBestBuyOrdersWithGlobalFilter(string FilterName, string FilterValue, int startLimit, int endLimit, string sort)
        {
            List<BestBuyOrdersViewModel> listBBProductViewModel = null;
            try
            {
                System.Data.DataSet ds = new System.Data.DataSet();
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {

                    MySqlCommand cmd = new MySqlCommand("p_GetBestBuyOrdersWithGlobalFiltersWithDynamicQueryNew", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("FilterName", FilterName);
                    cmd.Parameters.AddWithValue("FilterValue", FilterValue);
                    cmd.Parameters.AddWithValue("startLimit", startLimit);
                    cmd.Parameters.AddWithValue("endLimit", endLimit);
                    cmd.Parameters.AddWithValue("sort", sort);


                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(ds);
                    listBBProductViewModel = new List<BestBuyOrdersViewModel>();
                    System.Data.DataView dataView = new System.Data.DataView(ds.Tables[0]);
                    System.Data.DataTable distinctValue = dataView.ToTable(true, "order_id");
                    DataTable dt = ds.Tables[0];
                    foreach (System.Data.DataRow reader in distinctValue.Rows)
                    {
                        List<BestBuyOrderDetailViewModel> objList = new List<BestBuyOrderDetailViewModel>();

                        var list = dt.AsEnumerable().Where(e => e.Field<string>("order_id") == reader["order_id"].ToString()).ToList();


                        var totalQuantity = list.Sum(e => Convert.ToDouble(e.Field<string>("quantity")));
                        var totalPrice = list.Sum(e => Convert.ToDouble(e.Field<string>("total_price")));
                        var total_commission = list.Sum(e => Convert.ToDouble(e.Field<string>("total_commission")));
                        var totalGst = list.Sum(e => Convert.ToDouble(e.Field<string>("TaxGST")));
                        var totalPst = list.Sum(e => Convert.ToDouble(e.Field<string>("TaxPST")));
                        var toalAverageCost = list.Sum(e => e.Field<double>("TotalAverageCost"));
                        //var ShippingFee= list.Sum(e => Convert.ToDouble(e.Field<string>("ShippingFee")));

                        var sumOfGstPst = totalGst + totalPst;

                        var total = (sumOfGstPst / totalPrice) * 100;

                        //var totalComission = Math.Round((total_commission) / (1 + total / 100), 2);
                        var totalComission = total_commission;


                        BestBuyOrdersViewModel ViewModel = new BestBuyOrdersViewModel();
                        ViewModel.OrderNumber = Convert.ToString(list.Select(e => e.Field<string>("order_id")).FirstOrDefault());
                        ViewModel.SellerCloudOrderID = Convert.ToString(list.Select(e => e.Field<int>("sellerCloudID")).FirstOrDefault());

                        ViewModel.ShipmentAddress = Convert.ToString(list.Select(e => e.Field<string>("CustomerAddress")).FirstOrDefault());
                        ViewModel.OrderDate = Convert.ToDateTime(list.Select(e => e.Field<DateTime>("acceptance_decision_date")).FirstOrDefault());
                        ViewModel.ParentOrderID = Convert.ToInt32(list.Select(e => e.Field<int?>("ParentOrderID")).FirstOrDefault() != null ? list.Select(e => e.Field<int>("ParentOrderID")).FirstOrDefault() : 0).ToString();
                        ViewModel.IsParent = Convert.ToString(list.Select(e => e.Field<string>("IsParent")).FirstOrDefault());
                        ViewModel.IsNotes = Convert.ToString(list.Select(e => e.Field<string>("IsNotes")).FirstOrDefault());
                        ViewModel.ShippingPrice = Convert.ToString(list.Select(e => e.Field<string>("ShippingPaidByCustomer")).FirstOrDefault());

                        var ProfitLoss = Math.Round(totalPrice - toalAverageCost - total_commission, 2);
                        var profitAndLossInPercent = Math.Round((ProfitLoss / totalPrice) * 100);

                        ViewModel.TotalTax = Convert.ToDecimal(0);
                        ViewModel.TotalQuantity = Convert.ToInt32(totalQuantity);
                        ViewModel.TotalComission = Convert.ToDecimal(total_commission);
                        ViewModel.TotalPrice = Convert.ToDecimal(totalPrice);
                        ViewModel.ProfitAndLossInDollar = Convert.ToDecimal(ProfitLoss);
                        ViewModel.ProfitAndLossInPercentage = Convert.ToDecimal(profitAndLossInPercent);
                        ViewModel.TotalAverageCost = Convert.ToDecimal(toalAverageCost);



                        foreach (DataRow dataRow in list)
                        {

                            BestBuyOrderDetailViewModel detailViewModel = new BestBuyOrderDetailViewModel();
                            detailViewModel.Location = "";
                            detailViewModel.LocationNotes = Convert.ToString(dataRow["LocationNotes"] != DBNull.Value ? dataRow["LocationNotes"] : string.Empty);
                            detailViewModel.ShadowOf = Convert.ToString(dataRow["ShadowOf"] != DBNull.Value ? dataRow["ShadowOf"] : string.Empty);

                            detailViewModel.OrderStatus = Convert.ToString(dataRow["order_line_state"] != DBNull.Value ? dataRow["order_line_state"] : string.Empty);
                            detailViewModel.ProductSKU = Convert.ToString(dataRow["sku"] != DBNull.Value ? dataRow["sku"] : string.Empty);
                            detailViewModel.Prime = Convert.ToString("Y");
                            detailViewModel.ShippingFee = Convert.ToDecimal(dataRow["ShippingFee"] != DBNull.Value ? dataRow["ShippingFee"] : 0);
                            if (dataRow["image_name"] != DBNull.Value)
                            {
                                detailViewModel.ImageUrl = Convert.ToString(dataRow["image_name"]);
                            }
                            else
                            {
                                detailViewModel.ImageUrl = "";
                            }
                            detailViewModel.AverageCost = Convert.ToString(dataRow["avg_cost"] != DBNull.Value ? dataRow["avg_cost"] : "0");
                            detailViewModel.ProductTitle = Convert.ToString(dataRow["title"] != DBNull.Value ? dataRow["title"] : string.Empty);
                            detailViewModel.TotalQuantity = Convert.ToInt32(dataRow["quantity"] != DBNull.Value ? dataRow["quantity"] : "0");
                            detailViewModel.WarehouseQuantity = Convert.ToString("10");

                            if (dataRow["CreditCardId"] != DBNull.Value)
                            {
                                ViewModel.CreditCardId = Convert.ToInt32(dataRow["CreditCardId"] != DBNull.Value ? dataRow["CreditCardId"] : 0);
                            }
                            if (dataRow["ZincAccountId"] != DBNull.Value)
                            {
                                ViewModel.ZincAccountId = Convert.ToInt32(dataRow["ZincAccountId"] != DBNull.Value ? dataRow["ZincAccountId"] : 0);
                            }

                            detailViewModel.ZincASIN = Convert.ToString("");
                            detailViewModel.ZincLink = Convert.ToString("");
                            detailViewModel.ZincStatus = Convert.ToString(dataRow["drop_ship_status"] != DBNull.Value ? dataRow["drop_ship_status"] : "");
                            detailViewModel.ProfitLoss = "";
                            detailViewModel.Comission = Convert.ToDecimal(dataRow["total_commission"] != DBNull.Value ? dataRow["total_commission"] : "0");
                            detailViewModel.UnitPrice = Convert.ToDecimal(dataRow["total_price"] != DBNull.Value ? dataRow["total_price"] : "0");
                            detailViewModel.UnitPrice = Math.Round((detailViewModel.UnitPrice - detailViewModel.ShippingFee) / detailViewModel.TotalQuantity, 2);
                            detailViewModel.OrderDetailID = Convert.ToInt32(dataRow["order_line_id"] != DBNull.Value ? dataRow["order_line_id"] : "0");

                            detailViewModel.TaxGST = Convert.ToDecimal(dataRow["TaxGST"] != DBNull.Value ? dataRow["TaxGST"] : "0");
                            detailViewModel.TaxPST = Convert.ToDecimal(dataRow["TaxPST"] != DBNull.Value ? dataRow["TaxPST"] : "0");
                            detailViewModel.SCOrderStatus = Convert.ToString(dataRow["ScOrderStatus"] != DBNull.Value ? dataRow["ScOrderStatus"] : "");
                            //detailViewModel.ZincCode = Convert.ToString(dataRow["order_code"] != DBNull.Value ? dataRow["order_code"] : "");
                            //detailViewModel.ZincMessage = Convert.ToString(dataRow["order_message"] != DBNull.Value ? dataRow["order_message"] : "");
                            detailViewModel.ZincMessage = "";
                            detailViewModel.PaymentStatus = Convert.ToString(dataRow["payment_staus"] != DBNull.Value ? dataRow["payment_staus"] : "");
                            if (detailViewModel.ZincMessage != "")
                            {
                                if (detailViewModel.ZincMessage.Contains("Guaranteed delivery date"))
                                {
                                    detailViewModel.ZincMessage = detailViewModel.ZincMessage.Replace("Guaranteed delivery date", "Delivery Date");
                                }
                            }
                            detailViewModel.ZincOrderLogID = "0"; 
                            detailViewModel.ZincRequestID = Convert.ToString(dataRow["request_id"] != DBNull.Value ? dataRow["request_id"] : "");
                            detailViewModel.ZincOrderLogDetailID = "";
                            detailViewModel.ZincOrderStatusInternal = Convert.ToString(dataRow["zinc_order_status_internal"] != DBNull.Value ? dataRow["zinc_order_status_internal"] : "");
                            detailViewModel.IsTrackingUpdateToSC = Convert.ToBoolean(dataRow["is_tracking_updated"] != DBNull.Value ? dataRow["is_tracking_updated"] : false);
                            detailViewModel.DropshipStatus = Convert.ToBoolean(dataRow["dropship_status"] != DBNull.Value ? dataRow["dropship_status"] : false);
                            detailViewModel.OnOrder = Convert.ToInt32(dataRow["OnOrder"] != DBNull.Value ? dataRow["OnOrder"] : 0);
                            detailViewModel.DropshipQty = Convert.ToInt32(dataRow["dropship_Qty"] != DBNull.Value ? dataRow["dropship_Qty"] : "0");
                            detailViewModel.BestBuyPorductID = Convert.ToString(dataRow["bb_product_ID"] != DBNull.Value ? dataRow["bb_product_ID"] : "0");

                            //calculations
                            detailViewModel.calculation_TotalAmountOfUnitPrice = detailViewModel.TotalQuantity * detailViewModel.UnitPrice;
                            detailViewModel.calculation_TotalTax = detailViewModel.TaxGST + detailViewModel.TaxPST;
                            detailViewModel.calculation_TotalTacPercentage = Math.Round((detailViewModel.calculation_TotalTax / detailViewModel.calculation_TotalAmountOfUnitPrice) * 100, 2);
                            //detailViewModel.calculation_Comission = Math.Round((detailViewModel.Comission) / (1 + detailViewModel.calculation_TotalTacPercentage / 100), 2);
                            detailViewModel.calculation_Comission = Math.Round((detailViewModel.Comission), 2);
                            detailViewModel.caculation_TotalAvgCost = Math.Round(Math.Round(Convert.ToDecimal(detailViewModel.AverageCost), 2) * detailViewModel.TotalQuantity, 2);
                            detailViewModel.calculation_SumTotal = Math.Round(detailViewModel.calculation_TotalTax + detailViewModel.calculation_TotalAmountOfUnitPrice, 2);
                            detailViewModel.calculation_comissionPercentage = Math.Round(((detailViewModel.calculation_Comission / detailViewModel.calculation_TotalAmountOfUnitPrice) * 100), 2);
                            detailViewModel.calculation_ProfitLoss = Math.Round(detailViewModel.calculation_TotalAmountOfUnitPrice - 0 - detailViewModel.caculation_TotalAvgCost - detailViewModel.calculation_Comission, 2);
                            detailViewModel.calculation_ProfitLossPercentage = Math.Round((detailViewModel.calculation_ProfitLoss / detailViewModel.calculation_TotalAmountOfUnitPrice) * 100, 2);


                            //List<ProductWarehouseQtyViewModel> warehouseQty = dataAccess.GetProductQtyBySKU_ForOrdersPage(detailViewModel.ProductSKU, conn);
                            List<ProductWarehouseQtyViewModel> warehouseQty = dataAccess.GetWareHousesQtyList(detailViewModel.ProductSKU);
                            detailViewModel.ProductrWarehouseQtyViewModel = warehouseQty;
                            objList.Add(detailViewModel);



                            //SendNewOrderEmail email = new SendNewOrderEmail();
                            //ViewModel.BBProductDetail = objList;
                            //List<BestBuyOrdersViewModel> mode = new List<BestBuyOrdersViewModel>();

                            //mode.Add(ViewModel);
                            //email.SendrderEmail(mode);
                        }
                        //if (ViewModel.OrderNumber != string.Empty)
                        //{
                        //    ViewModel.BBProductDetail = GetAllBestBuyOrdersDetail(ViewModel.OrderNumber, connStr);
                        //    // ViewModel.BBProductDetail = null;
                        //}


                        ViewModel.BBProductDetail = objList;

                        listBBProductViewModel.Add(ViewModel);
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return listBBProductViewModel;
        }

        /*following request is irignal */
        public List<BestBuyOrdersViewModel> GetAllBestBuyOrdersWithGlobalFilters(string FilterName, string FilterValue, int startLimit, int endLimit, string sort)
        {
            List<BestBuyOrdersViewModel> listBBProductViewModel = null;
            try
            {
                System.Data.DataSet ds = new System.Data.DataSet();
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {

                    MySqlCommand cmd = new MySqlCommand("p_GetBestBuyOrdersWithGlobalFiltersWithDynamicQuery", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("FilterName", FilterName);
                    cmd.Parameters.AddWithValue("FilterValue", FilterValue);
                    cmd.Parameters.AddWithValue("startLimit", startLimit);
                    cmd.Parameters.AddWithValue("endLimit", endLimit);
                    cmd.Parameters.AddWithValue("sort", sort);


                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(ds);
                    listBBProductViewModel = new List<BestBuyOrdersViewModel>();
                    System.Data.DataView dataView = new System.Data.DataView(ds.Tables[0]);
                    System.Data.DataTable distinctValue = dataView.ToTable(true, "sellerCloudID");
                    DataTable dt = ds.Tables[0];
                    foreach (System.Data.DataRow reader in distinctValue.Rows)
                    {
                        List<BestBuyOrderDetailViewModel> objList = new List<BestBuyOrderDetailViewModel>();

                        var list = dt.AsEnumerable().Where(e => e.Field<string>("sellerCloudID") == reader["sellerCloudID"].ToString()).ToList();


                        var totalQuantity = list.Sum(e => Convert.ToDouble(e.Field<string>("quantity")));
                        var totalPrice = list.Sum(e => Convert.ToDouble(e.Field<string>("total_price")));
                        var total_commission = list.Sum(e => Convert.ToDouble(e.Field<string>("total_commission")));
                        var totalGst = list.Sum(e => Convert.ToDouble(e.Field<string>("TaxGST")));
                        var totalPst = list.Sum(e => Convert.ToDouble(e.Field<string>("TaxPST")));
                        var toalAverageCost = list.Sum(e => e.Field<double>("TotalAverageCost"));
                        //var ShippingFee= list.Sum(e => Convert.ToDouble(e.Field<string>("ShippingFee")));

                        var sumOfGstPst = totalGst + totalPst;

                        var total = (sumOfGstPst / totalPrice) * 100;

                        //var totalComission = Math.Round((total_commission) / (1 + total / 100), 2);
                        var totalComission = total_commission;


                        BestBuyOrdersViewModel ViewModel = new BestBuyOrdersViewModel();
                        ViewModel.OrderNumber = Convert.ToString(list.Select(e => e.Field<string>("order_id")).FirstOrDefault());
                        ViewModel.SellerCloudOrderID = Convert.ToString(list.Select(e => e.Field<string>("sellerCloudID")).FirstOrDefault());

                        ViewModel.ShipmentAddress = Convert.ToString(list.Select(e => e.Field<string>("CustomerAddress")).FirstOrDefault());
                        ViewModel.OrderDate = Convert.ToDateTime(list.Select(e => e.Field<DateTime>("acceptance_decision_date")).FirstOrDefault());
                        ViewModel.ParentOrderID = Convert.ToInt32(list.Select(e => e.Field<int?>("ParentOrderID")).FirstOrDefault() != null ? list.Select(e => e.Field<int>("ParentOrderID")).FirstOrDefault() : 0).ToString();
                        ViewModel.IsParent = Convert.ToString(list.Select(e => e.Field<string>("IsParent")).FirstOrDefault());
                        ViewModel.IsNotes = Convert.ToString(list.Select(e => e.Field<string>("IsNotes")).FirstOrDefault());
                        ViewModel.ShippingPrice = Convert.ToString(list.Select(e => e.Field<string>("ShippingPaidByCustomer")).FirstOrDefault());

                        var ProfitLoss = Math.Round(totalPrice - toalAverageCost - total_commission, 2);
                        var profitAndLossInPercent = Math.Round((ProfitLoss / totalPrice) * 100);

                        ViewModel.TotalTax = Convert.ToDecimal(0);
                        ViewModel.TotalQuantity = Convert.ToInt32(totalQuantity);
                        ViewModel.TotalComission = Convert.ToDecimal(total_commission);
                        ViewModel.TotalPrice = Convert.ToDecimal(totalPrice);
                        ViewModel.ProfitAndLossInDollar = Convert.ToDecimal(ProfitLoss);
                        ViewModel.ProfitAndLossInPercentage = Convert.ToDecimal(profitAndLossInPercent);
                        ViewModel.TotalAverageCost = Convert.ToDecimal(toalAverageCost);



                        foreach (DataRow dataRow in list)
                        {

                            BestBuyOrderDetailViewModel detailViewModel = new BestBuyOrderDetailViewModel();
                            detailViewModel.Location = "";
                            detailViewModel.LocationNotes = Convert.ToString(dataRow["LocationNotes"] != DBNull.Value ? dataRow["LocationNotes"] : string.Empty);
                            detailViewModel.ShadowOf = Convert.ToString(dataRow["ShadowOf"] != DBNull.Value ? dataRow["ShadowOf"] : string.Empty);

                            detailViewModel.OrderStatus = Convert.ToString(dataRow["order_line_state"] != DBNull.Value ? dataRow["order_line_state"] : string.Empty);
                            detailViewModel.ProductSKU = Convert.ToString(dataRow["sku"] != DBNull.Value ? dataRow["sku"] : string.Empty);
                            detailViewModel.Prime = Convert.ToString("Y");
                            detailViewModel.ShippingFee = Convert.ToDecimal(dataRow["ShippingFee"] != DBNull.Value ? dataRow["ShippingFee"] : 0);
                            if (dataRow["image_name"] != DBNull.Value)
                            {
                                detailViewModel.ImageUrl = Convert.ToString(dataRow["image_name"]);
                            }
                            else
                            {
                                detailViewModel.ImageUrl = "";
                            }
                            detailViewModel.AverageCost = Convert.ToString(dataRow["avg_cost"] != DBNull.Value ? dataRow["avg_cost"] : "0");
                            detailViewModel.ProductTitle = Convert.ToString(dataRow["title"] != DBNull.Value ? dataRow["title"] : string.Empty);
                            detailViewModel.TotalQuantity = Convert.ToInt32(dataRow["quantity"] != DBNull.Value ? dataRow["quantity"] : "0");
                            detailViewModel.WarehouseQuantity = Convert.ToString("10");

                            if (dataRow["CreditCardId"] != DBNull.Value)
                            {
                                ViewModel.CreditCardId = Convert.ToInt32(dataRow["CreditCardId"] != DBNull.Value ? dataRow["CreditCardId"] : 0);
                            }
                            if (dataRow["ZincAccountId"] != DBNull.Value)
                            {
                                ViewModel.ZincAccountId = Convert.ToInt32(dataRow["ZincAccountId"] != DBNull.Value ? dataRow["ZincAccountId"] : 0);
                            }

                            detailViewModel.ZincASIN = Convert.ToString("");
                            detailViewModel.ZincLink = Convert.ToString("");
                            detailViewModel.ZincStatus = Convert.ToString(dataRow["drop_ship_status"] != DBNull.Value ? dataRow["drop_ship_status"] : "");
                            detailViewModel.ProfitLoss = "";
                            detailViewModel.Comission = Convert.ToDecimal(dataRow["total_commission"] != DBNull.Value ? dataRow["total_commission"] : "0");
                            detailViewModel.UnitPrice = Convert.ToDecimal(dataRow["total_price"] != DBNull.Value ? dataRow["total_price"] : "0");
                            detailViewModel.UnitPrice = Math.Round((detailViewModel.UnitPrice - detailViewModel.ShippingFee) / detailViewModel.TotalQuantity, 2);
                            detailViewModel.OrderDetailID = Convert.ToInt32(dataRow["bbe2_line_id"] != DBNull.Value ? dataRow["bbe2_line_id"] : "0");
                            detailViewModel.TaxGST = Convert.ToDecimal(dataRow["TaxGST"] != DBNull.Value ? dataRow["TaxGST"] : "0");
                            detailViewModel.TaxPST = Convert.ToDecimal(dataRow["TaxPST"] != DBNull.Value ? dataRow["TaxPST"] : "0");
                            detailViewModel.SCOrderStatus = Convert.ToString(dataRow["ScOrderStatus"] != DBNull.Value ? dataRow["ScOrderStatus"] : "");
                            detailViewModel.ZincCode = Convert.ToString(dataRow["order_code"] != DBNull.Value ? dataRow["order_code"] : "");
                            detailViewModel.ZincMessage = Convert.ToString(dataRow["order_message"] != DBNull.Value ? dataRow["order_message"] : "");
                            detailViewModel.PaymentStatus = Convert.ToString(dataRow["payment_staus"] != DBNull.Value ? dataRow["payment_staus"] : "");
                            if (detailViewModel.ZincMessage != "")
                            {
                                if (detailViewModel.ZincMessage.Contains("Guaranteed delivery date"))
                                {
                                    detailViewModel.ZincMessage = detailViewModel.ZincMessage.Replace("Guaranteed delivery date", "Delivery Date");
                                }
                            }
                            detailViewModel.ZincOrderLogID = Convert.ToString(dataRow["zinc_order_log_id"] != DBNull.Value ? dataRow["zinc_order_log_id"] : "0");
                            detailViewModel.ZincRequestID = Convert.ToString(dataRow["request_id"] != DBNull.Value ? dataRow["request_id"] : "");
                            detailViewModel.ZincOrderLogDetailID = Convert.ToString(dataRow["zinc_order_log_detail_id"] != DBNull.Value ? dataRow["zinc_order_log_detail_id"] : "");
                            detailViewModel.ZincOrderStatusInternal = Convert.ToString(dataRow["zinc_order_status_internal"] != DBNull.Value ? dataRow["zinc_order_status_internal"] : "");
                            detailViewModel.IsTrackingUpdateToSC = Convert.ToBoolean(dataRow["is_tracking_updated"] != DBNull.Value ? dataRow["is_tracking_updated"] : false);
                            detailViewModel.DropshipStatus = Convert.ToBoolean(dataRow["dropship_status"] != DBNull.Value ? dataRow["dropship_status"] : false);
                            detailViewModel.OnOrder = Convert.ToInt32(dataRow["OnOrder"] != DBNull.Value ? dataRow["OnOrder"] : 0);
                            detailViewModel.DropshipQty = Convert.ToInt32(dataRow["dropship_Qty"] != DBNull.Value ? dataRow["dropship_Qty"] : "0");
                            detailViewModel.BestBuyPorductID = Convert.ToString(dataRow["bb_product_ID"] != DBNull.Value ? dataRow["bb_product_ID"] : "0");

                            //calculations
                            detailViewModel.calculation_TotalAmountOfUnitPrice = detailViewModel.TotalQuantity * detailViewModel.UnitPrice;
                            detailViewModel.calculation_TotalTax = detailViewModel.TaxGST + detailViewModel.TaxPST;
                            detailViewModel.calculation_TotalTacPercentage = Math.Round((detailViewModel.calculation_TotalTax / detailViewModel.calculation_TotalAmountOfUnitPrice) * 100, 2);
                            //detailViewModel.calculation_Comission = Math.Round((detailViewModel.Comission) / (1 + detailViewModel.calculation_TotalTacPercentage / 100), 2);
                            detailViewModel.calculation_Comission = Math.Round((detailViewModel.Comission), 2);
                            detailViewModel.caculation_TotalAvgCost = Math.Round(Math.Round(Convert.ToDecimal(detailViewModel.AverageCost), 2) * detailViewModel.TotalQuantity, 2);
                            detailViewModel.calculation_SumTotal = Math.Round(detailViewModel.calculation_TotalTax + detailViewModel.calculation_TotalAmountOfUnitPrice, 2);
                            detailViewModel.calculation_comissionPercentage = Math.Round(((detailViewModel.calculation_Comission / detailViewModel.calculation_TotalAmountOfUnitPrice) * 100), 2);
                            detailViewModel.calculation_ProfitLoss = Math.Round(detailViewModel.calculation_TotalAmountOfUnitPrice - 0 - detailViewModel.caculation_TotalAvgCost - detailViewModel.calculation_Comission, 2);
                            detailViewModel.calculation_ProfitLossPercentage = Math.Round((detailViewModel.calculation_ProfitLoss / detailViewModel.calculation_TotalAmountOfUnitPrice) * 100, 2);


                            //List<ProductWarehouseQtyViewModel> warehouseQty = dataAccess.GetProductQtyBySKU_ForOrdersPage(detailViewModel.ProductSKU, conn);
                            List<ProductWarehouseQtyViewModel> warehouseQty = dataAccess.GetWareHousesQtyList(detailViewModel.ProductSKU);
                            detailViewModel.ProductrWarehouseQtyViewModel = warehouseQty;
                            objList.Add(detailViewModel);



                            //SendNewOrderEmail email = new SendNewOrderEmail();
                            //ViewModel.BBProductDetail = objList;
                            //List<BestBuyOrdersViewModel> mode = new List<BestBuyOrdersViewModel>();

                            //mode.Add(ViewModel);
                            //email.SendrderEmail(mode);
                        }
                        //if (ViewModel.OrderNumber != string.Empty)
                        //{
                        //    ViewModel.BBProductDetail = GetAllBestBuyOrdersDetail(ViewModel.OrderNumber, connStr);
                        //    // ViewModel.BBProductDetail = null;
                        //}


                        ViewModel.BBProductDetail = objList;

                        listBBProductViewModel.Add(ViewModel);
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return listBBProductViewModel;
        }

        public List<BestBuyOrdersViewModel> GetAllBestBuyOrdersWithGlobalFilterSellerCloudID(string FilterName, string FilterValue)
        {
            List<BestBuyOrdersViewModel> listBBProductViewModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetBestBuyOrdersWithGlobalFiltersSellerCloudID", conn);
                    cmd.Parameters.AddWithValue("FilterName", FilterName);
                    cmd.Parameters.AddWithValue("FilterValue", FilterValue);
                    //MySqlConnection mySqlConnection = new MySqlConnection(connStr);
                    //mySqlConnection.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            listBBProductViewModel = new List<BestBuyOrdersViewModel>();
                            while (reader.Read())
                            {
                                BestBuyOrdersViewModel ViewModel = new BestBuyOrdersViewModel();
                                ViewModel.OrderNumber = Convert.ToString(reader["order_id"] != DBNull.Value ? reader["order_id"] : string.Empty);
                                ViewModel.SellerCloudOrderID = Convert.ToString(reader["sellerCloudID"] != DBNull.Value ? reader["sellerCloudID"] : string.Empty);
                                ViewModel.ShipmentAddress = Convert.ToString(reader["CustomerAddress"] != DBNull.Value ? reader["CustomerAddress"] : string.Empty);
                                ViewModel.OrderDate = Convert.ToDateTime(reader["acceptance_decision_date"] != DBNull.Value ? reader["acceptance_decision_date"] : (DateTime?)null);
                                ViewModel.ShipmentAddress = Convert.ToString(reader["CustomerAddress"] != DBNull.Value ? reader["CustomerAddress"] : string.Empty);
                                ViewModel.TotalTax = Convert.ToDecimal(reader["TotalTax"] != DBNull.Value ? reader["TotalTax"] : "0");
                                ViewModel.TotalQuantity = Convert.ToInt32(reader["totalQuantity"] != DBNull.Value ? reader["totalQuantity"] : "0");
                                ViewModel.TotalComission = Convert.ToDecimal(reader["OrderLineComission"] != DBNull.Value ? reader["OrderLineComission"] : "0");
                                ViewModel.TotalPrice = Convert.ToDecimal(reader["totalPrice"] != DBNull.Value ? reader["totalPrice"] : "0");

                                if (ViewModel.OrderNumber != string.Empty)
                                {
                                    ViewModel.BBProductDetail = GetAllBestBuyOrdersDetail(ViewModel.OrderNumber, connStr);
                                }
                                listBBProductViewModel.Add(ViewModel);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return listBBProductViewModel;
        }

        public bool UpdateBestBuyProductFromBestBuyOffer(BBProductViewModel viewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_UpdateBestBuyProductFromBestBuyOffer", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_discount_end_date", viewModel.DiscountEndDate);
                    cmd.Parameters.AddWithValue("_discount_start_date", viewModel.DiscountStartDate);
                    cmd.Parameters.AddWithValue("_unit_discount_price_selling_price", viewModel.UnitDiscountPrice_SellingPrice);
                    cmd.Parameters.AddWithValue("_unit_origin_price_MSRP", viewModel.UnitOriginPrice_MSRP);
                    cmd.Parameters.AddWithValue("_category_code", viewModel.CategoryCode);
                    cmd.Parameters.AddWithValue("_category_name", viewModel.CategoryName);
                    cmd.Parameters.AddWithValue("_logistic_class_code", viewModel.LogisticClass_Code);
                    cmd.Parameters.AddWithValue("_product_sku", viewModel.Product_Sku);
                    cmd.Parameters.AddWithValue("_shop_sku_offer_sku", viewModel.ShopSKU_OfferSKU);
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }


        public List<BestBuyUpdateViewModel> GetBestBuyUpdate(int offset)
        {
            List<BestBuyUpdateViewModel> listModel = null;
            // List<ZincWatchListSummaryViewModal> listModel = new List<ZincWatchListSummaryViewModal>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmdd = new MySqlCommand(@"SELECT * FROM bestBuyE2.BestBuyUpdateJob ORDER BY JobId desc limit 25 offset "+ offset ,conn);
                    cmdd.CommandType = System.Data.CommandType.Text;
                    MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(cmdd);
                    DataTable dt = new DataTable();
                    mySqlDataAdapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        listModel = new List<BestBuyUpdateViewModel>();
                        foreach (DataRow dr in dt.Rows)
                        {
                            BestBuyUpdateViewModel model = new BestBuyUpdateViewModel();
                            model.JobID = Convert.ToInt32(dr["JobId"] != DBNull.Value ? dr["JobId"] : "0");
                            model.StartTime = Convert.ToDateTime(dr["StartTime"] != DBNull.Value ? dr["StartTime"] : DateTime.MinValue);
                            model.CompletionTime = Convert.ToDateTime(dr["CompleteTime"] != DBNull.Value ? dr["CompleteTime"] : DateTime.MinValue);
                            model.TotalProduct = Convert.ToInt32(dr["TotalProduct"] != DBNull.Value ? dr["TotalProduct"] : "0");
                            listModel.Add(model);
                        }
                    }
                }
                return listModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetWatchlistSummaryCountupdate()
        {
            int count = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(@"SELECT count(JobID) AS Records FROM bestBuyE2.BestBuyUpdateJob ;", conn);
                    cmd.CommandType = System.Data.CommandType.Text;
                    count = Convert.ToInt32(cmd.ExecuteScalar());
                    conn.Close();
                }
                return count;
            }
            catch (Exception exp)
            {
                throw;
            }
        }

        public GetExplainAmountViewModel GetExplainAmount(string sellercloudId, string productSku)
        {
            GetExplainAmountViewModel listBBProductViewModel = new GetExplainAmountViewModel();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetExplainAmount", conn);
                    cmd.Parameters.AddWithValue("_sellercloudId", sellercloudId);
                    cmd.Parameters.AddWithValue("_productSku", productSku);
                    //MySqlConnection mySqlConnection = new MySqlConnection(connStr);
                    //mySqlConnection.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                          
                            while (reader.Read())
                            {
                                GetExplainAmountViewModel ViewModel = new GetExplainAmountViewModel();
                                ViewModel.quantity = Convert.ToInt32(reader["quantity"] != DBNull.Value ? reader["quantity"] : "");
                                ViewModel.ShippingFee = Convert.ToDecimal(reader["ShippingFee"] != DBNull.Value ? reader["ShippingFee"] : 0);
                                ViewModel.unitprice = Convert.ToDecimal(reader["total_price"] != DBNull.Value ? reader["total_price"] : 0);
                                ViewModel.unitprice = Math.Round((ViewModel.unitprice - ViewModel.ShippingFee) / ViewModel.quantity, 2);
                                ViewModel.avg_cost = Convert.ToDecimal(reader["avg_cost"] != DBNull.Value ? reader["avg_cost"] : 0);
                                ViewModel.total_commission = Convert.ToDecimal(reader["total_commission"] != DBNull.Value ? reader["total_commission"] : 0);
                                listBBProductViewModel=ViewModel;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return listBBProductViewModel;
        }

    }
}
