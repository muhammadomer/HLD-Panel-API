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
    public class SCOrderPageViewDataAccess
    {

        public string connStr { get; set; }
        ProductWarehouseQtyDataAccess dataAccess = null;
        ZincDataAccess _zincDataAccess = null;
        TagDataAccess _tagDataAccess = null;
        ApprovedPriceDataAccess ApprovedPriceDataAccess = null;

        public SCOrderPageViewDataAccess(IConnectionString connectionString)
        {
            connStr = connectionString.GetConnectionString();
            dataAccess = new ProductWarehouseQtyDataAccess(connectionString);
            _zincDataAccess = new ZincDataAccess(connectionString);
            _tagDataAccess = new TagDataAccess(connectionString);
            ApprovedPriceDataAccess = new ApprovedPriceDataAccess(connectionString);
        }

        public SCOrderPaymentPageViewModel GetSCOrderForOrderPageView(string bbOrderId)
        {
            SCOrderPaymentPageViewModel ViewModel = null;
            try
            {

                System.Data.DataSet ds = new System.Data.DataSet();
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {

                    MySqlCommand cmd = new MySqlCommand("P_GETPaymentDetailsForOrderViewPage", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_BBOrderID", bbOrderId);


                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(ds);

                    if (ds.Tables.Count > 0)
                    {
                        System.Data.DataView dataView = new System.Data.DataView(ds.Tables[0]);
                        System.Data.DataTable distinctValue = dataView.ToTable(true, "sellerCloudID");
                        DataTable dt = ds.Tables[0];
                        foreach (System.Data.DataRow reader in distinctValue.Rows)
                        {
                            ViewModel = new SCOrderPaymentPageViewModel();
                            var list = dt.AsEnumerable().Where(e => e.Field<string>("sellerCloudID") == reader["sellerCloudID"].ToString()).ToList();

                            var totalQuantity = list.Sum(e => Convert.ToDouble(e.Field<string>("quantity")));
                            var totalPrice = list.Sum(e => Convert.ToDouble(e.Field<string>("total_price")));
                            var total_commission = list.Sum(e => Convert.ToDouble(e.Field<string>("total_commission")));
                            var totalGst = list.Sum(e => Convert.ToDouble(e.Field<string>("TaxGST")));
                            var totalPst = list.Sum(e => Convert.ToDouble(e.Field<string>("TaxPST")));
                            var toalAverageCost = list.Sum(e => e.Field<double>("TotalAverageCost"));


                            var sumOfGstPst = totalGst + totalPst;

                            var total = (sumOfGstPst / totalPrice) * 100;

                            var totalComission = Math.Round((total_commission) / (1 + total / 100), 2);
                            var ProfitLoss = Math.Round(totalPrice - toalAverageCost - totalComission, 2);
                            var profitAndLossInPercent = Math.Round((ProfitLoss / totalPrice) * 100);


                            ViewModel.SellerCloudID = Convert.ToString(list.Select(e => e.Field<string>("sellerCloudID")).FirstOrDefault());
                            ViewModel.sku = Convert.ToString(list.Select(e => e.Field<string>("sku")).FirstOrDefault());


                            ViewModel.TotalTax = Convert.ToDecimal(0);
                            ViewModel.TotalQty = Convert.ToInt32(totalQuantity);
                            ViewModel.TotalCommission = Convert.ToDecimal(totalComission);
                            ViewModel.TotalPrice = Convert.ToDecimal(totalPrice);
                            ViewModel.ProfitAndLossInDollar = Convert.ToDecimal(ProfitLoss);
                            ViewModel.ProfitAndLossInPercentage = Convert.ToDecimal(profitAndLossInPercent);
                            ViewModel.TotalAvgCost = Convert.ToDecimal(toalAverageCost);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return ViewModel;

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

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_SourceOrderID", bbOrderId);

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(ds);
                    BBProductViewModel = new BestBuyOrdersViewPageModel();
                    System.Data.DataView dataView = new System.Data.DataView(ds.Tables[0]);
                    System.Data.DataTable distinctValue = dataView.ToTable(true, "sellerCloudID");
                    DataTable dt = ds.Tables[0];
                    foreach (System.Data.DataRow reader in distinctValue.Rows)
                    {
                        List<BestBuyOrderDetailViewModel> objList = new List<BestBuyOrderDetailViewModel>();

                        var list = dt.AsEnumerable().Where(e => e.Field<string>("sellerCloudID") == reader["sellerCloudID"].ToString()).ToList();


                        var totalQuantity = list.Sum(e => Convert.ToDouble(e.Field<string>("quantity")));
                        var totalPrice = (list.Sum(e => Convert.ToDouble(e.Field<string>("total_price"))));
                        var total_commission = list.Sum(e => Convert.ToDouble(e.Field<string>("total_commission")));
                        var totalGst = list.Sum(e => Convert.ToDouble(e.Field<string>("TaxGST")));
                        var totalPst = list.Sum(e => Convert.ToDouble(e.Field<string>("TaxPST")));
                        var toalAverageCost = list.Sum(e => e.Field<double>("TotalAverageCost"));

                        var shipping = Convert.ToDouble(list.Select(e => e.Field<string>("ShippingPaidByCustomer")).FirstOrDefault());
                        
                        var sumOfGstPst = totalGst + totalPst;

                        var total = (sumOfGstPst / totalPrice) * 100;

                        //var totalComission = Math.Round((total_commission) / (1 + total / 100), 2);
                        var totalComission = total_commission;
                        var ProfitLoss = Math.Round(totalPrice - toalAverageCost - total_commission, 2);
                        var profitAndLossInPercent = Math.Round((ProfitLoss / totalPrice) * 100);

                        totalPrice = totalPrice - shipping;

                        BBProductViewModel.OrderNumber = Convert.ToString(list.Select(e => e.Field<string>("order_id")).FirstOrDefault());
                        BBProductViewModel.SellerCloudOrderID = Convert.ToString(list.Select(e => e.Field<string>("sellerCloudID")).FirstOrDefault());
                        BBProductViewModel.CustomerName = Convert.ToString(list.Select(e => e.Field<string>("CustomerName")).FirstOrDefault());
                        BBProductViewModel.Street = Convert.ToString(list.Select(e => e.Field<string>("Street")).FirstOrDefault());
                        BBProductViewModel.State = Convert.ToString(list.Select(e => e.Field<string>("State")).FirstOrDefault());
                        BBProductViewModel.Country = Convert.ToString(list.Select(e => e.Field<string>("Country")).FirstOrDefault());
                        BBProductViewModel.OrderDate = Convert.ToDateTime(list.Select(e => e.Field<DateTime>("acceptance_decision_date")).FirstOrDefault());
                        BBProductViewModel.ParentOrderID = Convert.ToInt32(list.Select(e => e.Field<int?>("ParentOrderID")).FirstOrDefault() != null ? list.Select(e => e.Field<int>("ParentOrderID")).FirstOrDefault() : 0).ToString();
                        BBProductViewModel.IsParent = Convert.ToString(list.Select(e => e.Field<string>("IsParent")).FirstOrDefault());
                        BBProductViewModel.IsNotes = Convert.ToString(list.Select(e => e.Field<string>("IsNotes")).FirstOrDefault());
                        BBProductViewModel.ShippingPrice = Convert.ToString(list.Select(e => e.Field<string>("ShippingPaidByCustomer")).FirstOrDefault());

                        BBProductViewModel.TotalTax = Convert.ToDecimal(0);
                        BBProductViewModel.TotalQuantity = Convert.ToInt32(totalQuantity);
                        BBProductViewModel.TotalComission = Convert.ToDecimal(total_commission);
                        BBProductViewModel.TotalPrice = Math.Round(Convert.ToDecimal(totalPrice), 2);
                        BBProductViewModel.ProfitAndLossInDollar = Convert.ToDecimal(ProfitLoss);
                        BBProductViewModel.ProfitAndLossInPercentage = Convert.ToDecimal(profitAndLossInPercent);
                        BBProductViewModel.TotalAverageCost = Convert.ToDecimal(toalAverageCost);
                        BBProductViewModel.tracking_number = Convert.ToString(list.Select(e => e.Field<string>("tracking_number")).FirstOrDefault());
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
                            detailViewModel.UnitPrice = Convert.ToDecimal(dataRow["total_price"] != DBNull.Value ? dataRow["total_price"] : "0");
                            detailViewModel.UnitPrice = Math.Round(detailViewModel.UnitPrice / detailViewModel.TotalQuantity, 2);
                            detailViewModel.OrderDetailID = Convert.ToInt32(dataRow["bbe2_line_id"] != DBNull.Value ? dataRow["bbe2_line_id"] : "0");
                            detailViewModel.TaxGST = Convert.ToDecimal(dataRow["TaxGST"] != DBNull.Value ? dataRow["TaxGST"] : "0");
                            detailViewModel.TaxPST = Convert.ToDecimal(dataRow["TaxPST"] != DBNull.Value ? dataRow["TaxPST"] : "0");
                            detailViewModel.SCOrderStatus = Convert.ToString(dataRow["ScOrderStatus"] != DBNull.Value ? dataRow["ScOrderStatus"] : "");
                            detailViewModel.ZincCode = Convert.ToString(dataRow["order_code"] != DBNull.Value ? dataRow["order_code"] : "");
                            detailViewModel.ZincMessage = Convert.ToString(dataRow["order_message"] != DBNull.Value ? dataRow["order_message"] : "");

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
            }

            return BBProductViewModel;
        }



    }
}
