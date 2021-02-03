using DataAccess.Helper;
using DataAccess.ViewModels;
using Hld.WebApplication.ViewModel;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataAccess
{
   public class BestBuyOrdersDataAccessNew
    {
        public string connStr { get; set; }
        ProductWarehouseQtyDataAccess dataAccess = null;
        ApprovedPriceDataAccess ApprovedPriceDataAccess = null;
        ZincDataAccess _zincDataAccess = null;
        TagDataAccess _tagDataAccess = null;
        public BestBuyOrdersDataAccessNew(IConnectionString connectionString)
        {
            connStr = connectionString.GetConnectionString();
            dataAccess = new ProductWarehouseQtyDataAccess(connectionString);
            ApprovedPriceDataAccess = new ApprovedPriceDataAccess(connectionString);
            _zincDataAccess = new ZincDataAccess(connectionString);
            _tagDataAccess = new TagDataAccess(connectionString);
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
                throw ex;
            }
            return model;
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
                    MySqlCommand cmd = new MySqlCommand("p_GetBBOrders_SearchTotalCount", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("zincOrderStatus", getString(viewModel.ZincStatus));
                    cmd.Parameters.AddWithValue("sort", viewModel.Sort);
                    cmd.Parameters.AddWithValue("OrderStatus", viewModel.OrderStatus);
                    cmd.Parameters.AddWithValue("sku", viewModel.Sku);
                    cmd.Parameters.AddWithValue("dateFrom", viewModel.DateFrom);
                    cmd.Parameters.AddWithValue("dateTo", viewModel.DateTo);
                    cmd.Parameters.AddWithValue("DSStatus", viewModel.DSStatus);
                    cmd.Parameters.AddWithValue("TagSearch", viewModel.ShippingTags);
                    cmd.Parameters.AddWithValue("PaymentStatus", viewModel.PaymentStatus);
                    cmd.Parameters.AddWithValue("_ShippingBoxContain", viewModel.ShippingBoxContain);
                    cmd.Parameters.AddWithValue("_wHQStatus", viewModel.WHQStatus);
                    cmd.Parameters.AddWithValue("_bBOrderStatus", getString(viewModel.BBOrderStatus));


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
        public List<BestBuyOrdersViewModel> GetAllBestBuyOrdersWithGlobalFilter(string FilterName, string FilterValue, int startLimit, int endLimit, string sort)
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

                        var list = dt.AsEnumerable().Where(e => e.Field<Int32?>("sellerCloudID").ToString() == reader["sellerCloudID"].ToString()).ToList();


                        var totalQuantity = list.Sum(e => Convert.ToDouble(e.Field<string>("quantity")));
                        
                        var totalPrice = list.Sum(e => Convert.ToDouble(e.Field<double>("totalPrice")));
                        var total_commission = list.Sum(e => Convert.ToDouble(e.Field<decimal>("total_commission")));
                        var totalGst = list.Sum(e => Convert.ToDouble(e.Field<string>("TaxGST")));
                        var totalPst = list.Sum(e => Convert.ToDouble(e.Field<string>("TaxPST")));
                        var toalAverageCost = list.Sum(e => e.Field<double>("TotalAverageCost"));

                        var sumOfGstPst = totalGst + totalPst;

                        var total = (sumOfGstPst / totalPrice) * 100;

                        //var totalComission = Math.Round((total_commission) / (1 + total / 100), 2);
                        var totalComission = total_commission;


                        BestBuyOrdersViewModel ViewModel = new BestBuyOrdersViewModel();
                        ViewModel.OrderNumber = Convert.ToString(list.Select(e => e.Field<string>("order_id")).FirstOrDefault());
                        ViewModel.SellerCloudOrderID = Convert.ToString(list.Select(e => e.Field<Int32?>("sellerCloudID")).FirstOrDefault());

                        ViewModel.ShipmentAddress = Convert.ToString(list.Select(e => e.Field<string>("CustomerAddress")).FirstOrDefault());
                        ViewModel.OrderDate = Convert.ToDateTime(list.Select(e => e.Field<DateTime?>("acceptance_decision_date")).FirstOrDefault());
                        ViewModel.ParentOrderID = Convert.ToInt32(list.Select(e => e.Field<int?>("ParentOrderID")).FirstOrDefault() != null ? list.Select(e => e.Field<int>("ParentOrderID")).FirstOrDefault() : 0).ToString();
                        ViewModel.IsParent = Convert.ToString(list.Select(e => e.Field<string>("IsParent")).FirstOrDefault());
                        ViewModel.IsNotes = Convert.ToString(list.Select(e => e.Field<string>("IsNotes")).FirstOrDefault());
                        ViewModel.ShippingPrice = Convert.ToString(list.Select(e => e.Field<string>("ShippingPaidByCustomer")).FirstOrDefault());

                        var ProfitLoss = Math.Round((totalPrice + Convert.ToDouble(ViewModel.ShippingPrice) )- toalAverageCost - total_commission, 2);
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
                           // detailViewModel.UnitPrice = Math.Round((detailViewModel.UnitPrice) / detailViewModel.TotalQuantity, 2);
                              detailViewModel.OrderDetailID = Convert.ToInt32(dataRow["bbe2_line_id"] != DBNull.Value ? dataRow["bbe2_line_id"] : "0");
                            detailViewModel.TaxGST = Convert.ToDecimal(dataRow["TaxGST"] != DBNull.Value ? dataRow["TaxGST"] : "0");
                            detailViewModel.TaxPST = Convert.ToDecimal(dataRow["TaxPST"] != DBNull.Value ? dataRow["TaxPST"] : "0");
                            detailViewModel.SCOrderStatus = Convert.ToString(dataRow["ScOrderStatus"] != DBNull.Value ? dataRow["ScOrderStatus"] : "");
                            detailViewModel.ZincCode = Convert.ToString(dataRow["Zinc_order_code"] != DBNull.Value ? dataRow["Zinc_order_code"] : "");
                            detailViewModel.ZincMessage = Convert.ToString(dataRow["Zinc_order_message"] != DBNull.Value ? dataRow["Zinc_order_message"] : "");
                            detailViewModel.PaymentStatus = Convert.ToString(dataRow["payment_staus"] != DBNull.Value ? dataRow["payment_staus"] : "");
                            if (detailViewModel.ZincMessage != "")
                            {
                                if (detailViewModel.ZincMessage.Contains("Guaranteed delivery date"))
                                {
                                    detailViewModel.ZincMessage = detailViewModel.ZincMessage.Replace("Guaranteed delivery date", "Delivery Date");
                                }
                            }
                        //    detailViewModel.ZincOrderLogID = Convert.ToString(dataRow["zinc_order_log_id"] != DBNull.Value ? dataRow["zinc_order_log_id"] : "0");
                            detailViewModel.ZincRequestID = Convert.ToString(dataRow["request_id"] != DBNull.Value ? dataRow["request_id"] : "");
                        //    detailViewModel.ZincOrderLogDetailID = Convert.ToString(dataRow["zinc_order_log_detail_id"] != DBNull.Value ? dataRow["zinc_order_log_detail_id"] : "");
                            detailViewModel.ZincOrderStatusInternal = Convert.ToString(dataRow["zinc_order_status_internal"] != DBNull.Value ? dataRow["zinc_order_status_internal"] : "");
                            detailViewModel.IsTrackingUpdateToSC = Convert.ToBoolean(dataRow["is_tracking_updated"] != DBNull.Value ? dataRow["is_tracking_updated"] : false);
                            detailViewModel.DropshipStatus = Convert.ToBoolean(dataRow["dropship_status"] != DBNull.Value ? dataRow["dropship_status"] : false);
                            detailViewModel.OnOrder = Convert.ToInt32(dataRow["OnOrder"] != DBNull.Value ? dataRow["OnOrder"] : 0);
                            detailViewModel.DropshipQty = Convert.ToInt32(dataRow["dropship_Qty"] != DBNull.Value ? dataRow["dropship_Qty"] : "0");
                            detailViewModel.BestBuyPorductID = Convert.ToString(dataRow["bb_product_ID"] != DBNull.Value ? dataRow["bb_product_ID"] : "0");
                            detailViewModel.WarehouseQuantity = Convert.ToString(dataRow["AggregatedQty"] != DBNull.Value ? dataRow["AggregatedQty"] : "0");
                            //calculations
                            detailViewModel.calculation_TotalAmountOfUnitPrice = detailViewModel.TotalQuantity * detailViewModel.UnitPrice;
                            detailViewModel.calculation_TotalTax = detailViewModel.TaxGST + detailViewModel.TaxPST;
                            detailViewModel.calculation_TotalTacPercentage = Math.Round((detailViewModel.calculation_TotalTax / detailViewModel.calculation_TotalAmountOfUnitPrice) * 100, 2);
                            //detailViewModel.calculation_Comission = Math.Round((detailViewModel.Comission) / (1 + detailViewModel.calculation_TotalTacPercentage / 100), 2);
                            detailViewModel.calculation_Comission = Math.Round((detailViewModel.Comission), 2);
                            detailViewModel.caculation_TotalAvgCost = Math.Round(Math.Round(Convert.ToDecimal(detailViewModel.AverageCost), 2) * detailViewModel.TotalQuantity, 2);
                            detailViewModel.calculation_SumTotal = Math.Round(detailViewModel.calculation_TotalTax + detailViewModel.calculation_TotalAmountOfUnitPrice, 2);
                            detailViewModel.calculation_comissionPercentage = Math.Round(((detailViewModel.calculation_Comission / detailViewModel.calculation_TotalAmountOfUnitPrice) * 100), 2);
                            detailViewModel.calculation_ProfitLoss = Math.Round((detailViewModel.calculation_TotalAmountOfUnitPrice + detailViewModel.ShippingFee) - detailViewModel.caculation_TotalAvgCost - detailViewModel.calculation_Comission, 2);
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
        public List<BestBuyOrdersViewModel> GetAllBestBuyOrdersSearch(BestBuyOrderSearchTotalCountViewModel viewModel)
        {

            List<BestBuyOrdersViewModel> listBBProductViewModel = null;
            try
            
            { 
                System.Data.DataSet ds = new System.Data.DataSet();
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {

                    MySqlCommand cmd = new MySqlCommand("P_forTestingPurposeDumyCopy", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("zincOrderStatus", getString(viewModel.ZincStatus) );
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
                    cmd.Parameters.AddWithValue("_wHQStatus", viewModel.WHQStatus);
                    cmd.Parameters.AddWithValue("_bBOrderStatus", getString(viewModel.BBOrderStatus));

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(ds);
                    listBBProductViewModel = new List<BestBuyOrdersViewModel>();
                    System.Data.DataView dataView = new System.Data.DataView(ds.Tables[0]);
                    System.Data.DataTable distinctValue = dataView.ToTable(true, "order_id");
                    DataTable dt = ds.Tables[0];
                    foreach (System.Data.DataRow reader in distinctValue.Rows)
                    {
                        List<BestBuyOrderDetailViewModel> objList = new List<BestBuyOrderDetailViewModel>();


                        //var list = dt.AsEnumerable().Where(e => e.Field<Int32?>("sellerCloudID").ToString() == reader["sellerCloudID"].ToString()).ToList();

                        var list = dt.AsEnumerable().Where(e => e.Field<string>("order_id").ToString() == reader["order_id"].ToString()).ToList();


                        //var totalQuantity = list.Sum(e => Convert.ToDecimal(e.Field<string>("quantity")));
                        //var totalPrice = list.Sum(e => Convert.ToDouble(e.Field<double>("totalPrice")));
                        //var total_commission = list.Sum(e => Convert.ToDecimal(e.Field<decimal>("total_commission")));
                        //var totalGst = list.Sum(e => Convert.ToDecimal(e.Field<string>("TaxGST")));
                        //var totalPst = list.Sum(e => Convert.ToDecimal(e.Field<string>("TaxPST")));
                        //var toalAverageCost = list.Sum(e => e.Field<double>("TotalAverageCost"));

                        var totalQuantity = list.Sum(e => Convert.ToDouble(e.Field<string>("quantity")));

                        var totalPrice = list.Sum(e => Convert.ToDouble(e.Field<double>("totalPrice")));
                        var total_commission = list.Sum(e => Convert.ToDouble(e.Field<decimal>("total_commission")));
                        var totalGst = list.Sum(e => Convert.ToDouble(e.Field<string>("TaxGST")));
                        var totalPst = list.Sum(e => Convert.ToDouble(e.Field<string>("TaxPST")));
                        var toalAverageCost = list.Sum(e => e.Field<double>("TotalAverageCost"));
                        var sumOfGstPst = totalGst + totalPst;

                        var total =(Convert.ToDecimal(sumOfGstPst) / Convert.ToDecimal( totalPrice)) * 100;

                        //var totalComission = Math.Round((total_commission) / (1 + total / 100), 2);
                        var totalComission = total_commission;
                    
                        BestBuyOrdersViewModel ViewModel = new BestBuyOrdersViewModel();
                        ViewModel.OrderNumber = Convert.ToString(list.Select(e => e.Field<string>("order_id")).FirstOrDefault());

                        ViewModel.SellerCloudOrderID = Convert.ToString(list.Select(e => e.Field<Int32?>("sellerCloudID")).FirstOrDefault());

                        //ViewModel.SellerCloudOrderID = Convert.ToInt32(list.Select(e => e.Field<int?>("sellerCloudID")).FirstOrDefault() != null ? list.Select(e => e.Field<int>("sellerCloudID")).FirstOrDefault() : 0).ToString();
               
                        ViewModel.ShipmentAddress = Convert.ToString(list.Select(e => e.Field<string>("CustomerAddress")).FirstOrDefault());
                        ViewModel.OrderDate = Convert.ToDateTime(list.Select(e => e.Field<DateTime?>("acceptance_decision_date")).FirstOrDefault());
                        ViewModel.ParentOrderID = Convert.ToInt32(list.Select(e => e.Field<int?>("ParentOrderID")).FirstOrDefault() != null ? list.Select(e => e.Field<int>("ParentOrderID")).FirstOrDefault() : 0).ToString();
                        ViewModel.IsParent = Convert.ToString(list.Select(e => e.Field<string>("IsParent")).FirstOrDefault());
                        ViewModel.IsNotes = Convert.ToString(list.Select(e => e.Field<string>("IsNotes")).FirstOrDefault());
                        ViewModel.ShippingPrice = Convert.ToString(list.Select(e => e.Field<string>("ShippingPaidByCustomer")).FirstOrDefault());

                        var ProfitLoss = Math.Round((totalPrice + Convert.ToDouble(ViewModel.ShippingPrice)) - toalAverageCost - total_commission, 2);
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

                            detailViewModel.WarehouseQuantity = Convert.ToString(dataRow["AggregatedQty"] != DBNull.Value ? dataRow["AggregatedQty"] : "0");

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
                            //detailViewModel.UnitPrice = Math.Round((detailViewModel.UnitPrice) / detailViewModel.TotalQuantity, 2);
                            detailViewModel.OrderDetailID = Convert.ToInt32(dataRow["bbe2_line_id"] != DBNull.Value ? dataRow["bbe2_line_id"] : "0");
                            detailViewModel.TaxGST = Convert.ToDecimal(dataRow["TaxGST"] != DBNull.Value ? dataRow["TaxGST"] : "0");
                            detailViewModel.TaxPST = Convert.ToDecimal(dataRow["TaxPST"] != DBNull.Value ? dataRow["TaxPST"] : "0");
                            detailViewModel.SCOrderStatus = Convert.ToString(dataRow["ScOrderStatus"] != DBNull.Value ? dataRow["ScOrderStatus"] : "");
                            detailViewModel.ZincCode = Convert.ToString(dataRow["Zinc_order_code"] != DBNull.Value ? dataRow["Zinc_order_code"] : "");
                            detailViewModel.ZincMessage = Convert.ToString(dataRow["Zinc_order_message"] != DBNull.Value ? dataRow["Zinc_order_message"] : "");
                            detailViewModel.PaymentStatus = Convert.ToString(dataRow["payment_staus"] != DBNull.Value ? dataRow["payment_staus"] : "");
                            if (detailViewModel.ZincMessage != "")
                            {
                                if (detailViewModel.ZincMessage.Contains("Guaranteed delivery date"))
                                {
                                    detailViewModel.ZincMessage = detailViewModel.ZincMessage.Replace("Guaranteed delivery date", "Delivery Date");
                                }
                            }
                      //      detailViewModel.ZincOrderLogID = Convert.ToString(dataRow["zinc_order_log_id"] != DBNull.Value ? dataRow["zinc_order_log_id"] : "0");
                            detailViewModel.ZincRequestID = Convert.ToString(dataRow["request_id"] != DBNull.Value ? dataRow["request_id"] : "");
                      //      detailViewModel.ZincOrderLogDetailID = Convert.ToString(dataRow["zinc_order_log_detail_id"] != DBNull.Value ? dataRow["zinc_order_log_detail_id"] : "");
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
                            detailViewModel.calculation_ProfitLoss = Math.Round((detailViewModel.calculation_TotalAmountOfUnitPrice + detailViewModel.ShippingFee) - detailViewModel.caculation_TotalAvgCost - detailViewModel.calculation_Comission, 2);
                            detailViewModel.calculation_ProfitLossPercentage = Math.Round((detailViewModel.calculation_ProfitLoss / detailViewModel.calculation_TotalAmountOfUnitPrice) * 100, 2);


                            ////List<ProductWarehouseQtyViewModel> warehouseQty = dataAccess.GetProductQtyBySKU_ForOrdersPage(detailViewModel.ProductSKU, conn);
                            //List<ProductWarehouseQtyViewModel> warehouseQty = dataAccess.GetWareHousesQtyList(detailViewModel.ProductSKU);
                            //detailViewModel.ProductrWarehouseQtyViewModel = warehouseQty;
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
        public List<BestBuyOrdersViewModel> GetAllBestBuyOrders(int startLimit, int endLimit, string sort)
        {
            MySqlConnection mySqlConnection = null;
            List<BestBuyOrdersViewModel> listBBProductViewModel = new List<BestBuyOrdersViewModel>();
            try
            {
                System.Data.DataSet ds = new System.Data.DataSet();
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {

                    MySqlCommand cmd = new MySqlCommand("p_GetBestBuyOrdersNew", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("startLimit", startLimit);
                    cmd.Parameters.AddWithValue("endLimit", endLimit);
                    cmd.Parameters.AddWithValue("sort", sort);


                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(ds);
                    listBBProductViewModel = new List<BestBuyOrdersViewModel>();
                    System.Data.DataView dataView = new System.Data.DataView(ds.Tables[0]);
                    System.Data.DataTable distinctValue = dataView.ToTable(true, "order_id");
                    DataTable dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        mySqlConnection = new MySqlConnection(connStr);
                        mySqlConnection.Open();
                    }
                    foreach (System.Data.DataRow reader in distinctValue.Rows)
                    {
                        List<BestBuyOrderDetailViewModel> objList = new List<BestBuyOrderDetailViewModel>();




                        var list = dt.AsEnumerable().Where(e => e.Field<string>("order_id").ToString() == reader["order_id"].ToString()).ToList();


                        var totalQuantity = list.Sum(e => Convert.ToDouble(e.Field<string>("quantity")));
                        var totalPrice = list.Sum(e => Convert.ToDouble(e.Field<string>("total_price")));
                        var total_commission = list.Sum(e => Convert.ToDouble(e.Field<decimal>("total_commission")));
                        var totalGst = list.Sum(e => Convert.ToDouble(e.Field<string>("TaxGST")));
                        var totalPst = list.Sum(e => Convert.ToDouble(e.Field<string>("TaxPST")));
                        var toalAverageCost = list.Sum(e => e.Field<double>("TotalAverageCost"));


                        var sumOfGstPst = totalGst + totalPst;

                        var total = (sumOfGstPst / totalPrice) * 100;

                      


                        BestBuyOrdersViewModel ViewModel = new BestBuyOrdersViewModel();
                        ViewModel.OrderNumber = Convert.ToString(list.Select(e => e.Field<string>("order_id")).FirstOrDefault());
                        ViewModel.SellerCloudOrderID = Convert.ToString(list.Select(e => e.Field<Int32?>("sellerCloudID")).FirstOrDefault());
                        ViewModel.ParentOrderID = Convert.ToInt32(list.Select(e => e.Field<int?>("ParentOrderID")).FirstOrDefault() != null ? list.Select(e => e.Field<int>("ParentOrderID")).FirstOrDefault() : 0).ToString();
                        ViewModel.IsParent = Convert.ToString(list.Select(e => e.Field<string>("IsParent")).FirstOrDefault());
                        ViewModel.IsNotes = Convert.ToString(list.Select(e => e.Field<string>("IsNotes")).FirstOrDefault());
                        ViewModel.ShippingPrice = Convert.ToString(list.Select(e => e.Field<string>("ShippingPaidByCustomer")).FirstOrDefault());

                        ViewModel.ShipmentAddress = Convert.ToString(list.Select(e => e.Field<string>("CustomerAddress")).FirstOrDefault());
                        ViewModel.OrderDate = Convert.ToDateTime(list.Select(e => e.Field<DateTime?>("acceptance_decision_date")).FirstOrDefault());
                        var ProfitLoss = Math.Round((totalPrice + Convert.ToDouble(ViewModel.ShippingPrice)) - toalAverageCost - total_commission, 2);
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
                            //detailViewModel.OrderStatus = Convert.ToString(dataRow["order_line_state"] != DBNull.Value ? dataRow["order_line_state"] : string.Empty);
                            detailViewModel.OrderStatus = Convert.ToString(dataRow["order_state"] != DBNull.Value ? dataRow["order_state"] : string.Empty);
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
                            detailViewModel.UnitPrice = Math.Round((detailViewModel.UnitPrice) / detailViewModel.TotalQuantity, 2);
                            detailViewModel.OrderDetailID = Convert.ToInt32(dataRow["bbe2_line_id"] != DBNull.Value ? dataRow["bbe2_line_id"] : "0");
                            detailViewModel.TaxGST = Convert.ToDecimal(dataRow["TaxGST"] != DBNull.Value ? dataRow["TaxGST"] : "0");
                            detailViewModel.TaxPST = Convert.ToDecimal(dataRow["TaxPST"] != DBNull.Value ? dataRow["TaxPST"] : "0");
                            detailViewModel.WarehouseQuantity = Convert.ToString(dataRow["AggregatedQty"] != DBNull.Value ? dataRow["AggregatedQty"] : "0");
                            detailViewModel.SCOrderStatus = Convert.ToString(dataRow["ScOrderStatus"] != DBNull.Value ? dataRow["ScOrderStatus"] : "");
                            detailViewModel.ZincCode = Convert.ToString(dataRow["Zinc_order_code"] != DBNull.Value ? dataRow["Zinc_order_code"] : "");
                            detailViewModel.ZincMessage = Convert.ToString(dataRow["Zinc_order_message"] != DBNull.Value ? dataRow["Zinc_order_message"] : "");
                            detailViewModel.PaymentStatus = Convert.ToString(dataRow["payment_staus"] != DBNull.Value ? dataRow["payment_staus"] : "");
                            if (detailViewModel.ZincMessage != "")
                            {
                                if (detailViewModel.ZincMessage.Contains("Guaranteed delivery date"))
                                {
                                    detailViewModel.ZincMessage = detailViewModel.ZincMessage.Replace("Guaranteed delivery date", "Delivery Date");
                                }
                            }
                            //detailViewModel.ZincOrderLogID = Convert.ToString(dataRow["zinc_order_log_id"] != DBNull.Value ? dataRow["zinc_order_log_id"] : "0");
                            detailViewModel.ZincRequestID = Convert.ToString(dataRow["request_id"] != DBNull.Value ? dataRow["request_id"] : "");
                            //detailViewModel.ZincOrderLogDetailID = Convert.ToString(dataRow["zinc_order_log_detail_id"] != DBNull.Value ? dataRow["zinc_order_log_detail_id"] : "");
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
                            detailViewModel.calculation_ProfitLoss = Math.Round((detailViewModel.calculation_TotalAmountOfUnitPrice + detailViewModel.ShippingFee) - detailViewModel.caculation_TotalAvgCost - detailViewModel.calculation_Comission, 2);
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
                throw ex;
            }
            return listBBProductViewModel;

        }
        public BestBuyOrdersViewPageModel SCOrderPageViewOrderDetails(string bbOrderId)
        {
            BestBuyOrdersViewPageModel BBProductViewModel = null;
            try
            {
                System.Data.DataSet ds = new System.Data.DataSet();
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {

                    MySqlCommand cmd = new MySqlCommand("P_GetSCOrderForOrderPageView", conn);
                //    MySqlCommand cmd = new MySqlCommand("P_GetSCOrderForOrderPageViewNew", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_SourceOrderID", bbOrderId);

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(ds);
                    BBProductViewModel = new BestBuyOrdersViewPageModel();
                    System.Data.DataView dataView = new System.Data.DataView(ds.Tables[0]);
                    System.Data.DataTable distinctValue = dataView.ToTable(true, "order_id");
                    DataTable dt = ds.Tables[0];
                    foreach (System.Data.DataRow reader in distinctValue.Rows)
                    {
                        List<BestBuyOrderDetailViewModel> objList = new List<BestBuyOrderDetailViewModel>();

                        var list = dt.AsEnumerable().Where(e => e.Field<string>("order_id").ToString() == reader["order_id"].ToString()).ToList();
                        var shipping = Convert.ToDouble(list.Select(e => e.Field<string>("ShippingPaidByCustomer")).FirstOrDefault());
                        var totalQuantity = list.Sum(e => Convert.ToDouble(e.Field<string>("quantity")));

                        var totalPrice = list.Sum(e => Convert.ToDouble(e.Field<double>("totalPrice")));
                        var total_commission = list.Sum(e => Convert.ToDouble(e.Field<decimal>("total_commission")));
                        var totalGst = list.Sum(e => Convert.ToDouble(e.Field<string>("TaxGST")));
                        var totalPst = list.Sum(e => Convert.ToDouble(e.Field<string>("TaxPST")));
                        var toalAverageCost = list.Sum(e => e.Field<double>("TotalAverageCost"));
                        var ProfitLoss = Math.Round((totalPrice+ shipping) - toalAverageCost - total_commission, 2);
                        var profitAndLossInPercent = Math.Round((ProfitLoss / totalPrice) * 100);

                        BBProductViewModel.OrderNumber = Convert.ToString(list.Select(e => e.Field<string>("order_id")).FirstOrDefault());

                        BBProductViewModel.SellerCloudOrderID = Convert.ToString(list.Select(e => e.Field<Int32?>("sellerCloudID")).FirstOrDefault());

                        BBProductViewModel.CustomerName = Convert.ToString(list.Select(e => e.Field<string>("CustomerName")).FirstOrDefault());
                        BBProductViewModel.Street = Convert.ToString(list.Select(e => e.Field<string>("Street")).FirstOrDefault());
                        BBProductViewModel.State = Convert.ToString(list.Select(e => e.Field<string>("State")).FirstOrDefault());
                        BBProductViewModel.Country = Convert.ToString(list.Select(e => e.Field<string>("Country")).FirstOrDefault());
                        BBProductViewModel.OrderDate = Convert.ToDateTime(list.Select(e => e.Field<DateTime?>("acceptance_decision_date")).FirstOrDefault());
                        BBProductViewModel.ParentOrderID = Convert.ToInt32(list.Select(e => e.Field<int?>("ParentOrderID")).FirstOrDefault() != null ? list.Select(e => e.Field<int>("ParentOrderID")).FirstOrDefault() : 0).ToString();
                        BBProductViewModel.IsParent = Convert.ToString(list.Select(e => e.Field<string>("IsParent")).FirstOrDefault());
                        BBProductViewModel.IsNotes = Convert.ToString(list.Select(e => e.Field<string>("IsNotes")).FirstOrDefault());
                        BBProductViewModel.ShippingPrice = Convert.ToString(list.Select(e => e.Field<string>("ShippingPaidByCustomer")).FirstOrDefault());

                        BBProductViewModel.TotalTax = Convert.ToDecimal(totalGst + totalPst);
                        BBProductViewModel.TotalQuantity = Convert.ToInt32(totalQuantity);
                        BBProductViewModel.TotalComission = Convert.ToDecimal(total_commission);
                        BBProductViewModel.TotalPrice = Math.Round(Convert.ToDecimal(totalPrice), 2);
                        BBProductViewModel.ProfitAndLossInDollar = Convert.ToDecimal(ProfitLoss);
                        BBProductViewModel.ProfitAndLossInPercentage = Convert.ToDecimal(profitAndLossInPercent);
                        BBProductViewModel.TotalAverageCost = Convert.ToDecimal(toalAverageCost);
                        BBProductViewModel.tracking_number = Convert.ToString(list.Select(e => e.Field<string>("Zinc_tracking_number")).FirstOrDefault());
                        BBProductViewModel.PaymentStatus = Convert.ToString(list.Select(e => e.Field<string>("payment_staus")).FirstOrDefault());
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
                            detailViewModel.AverageCost = Convert.ToString(dataRow["avg_cost"] != DBNull.Value ? dataRow["avg_cost"] : "0");
                            detailViewModel.ProductTitle = Convert.ToString(dataRow["title"] != DBNull.Value ? dataRow["title"] : string.Empty);
                            detailViewModel.TotalQuantity = Convert.ToInt32(dataRow["quantity"] != DBNull.Value ? dataRow["quantity"] : "0");
                            detailViewModel.WarehouseQuantity = Convert.ToString("10");
                            if (dataRow["CreditCardId"] != DBNull.Value)
                            {
                                BBProductViewModel.CreditCardId = Convert.ToInt32(dataRow["CreditCardId"] != DBNull.Value ? dataRow["CreditCardId"] : 0);
                            }
                            if (dataRow["ZincAccountId"] != DBNull.Value)
                            {
                                BBProductViewModel.ZincAccountId = Convert.ToInt32(dataRow["ZincAccountId"] != DBNull.Value ? dataRow["ZincAccountId"] : 0);
                            }

                            List<ApprovedPriceViewModel> approvedPrices = new List<ApprovedPriceViewModel>();
                            approvedPrices = ApprovedPriceDataAccess.GetApprovedPricesList(1278, 30, 0, detailViewModel.ProductSKU, "", "");

                            detailViewModel.approvedPrices = approvedPrices.Where(s => s.PriceStatus == true).ToList();
                            detailViewModel.ZincASIN = Convert.ToString("");
                            detailViewModel.ZincLink = Convert.ToString("");
                            detailViewModel.ZincStatus = Convert.ToString(dataRow["drop_ship_status"] != DBNull.Value ? dataRow["drop_ship_status"] : "");
                            detailViewModel.ProfitLoss = "";
                            detailViewModel.Comission = Convert.ToDecimal(dataRow["total_commission"] != DBNull.Value ? dataRow["total_commission"] : "0");
                            detailViewModel.ShippingFee = Convert.ToDecimal(dataRow["ShippingFee"] != DBNull.Value ? dataRow["ShippingFee"] : 0);
                            detailViewModel.UnitPrice = Convert.ToDecimal(dataRow["total_price"] != DBNull.Value ? dataRow["total_price"] : "0");
                            detailViewModel.UnitPrice = Math.Round((detailViewModel.UnitPrice) / detailViewModel.TotalQuantity, 2);
                            detailViewModel.OrderDetailID = Convert.ToInt32(dataRow["bbe2_line_id"] != DBNull.Value ? dataRow["bbe2_line_id"] : "0");
                            detailViewModel.TaxGST = Convert.ToDecimal(dataRow["TaxGST"] != DBNull.Value ? dataRow["TaxGST"] : "0");
                            detailViewModel.TaxPST = Convert.ToDecimal(dataRow["TaxPST"] != DBNull.Value ? dataRow["TaxPST"] : "0");
                            detailViewModel.SCOrderStatus = Convert.ToString(dataRow["ScOrderStatus"] != DBNull.Value ? dataRow["ScOrderStatus"] : "");
                            detailViewModel.ZincCode = Convert.ToString(dataRow["Zinc_order_code"] != DBNull.Value ? dataRow["Zinc_order_code"] : "");
                            detailViewModel.ZincMessage = Convert.ToString(dataRow["Zinc_order_message"] != DBNull.Value ? dataRow["Zinc_order_message"] : "");

                            if (detailViewModel.ZincMessage != "")
                            {
                                if (detailViewModel.ZincMessage.Contains("Guaranteed delivery date"))
                                {
                                    detailViewModel.ZincMessage = detailViewModel.ZincMessage.Replace("Guaranteed delivery date", "Delivery Date");
                                }
                            }
                       //     detailViewModel.ZincOrderLogID = Convert.ToString(dataRow["zinc_order_log_id"] != DBNull.Value ? dataRow["zinc_order_log_id"] : "0");
                            detailViewModel.ZincRequestID = Convert.ToString(dataRow["request_id"] != DBNull.Value ? dataRow["request_id"] : "");
                       //     detailViewModel.ZincOrderLogDetailID = Convert.ToString(dataRow["zinc_order_log_detail_id"] != DBNull.Value ? dataRow["zinc_order_log_detail_id"] : "");
                            detailViewModel.ZincOrderStatusInternal = Convert.ToString(dataRow["zinc_order_status_internal"] != DBNull.Value ? dataRow["zinc_order_status_internal"] : "");
                            detailViewModel.IsTrackingUpdateToSC = Convert.ToBoolean(dataRow["is_tracking_updated"] != DBNull.Value ? dataRow["is_tracking_updated"] : false);
                            detailViewModel.DropshipStatus = Convert.ToBoolean(dataRow["dropship_status"] != DBNull.Value ? dataRow["dropship_status"] : false);
                            detailViewModel.OnOrder = Convert.ToInt32(dataRow["OnOrder"] != DBNull.Value ? dataRow["OnOrder"] : 0);
                            detailViewModel.DropshipQty = Convert.ToInt32(dataRow["dropship_Qty"] != DBNull.Value ? dataRow["dropship_Qty"] : "0");
                            detailViewModel.BestBuyPorductID = Convert.ToString(dataRow["bb_product_ID"] != DBNull.Value ? dataRow["bb_product_ID"] : "0");
                            //detailViewModel.WarehouseQuantity = Convert.ToString(dataRow["AggregatedQty"] != DBNull.Value ? dataRow["AggregatedQty"] : "0");


                            //calculations
                            detailViewModel.calculation_TotalAmountOfUnitPrice = detailViewModel.TotalQuantity * detailViewModel.UnitPrice;
                            detailViewModel.calculation_TotalTax = detailViewModel.TaxGST + detailViewModel.TaxPST;
                            detailViewModel.calculation_TotalTacPercentage = Math.Round((detailViewModel.calculation_TotalTax / detailViewModel.calculation_TotalAmountOfUnitPrice) * 100, 2);
                            //detailViewModel.calculation_Comission = Math.Round((detailViewModel.Comission) / (1 + detailViewModel.calculation_TotalTacPercentage / 100), 2);
                            detailViewModel.calculation_Comission = Math.Round((detailViewModel.Comission), 2);
                            detailViewModel.caculation_TotalAvgCost = Math.Round(Math.Round(Convert.ToDecimal(detailViewModel.AverageCost), 2) * detailViewModel.TotalQuantity, 2);
                            detailViewModel.calculation_SumTotal = Math.Round(detailViewModel.calculation_TotalTax + detailViewModel.calculation_TotalAmountOfUnitPrice, 2);
                            detailViewModel.calculation_comissionPercentage = Math.Round(((detailViewModel.calculation_Comission / detailViewModel.calculation_TotalAmountOfUnitPrice) * 100), 2);
                            detailViewModel.calculation_ProfitLoss = Math.Round((detailViewModel.calculation_TotalAmountOfUnitPrice + detailViewModel.ShippingFee) - detailViewModel.caculation_TotalAvgCost - detailViewModel.calculation_Comission, 2);
                            detailViewModel.calculation_ProfitLossPercentage = Math.Round((detailViewModel.calculation_ProfitLoss / detailViewModel.calculation_TotalAmountOfUnitPrice) * 100, 2);


                            List<ProductWarehouseQtyViewModel> warehouseQty = dataAccess.GetWareHousesQtyList(detailViewModel.ProductSKU);

                            detailViewModel.ZincAsinDetail = _zincDataAccess.GetProductZincDetailBySKU(detailViewModel.ProductSKU);
                            List<SkuTagOrderViewModel> skuTagOrders = _tagDataAccess.GetTagforSku(detailViewModel.ProductSKU);
                            detailViewModel.skuTags = skuTagOrders;
                            detailViewModel.ProductrWarehouseQtyViewModel = warehouseQty;
                            objList.Add(detailViewModel);


                        }


                        BBProductViewModel.BBProductDetail = objList;


                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return BBProductViewModel;
        }

        public string getString(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {


                //   '\'apple\',\'banana\''
                string finalString = "";
                string[] splitString = text.Split(',');

                int end = splitString.Length - 1;
                StringBuilder stringBuilder = new StringBuilder();

                if (!(splitString.Length > 1))
                {
                    stringBuilder.Append(string.Concat("\'", splitString[0] + "\'"));
                }
                else
                {
                    for (int i = 0; i < splitString.Length; i++)
                    {
                        stringBuilder.Append(string.Concat("\'", splitString[i] + "\',"));
                    }
                }

                int index = stringBuilder.ToString().LastIndexOf(',');
                if (index != -1)
                {
                    finalString = stringBuilder.ToString().Remove(index);
                }
                else
                {
                    finalString = stringBuilder.ToString();
                }

                return finalString;
            }
            else
            {
                return "";
            }
        }
    }
}
