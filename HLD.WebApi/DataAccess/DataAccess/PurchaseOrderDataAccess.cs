using DataAccess.Helper;
using DataAccess.ViewModels;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DataAccess.DataAccess
{
    public class PurchaseOrderDataAccess
    {

        public string DOTconnStr { get; set; }
        public PurchaseOrderDataAccess(IConnectionString connectionString)
        {

            DOTconnStr = connectionString.GetConnectionString();
        }

        public bool SavePurchaseOrders(PurchaseOrderDataViewModel purchaseOrderData)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(DOTconnStr))
                {
                    conn.Open();
                    MySqlCommand cmdd = new MySqlCommand("P_savePurchaseOrder", conn);
                    cmdd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmdd.Parameters.AddWithValue("_POId", purchaseOrderData.POId);
                    cmdd.Parameters.AddWithValue("_CompanyId", purchaseOrderData.CompanyId);
                    cmdd.Parameters.AddWithValue("_VendorId", purchaseOrderData.VendorId);
                    cmdd.Parameters.AddWithValue("_OrderedOn", purchaseOrderData.OrderedOn);
                    cmdd.Parameters.AddWithValue("_DefaultWarehouseID", purchaseOrderData.DefaultWarehouseID);
                    cmdd.Parameters.AddWithValue("_CurrencyCode", purchaseOrderData.CurrencyCode);
                    cmdd.Parameters.AddWithValue("_POStatus", purchaseOrderData.POStatus);

                    cmdd.ExecuteNonQuery();

                }


                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("POId", typeof(System.Int32)));
                dt.Columns.Add(new DataColumn("VendorId", typeof(System.Int32)));
                dt.Columns.Add(new DataColumn("ID", typeof(System.Int32)));
                dt.Columns.Add(new DataColumn("PurchaseID", typeof(System.Int32)));
                dt.Columns.Add(new DataColumn("ProductID", typeof(System.String)));
                dt.Columns.Add(new DataColumn("ProductTitle", typeof(System.String)));
                dt.Columns.Add(new DataColumn("QtyOrdered", typeof(System.Int32)));
                dt.Columns.Add(new DataColumn("QtyReceived", typeof(System.Int32)));
                dt.Columns.Add(new DataColumn("QtyOnHand", typeof(System.Int32)));
                dt.Columns.Add(new DataColumn("SkuStatus", typeof(System.Int32)));
                dt.Columns.Add(new DataColumn("UnitPrice", typeof(System.Decimal)));
                dt.Columns.Add(new DataColumn("CurrencyCode", typeof(System.Int16)));

                foreach (PurchaseOrderItemsDataViewModel item in purchaseOrderData.items)
                {
                    DataRow dr = dt.NewRow();
                    dr["POId"] = purchaseOrderData.POId;
                    dr["VendorId"] = purchaseOrderData.VendorId;
                    dr["ID"] = item.ID;
                    dr["PurchaseID"] = item.PurchaseID;
                    dr["ProductID"] = item.ProductID;
                    dr["QtyOrdered"] = item.QtyOrdered;
                    dr["QtyReceived"] = item.QtyReceived;
                    dr["QtyOnHand"] = item.QtyOnHand;
                    dr["UnitPrice"] = item.UnitPrice;
                    dr["SkuStatus"] = item.SkuStatus;
                    dr["ProductTitle"] = item.ProductTitle;
                    dr["CurrencyCode"] = purchaseOrderData.CurrencyCode;

                    dt.Rows.Add(dr);
                }

                MySqlConnection con = new MySqlConnection(DOTconnStr);
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                con.Open();
                MySqlCommand cmd = new MySqlCommand("P_savePurchaseOrderItems", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.UpdatedRowSource = UpdateRowSource.None;

                cmd.Parameters.Add("?_POId", MySqlDbType.Int32).SourceColumn = "POId";
                cmd.Parameters.Add("?_VendorId", MySqlDbType.Int32).SourceColumn = "VendorId";
                cmd.Parameters.Add("?_ID", MySqlDbType.Int32).SourceColumn = "ID";
                cmd.Parameters.Add("?_PurchaseID", MySqlDbType.Int32).SourceColumn = "PurchaseID";
                cmd.Parameters.Add("?_ProductID", MySqlDbType.String).SourceColumn = "ProductID";
                cmd.Parameters.Add("?_ProductTitle", MySqlDbType.String).SourceColumn = "ProductTitle";
                cmd.Parameters.Add("?_QtyOrdered", MySqlDbType.Int32).SourceColumn = "QtyOrdered";
                cmd.Parameters.Add("?_QtyOnHand", MySqlDbType.Int32).SourceColumn = "QtyOnHand";
                cmd.Parameters.Add("?_SkuStatus", MySqlDbType.Int32).SourceColumn = "SkuStatus";
                cmd.Parameters.Add("?_QtyReceived", MySqlDbType.Int32).SourceColumn = "QtyReceived";
                cmd.Parameters.Add("?_UnitPrice", MySqlDbType.Decimal).SourceColumn = "UnitPrice";
                cmd.Parameters.Add("?_Currency", MySqlDbType.Int32).SourceColumn = "CurrencyCode";
                MySqlDataAdapter da = new MySqlDataAdapter();
                da.InsertCommand = cmd;
                da.UpdateBatchSize = 100;
                int records = da.Update(dt);
                con.Close();
                status = true;
            }
            catch (Exception ex)
            {

                throw;
            }
            return status;
        }
        //PURCHASE COUNT
        public int GetAllPurchaseOrdersCount(int _vendorID, string CurrentDate, string PreviousDate, int POID, string OpenItem = "", string ReceivedItem = "", string OrderdItem = "", string NotShipped = "")
        {
            int count = 0;
            if (OpenItem == null)
                OpenItem = "";
            if (ReceivedItem == null)
                ReceivedItem = "";
            if (OrderdItem == null)
                OrderdItem = "";
            if (NotShipped == null)
                NotShipped = "";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(DOTconnStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetPOOrdersCountForFilter", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_vendorID", _vendorID);
                    cmd.Parameters.AddWithValue("_POID", POID);
                    cmd.Parameters.AddWithValue("_OpenItem", OpenItem);
                   cmd.Parameters.AddWithValue("_OrderedItem", OrderdItem);
                    cmd.Parameters.AddWithValue("_ReceivedItem", ReceivedItem);
                    cmd.Parameters.AddWithValue("_NotShipped", NotShipped);
                    cmd.Parameters.AddWithValue("dateFrom", PreviousDate);
                    cmd.Parameters.AddWithValue("dateTo", CurrentDate);


                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    count = dt.Rows.Count;
                    conn.Close();
                }
            }
            catch (Exception exp)
            {
            }
            return count;
        }
        public GetSummaryForPOViewModel GetTableHeaderForPO(int VendorId, string CurrentDate, string PreviousDate, int POID = 0, string OpenItem = "", string ReceivedItem = "", string OrderdItem = "",string NotShipped="")
        {
            if (OpenItem == null)
                OpenItem = "";
            if (ReceivedItem == null)
                ReceivedItem = "";
            if (OrderdItem == null)
                OrderdItem = "";
            if (NotShipped == null)
                NotShipped = "";

            GetSummaryForPOViewModel Counter = new GetSummaryForPOViewModel();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(DOTconnStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GettableHeadersForPurchaseOrdersCopy", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_VendorId", VendorId);
                    cmd.Parameters.AddWithValue("_OpenItem", OpenItem);
                    cmd.Parameters.AddWithValue("_OrderedItem", OrderdItem);
                    cmd.Parameters.AddWithValue("_ReceivedItem", ReceivedItem);
                    cmd.Parameters.AddWithValue("_NotShipped", NotShipped);
                    cmd.Parameters.AddWithValue("_POID", POID);
                    cmd.Parameters.AddWithValue("dateFrom", PreviousDate);
                    cmd.Parameters.AddWithValue("dateTo", CurrentDate);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow reader in dt.Rows)
                        {
                            Counter.ordQty = Convert.ToInt32(reader["ordQty"] != DBNull.Value ? reader["ordQty"] : 0);
                            Counter.OpenQty = Convert.ToInt32(reader["OpenQty"] != DBNull.Value ? reader["OpenQty"] : 0);
                            Counter.recQty = Convert.ToInt32(reader["recQty"] != DBNull.Value ? reader["recQty"] : 0);
                            Counter.shiQty = Convert.ToInt32(reader["shiQty"] != DBNull.Value ? reader["shiQty"] : 0);
                            Counter.orderamount = Convert.ToDecimal(reader["orderamount"] != DBNull.Value ? reader["orderamount"] : 0);
                            Counter.ReceviedAmount = Convert.ToDecimal(reader["ReceviedAmount"] != DBNull.Value ? reader["ReceviedAmount"] : 0);
                            Counter.OpenAmount = Convert.ToDecimal(reader["OpenAmount"] != DBNull.Value ? reader["OpenAmount"] : 0);
                            Counter.shipedamount = Convert.ToDecimal(reader["shipedamount"] != DBNull.Value ? reader["shipedamount"] : 0);
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception exp)
            {
            }
            return Counter;
        }
        //PURCAHSE list
        public List<PurchaseOrdersViewModel> GetAllPurchaseOrders(int VendorId, string CurrentDate, string PreviousDate, int StartLimit, int EndLimit, int POID = 0, string OpenItem = "", string ReceivedItem = "", string OrderdItem = "", string NotShipped = "")
        {

            if (OpenItem == null)
                OpenItem = "";
            if (ReceivedItem == null)
                ReceivedItem = "";
            if (OrderdItem == null)
                OrderdItem = "";
            if (NotShipped == null)
                NotShipped = "";

            List<PurchaseOrdersViewModel> _ViewModels = new List<PurchaseOrdersViewModel>();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(DOTconnStr))
                {

                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetPOOrdersForVendorSearch", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("_OpenItem", OpenItem);
                    cmd.Parameters.AddWithValue("_OrderedItem", OrderdItem);
                    cmd.Parameters.AddWithValue("_ReceivedItem", ReceivedItem);
                    cmd.Parameters.AddWithValue("_NotShipped", NotShipped);
                    cmd.Parameters.AddWithValue("dateFrom", PreviousDate);
                    cmd.Parameters.AddWithValue("dateTo", CurrentDate);
                    cmd.Parameters.AddWithValue("startLimit", StartLimit);
                    cmd.Parameters.AddWithValue("endLimit", EndLimit);
                    cmd.Parameters.AddWithValue("_vendorID", VendorId);
                    cmd.Parameters.AddWithValue("_POID", POID);

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {

                        _ViewModels = new List<PurchaseOrdersViewModel>();

                        foreach (DataRow reader in dt.Rows)
                        {
                            PurchaseOrdersViewModel ViewModel = new PurchaseOrdersViewModel();
                            ViewModel.POId = Convert.ToInt32(reader["POId"] != DBNull.Value ? reader["POId"] : 0);
                            ViewModel.CompanyId = Convert.ToInt32(reader["CompanyId"] != DBNull.Value ? reader["CompanyId"] : 0);
                            ViewModel.VendorId = Convert.ToInt32(reader["VendorId"] != DBNull.Value ? reader["VendorId"] : 0);
                            ViewModel.QtyOpen = Convert.ToInt32(reader["QtyOpen"] != DBNull.Value ? reader["QtyOpen"] : 0);
                            ViewModel.OrderQty = Convert.ToInt32(reader["OrderQty"] != DBNull.Value ? reader["OrderQty"] : 0);
                            ViewModel.recQty = Convert.ToInt32(reader["recQty"] != DBNull.Value ? reader["recQty"] : 0);
                            ViewModel.SKUCount = Convert.ToInt32(reader["SKUCount"] != DBNull.Value ? reader["SKUCount"] : 0);
                            ViewModel.AmountOpen = Convert.ToDecimal(reader["AmountOpen"] != DBNull.Value ? reader["AmountOpen"] : 0);
                            ViewModel.AmountOrdered = Convert.ToDecimal(reader["AmountOrdered"] != DBNull.Value ? reader["AmountOrdered"] : 0);
                            ViewModel.AmountReceived = Convert.ToDecimal(reader["AmountReceived"] != DBNull.Value ? reader["AmountReceived"] : 0);

                            ViewModel.OrderedOn = Convert.ToDateTime(reader["OrderedOn"] != DBNull.Value ? reader["OrderedOn"] : DateTime.Now);

                            ViewModel.DefaultWarehouseID = Convert.ToInt32(reader["DefaultWarehouseID"] != DBNull.Value ? reader["DefaultWarehouseID"] : 0);

                            ViewModel.CurrencyCode = Convert.ToInt32(reader["CurrencyCode"] != DBNull.Value ? reader["CurrencyCode"] : 0);
                            ViewModel.POStatus = Convert.ToInt32(reader["POStatus"] != DBNull.Value ? reader["POStatus"] : 0);
                            ViewModel.POAcceptanceDate = Convert.ToString(reader["POAcceptanceDate"] != DBNull.Value ? reader["POAcceptanceDate"] : "");
                            ViewModel.POLastUpdate = Convert.ToString(reader["POLastUpdate"] != DBNull.Value ? reader["POLastUpdate"] : "");
                            ViewModel.Vendor = Convert.ToString(reader["UserAlias"] != DBNull.Value ? reader["UserAlias"] : "");

                            ViewModel.ShippedQty = Convert.ToInt32(reader["ShippedQty"] != DBNull.Value ? reader["ShippedQty"] : 0);
                            ViewModel.ShippedAmount = Convert.ToDecimal(reader["ShippedAmount"] != DBNull.Value ? reader["ShippedAmount"] : 0);
                            _ViewModels.Add(ViewModel);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
            }
            return _ViewModels;
        }

        public PurchaseOrderModel GetAllPurchaseOrdersItems(searchPOitemViewModel searchP)
        {
            PurchaseOrderModel _Models = new PurchaseOrderModel();
            List<PurchaseOrderItemsViewModel> _ViewModels = null;
            MySqlConnection mySqlConnection = null;
            string SKU = "";
            string Title = "";
            string OpenQty = "";
            if (searchP.SKU != null)
                SKU = searchP.SKU;

            if (searchP.Title != null)
                Title = searchP.Title;

            if (searchP.OpenQty == true)
                OpenQty = "Yes";

            try
            {
                System.Data.DataSet ds = new System.Data.DataSet();
                using (MySqlConnection conn = new MySqlConnection(DOTconnStr))
                {

                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetPOItems", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("POId", searchP.POId);
                    cmd.Parameters.AddWithValue("_vendorID", searchP.vendorID);
                    cmd.Parameters.AddWithValue("_SKU", SKU);
                    cmd.Parameters.AddWithValue("_Title", Title);
                    cmd.Parameters.AddWithValue("_OpenQty", OpenQty);

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(ds);
                    _ViewModels = new List<PurchaseOrderItemsViewModel>();
                    System.Data.DataView dataView = new System.Data.DataView(ds.Tables[0]);
                    System.Data.DataTable distinctValue = dataView.ToTable(true, "POId");
                    DataTable dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        mySqlConnection = new MySqlConnection(DOTconnStr);
                        mySqlConnection.Open();
                    }
                    foreach (System.Data.DataRow reader in distinctValue.Rows)
                    {
                        var list = dt.AsEnumerable().Where(e => e.Field<Int32>("POId") == Convert.ToUInt32(reader["POId"])).ToList();



                        _Models.POId = Convert.ToInt32(list.Select(e => e.Field<Int32>("POId")).FirstOrDefault());
                        _Models.VendorId = Convert.ToInt32(list.Select(e => e.Field<Int32>("VendorId")).FirstOrDefault());

                        _Models.IsAccepted = Convert.ToInt32(list.Select(e => e.Field<Int32>("IsAccepted")).FirstOrDefault());
                        _Models.CurrencyCode = Convert.ToInt32(list.Select(e => e.Field<Int32>("CurrencyCode")).FirstOrDefault());
                        _Models.ExchangeRate = Convert.ToDecimal(list.Select(e => e.Field<decimal>("ExchangeRate")).FirstOrDefault());
                        _Models.POStatus = Convert.ToInt32(list.Select(e => e.Field<Int32>("POStatus")).FirstOrDefault());
                        _Models.OrderedOn = Convert.ToString(list.Select(e => e.Field<DateTime>("OrderedOn")).FirstOrDefault());

                        _Models.POAcceptanceDate = Convert.ToString(list.Select(e => e.Field<DateTime?>("POAcceptanceDate")).FirstOrDefault());
                        _Models.POLastUpdate = Convert.ToString(list.Select(e => e.Field<DateTime?>("POLastUpdate")).FirstOrDefault());
                        _Models.Notes = Convert.ToString(list.Select(e => e.Field<String>("Notes")).FirstOrDefault());
                        _Models.POArrivalDate = Convert.ToString(list.Select(e => e.Field<DateTime?>("POArrivalDate")).FirstOrDefault());
                        _Models.Vendor = Convert.ToString(list.Select(e => e.Field<String>("Vendor")).FirstOrDefault());
                        foreach (DataRow dataRow in list)
                        {
                            PurchaseOrderItemsViewModel ViewModel = new PurchaseOrderItemsViewModel();
                            ViewModel.ID = Convert.ToInt32(dataRow["ID"] != DBNull.Value ? dataRow["ID"] : "0");
                            ViewModel.PurchaseID = Convert.ToInt32(dataRow["PurchaseID"] != DBNull.Value ? dataRow["PurchaseID"] : "0");

                            ViewModel.ProductID = Convert.ToString(dataRow["ProductID"] != DBNull.Value ? dataRow["ProductID"] : "0");
                            ViewModel.ProductTitle = Convert.ToString(dataRow["ProductTitle"] != DBNull.Value ? dataRow["ProductTitle"] : "");

                            ViewModel.Compress_image = Convert.ToString(dataRow["Compress_image"] != DBNull.Value ? dataRow["Compress_image"] : "");
                            ViewModel.image_name = Convert.ToString(dataRow["image_name"] != DBNull.Value ? dataRow["image_name"] : "");

                            ViewModel.QtyOrdered = Convert.ToInt32(dataRow["QtyOrdered"]);

                            ViewModel.QtyReceived = Convert.ToInt32(dataRow["QtyReceived"]);

                            ViewModel.QtyOpen = Convert.ToInt32(dataRow["QtyOpen"]);

                            ViewModel.SkuStatus = Convert.ToInt32(dataRow["SkuStatus"]);

                            ViewModel.UnitPrice = Convert.ToDecimal(dataRow["UnitPrice"]);
                            ViewModel.UnitPriceUSD = Convert.ToDecimal(dataRow["UnitPriceUSD"]);
                            ViewModel.QtyOnHand = Convert.ToInt32(dataRow["QtyOnHand"]);
                            ViewModel.ReceivedAmount = Convert.ToDecimal(dataRow["ReceivedAmount"]);
                            ViewModel.OpenAmount = Convert.ToDecimal(dataRow["OpenAmount"]);
                            ViewModel.OrderedAmount = Convert.ToDecimal(dataRow["OrderedAmount"]);
                            ViewModel.ShippedQty = Convert.ToInt32(dataRow["ShippedQty"] != DBNull.Value ? dataRow["ShippedQty"] : 0);
                            ViewModel.ShippedAmount = Convert.ToDecimal(dataRow["ShippedAmount"] != DBNull.Value ? dataRow["ShippedAmount"] : 0);
                            _ViewModels.Add(ViewModel);
                        }
                        _Models.ListItems = _ViewModels;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return _Models;
        }
        public int UpdateCurrency(int POId, int Currency)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(DOTconnStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_UpdatePurchaseOrderCurrencyCode", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_POId", POId);
                    cmd.Parameters.AddWithValue("_CurrencyCode", Currency);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {

            }
            return 0;
        }
        public List<int> GetAllPurchaseOrdersToGetUpdate()
        {

            List<int> _ViewModels = new List<int>();

            try
            {
                System.Data.DataSet ds = new System.Data.DataSet();
                using (MySqlConnection conn = new MySqlConnection(DOTconnStr))
                {

                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetPOToGetUpdate", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {

                        foreach (DataRow reader in dt.Rows)
                        {
                            int count = 0;
                            count = Convert.ToInt32(reader["POId"]);
                            _ViewModels.Add(count);
                        }


                    }
                }
            }
            catch (Exception ex)
            {
            }
            return _ViewModels;
        }

        public bool SavePOAsAccepted(UpdatePOAcceptedViewModel updatePOAccepted)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(DOTconnStr))
                {
                    conn.Open();
                    MySqlCommand cmdd = new MySqlCommand("P_SavePOAsAccepted", conn);
                    cmdd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmdd.Parameters.AddWithValue("_POId", updatePOAccepted.POId);
                    cmdd.Parameters.AddWithValue("_IsAccepted", updatePOAccepted.IsAccepted);
                    cmdd.Parameters.AddWithValue("_POAcceptanceDate", DateTime.Now);
                    cmdd.Parameters.AddWithValue("_POLastUpdate", updatePOAccepted.POLastUpdate);
                    cmdd.Parameters.AddWithValue("_POArrivalDate", updatePOAccepted.POArrivalDate);

                    cmdd.ExecuteNonQuery();
                    status = true;
                }


            }
            catch (Exception ex)
            {

                throw;
            }
            return status;
        }

        public bool UpdatePOShipDate(UpdatePOAcceptedViewModel updatePOAccepted)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(DOTconnStr))
                {
                    conn.Open();
                    MySqlCommand cmdd = new MySqlCommand("P_UpdatePOShipDate", conn);
                    cmdd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmdd.Parameters.AddWithValue("_POId", updatePOAccepted.POId);

                    cmdd.Parameters.AddWithValue("_POLastUpdate", updatePOAccepted.POLastUpdate);
                    cmdd.Parameters.AddWithValue("_POArrivalDate", updatePOAccepted.POArrivalDate);

                    cmdd.ExecuteNonQuery();
                    status = true;
                }


            }
            catch (Exception ex)
            {

                throw;
            }
            return status;
        }
        public bool SaveCurrencyExchange(UpdatePOExchangeViewModel exchangeViewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(DOTconnStr))
                {
                    conn.Open();
                    MySqlCommand cmdd = new MySqlCommand("P_SaveCurrency", conn);
                    cmdd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmdd.Parameters.AddWithValue("_POId", exchangeViewModel.POId);

                    cmdd.Parameters.AddWithValue("_ExchangeRate", exchangeViewModel.ExchangeRate);

                    cmdd.ExecuteNonQuery();
                    status = true;
                }


            }
            catch (Exception ex)
            {

                throw;
            }
            return status;
        }

        public UpdatePOAcceptedViewModel GetPOShiDatestoupdate(int poID)
        {
            UpdatePOAcceptedViewModel _ViewModels = null;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(DOTconnStr))
                {

                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetPOShiDatestoupdate", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_POId", poID);


                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {

                        _ViewModels = new UpdatePOAcceptedViewModel();

                        foreach (DataRow reader in dt.Rows)
                        {


                            _ViewModels.IsAccepted = Convert.ToInt32(reader["IsAccepted"]);
                            _ViewModels.POId = Convert.ToInt32(reader["POId"]);
                            _ViewModels.POArrivalDate = Convert.ToDateTime(reader["POArrivalDate"] != DBNull.Value ? reader["POArrivalDate"] : "");
                            _ViewModels.POLastUpdate = Convert.ToDateTime(reader["POLastUpdate"] != DBNull.Value ? reader["POLastUpdate"] : "");


                        }

                    }
                }
            }
            catch (Exception ex)
            {
            }
            return _ViewModels;
        }

        public MissingOrderReturnViewModel CheckPurchaseOrderOrderINDB(List<CheckMissingOrderViewModel> viewModel)
        {
            List<int> extid = new List<int>();
            List<int> msdid = new List<int>();
            MissingOrderReturnViewModel listModel = new MissingOrderReturnViewModel();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(DOTconnStr))
                {
                    conn.Open();
                    foreach (var item in viewModel)
                    {
                        MySqlCommand cmdd = new MySqlCommand(@"select POId from PurchaseOrders where POId = " + item.OrderID, conn);
                        cmdd.CommandType = System.Data.CommandType.Text;
                        MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(cmdd);
                        DataTable dt = new DataTable();
                        mySqlDataAdapter.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {

                            foreach (DataRow dr in dt.Rows)
                            {
                                int id = Convert.ToInt32(dr["POId"]);
                                if (id != 0)
                                {
                                    listModel.ExistingOrderCount++;

                                    extid.Add(id);


                                }

                            }

                        }
                        else
                        {
                            listModel.MissingOrderCount++;
                            msdid.Add(item.OrderID);
                        }

                    }
                    listModel.ExistingOrder = extid;
                    listModel.MissingOrder = msdid;

                }
                return listModel;
            }
            catch (Exception)
            {

                throw;
            }

        }


        //PO  list
        public List<PurchaseOrderItemsViewModel> GetPOProductDetails(int VendorId, string CurrentDate, string PreviousDate, int StartLimit, int EndLimit, int POID, string SKU = "", string Title = "", string OpenItem = "", string ReceivedItem = "", string OrderdItem = "", string NotShipped = "", string ShippedButNotReceived = "")
        {
            if (SKU == null)
                SKU = "";
            if (Title == null)
                Title = "";
            if (OpenItem == null)
                OpenItem = "";
            if (ReceivedItem == null)
                ReceivedItem = "";
            if (OrderdItem == null)
                OrderdItem = "";
            if (NotShipped == null)
                NotShipped = "";
            if (ShippedButNotReceived == null)
                ShippedButNotReceived = "";

            List<PurchaseOrderItemsViewModel> list = new List<PurchaseOrderItemsViewModel>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(DOTconnStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetPOProductDetails", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_VendorId", VendorId);
                    cmd.Parameters.AddWithValue("_Limit", StartLimit);
                    cmd.Parameters.AddWithValue("_OffSet", EndLimit);
                    cmd.Parameters.AddWithValue("_SKU", SKU);
                    cmd.Parameters.AddWithValue("_Title", Title);
                    cmd.Parameters.AddWithValue("_OpenItem", OpenItem);
                    cmd.Parameters.AddWithValue("_OrderedItem", OrderdItem);
                    cmd.Parameters.AddWithValue("_ReceivedItem", ReceivedItem);
                    cmd.Parameters.AddWithValue("_NotShipped", NotShipped);
                    cmd.Parameters.AddWithValue("_ShippedButNotReceived", ShippedButNotReceived);
                    cmd.Parameters.AddWithValue("_POID", POID);
                    cmd.Parameters.AddWithValue("dateFrom", PreviousDate);
                    cmd.Parameters.AddWithValue("dateTo", CurrentDate);

                    //DataSet ds = new DataSet();
                    //DataTable dt = new DataTable();
                    //MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    //da.Fill(dt);
                    //conn.Close();

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                PurchaseOrderItemsViewModel viewModel = new PurchaseOrderItemsViewModel();
                                {
                                    viewModel.ProductID = (string)reader["ProductID"];
                                    viewModel.PurchaseID = (int)reader["POId"];
                                    viewModel.ProductTitle = (string)reader["ProductTitle"];
                                    viewModel.VendorID = (int)reader["VendorId"];
                                    viewModel.Compress_image = reader["Compress_image"] != DBNull.Value ? (string)reader["Compress_image"] : "";
                                    viewModel.image_name = reader["image_name"] != DBNull.Value ? (string)reader["image_name"] : "";
                                    viewModel.OrderedOn = Convert.ToDateTime(reader["OrderedOn"] != DBNull.Value ? reader["OrderedOn"] : DateTime.MinValue);
                                    viewModel.Vendor = (string)reader["Vendor"];
                                    viewModel.QtyOrdered = (int)reader["QtyOrdered"];
                                    viewModel.QtyReceived = (int)reader["QtyReceived"];
                                    viewModel.CurrencyCode = (int)reader["CurrencyCode"];
                                    viewModel.OpenQty = (long)reader["OpenQty"];
                                    viewModel.OrderedAmount = (decimal)reader["OrderedAmount"];
                                    viewModel.ReceivedAmount = (decimal)reader["ReceivedAmount"];
                                    viewModel.OpenAmount = (decimal)reader["OpenAmount"];
                                    viewModel.UnitPrice = (decimal)reader["UnitPrice"];
                                    viewModel.UnitPriceUSD = (decimal)reader["UnitPriceUSD"];
                                    viewModel.ShippedQty = reader["ShippedQty"] != DBNull.Value ? Convert.ToInt32(reader["ShippedQty"]) : 0;
                                    viewModel.ShippedAmount = reader["ShippedAmount"] != DBNull.Value ? Convert.ToInt32(reader["ShippedAmount"]) : 0;
                                }
                                list.Add(viewModel);
                            }
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {

            }
            return list;
        }


        //PO COUNT pc
        public GetSummaryandCountPOViewModel GetCount(int VendorId, string CurrentDate, string PreviousDate, int POID = 0, string SKU = "", string Title = "", string OpenItem = "", string ReceivedItem = "", string OrderdItem = "", string NotShipped = "",string ShippedButNotReceived = "")
        {
            if (SKU == null)
                SKU = "";
            if (Title == null)
                Title = "";
            if (OpenItem == null)
                OpenItem = "";
            if (ReceivedItem == null)
                ReceivedItem = "";
            if (OrderdItem == null)
                OrderdItem = "";

            if (NotShipped == null)
                NotShipped = "";
            if (ShippedButNotReceived == null)
                ShippedButNotReceived = "";
            GetSummaryandCountPOViewModel Counter = new GetSummaryandCountPOViewModel();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(DOTconnStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetPOProductsCount", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_VendorId", VendorId);
                    cmd.Parameters.AddWithValue("_SKU", SKU);
                    cmd.Parameters.AddWithValue("_Title", Title);
                    cmd.Parameters.AddWithValue("_OpenItem", OpenItem);
                    cmd.Parameters.AddWithValue("_OrderedItem", OrderdItem);
                    cmd.Parameters.AddWithValue("_ReceivedItem", ReceivedItem);
                    cmd.Parameters.AddWithValue("_NotShipped", NotShipped);
                    cmd.Parameters.AddWithValue("_ShippedButNotReceived", ShippedButNotReceived);
                    cmd.Parameters.AddWithValue("_POID", POID);
                    cmd.Parameters.AddWithValue("dateFrom", PreviousDate);
                    cmd.Parameters.AddWithValue("dateTo", CurrentDate);


                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow reader in dt.Rows)
                        {
                            Counter.count = Convert.ToInt32(reader["count"] != DBNull.Value ? reader["count"] : 0);
                            Counter.POcount = Convert.ToInt32(reader["POcount"] != DBNull.Value ? reader["POcount"] : 0);
                            Counter.ordQty = Convert.ToInt32(reader["ordQty"] != DBNull.Value ? reader["ordQty"] : 0);
                            Counter.OpenQty = Convert.ToInt32(reader["OpenQty"] != DBNull.Value ? reader["OpenQty"] : 0);
                            Counter.recQty = Convert.ToInt32(reader["recQty"] != DBNull.Value ? reader["recQty"] : 0);
                            Counter.shiQty = Convert.ToInt32(reader["shiQty"] != DBNull.Value ? reader["shiQty"] : 0);
                            Counter.orderamount = Convert.ToInt32(reader["orderamount"] != DBNull.Value ? reader["orderamount"] : 0);
                            Counter.ReceviedAmount = Convert.ToInt32(reader["ReceviedAmount"] != DBNull.Value ? reader["ReceviedAmount"] : 0);
                            Counter.OpenAmount = Convert.ToInt32(reader["OpenAmount"] != DBNull.Value ? reader["OpenAmount"] : 0);
                            Counter.shipedamount = Convert.ToInt32(reader["shipedamount"] != DBNull.Value ? reader["shipedamount"] : 0);
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception exp)
            {
            }
            return Counter;
        }


        public bool UpdatePurchaseOrders(PurchaseOrderDataViewModel purchaseOrderData)
        {
            bool status = false;
            try
            {


                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("POId", typeof(System.Int32)));
                dt.Columns.Add(new DataColumn("VendorId", typeof(System.Int32)));
                dt.Columns.Add(new DataColumn("ID", typeof(System.Int32)));
                dt.Columns.Add(new DataColumn("PurchaseID", typeof(System.Int32)));
                dt.Columns.Add(new DataColumn("ProductID", typeof(System.String)));
                dt.Columns.Add(new DataColumn("ProductTitle", typeof(System.String)));
                dt.Columns.Add(new DataColumn("QtyOrdered", typeof(System.Int32)));
                dt.Columns.Add(new DataColumn("QtyReceived", typeof(System.Int32)));
                dt.Columns.Add(new DataColumn("QtyOnHand", typeof(System.Int32)));
                dt.Columns.Add(new DataColumn("SkuStatus", typeof(System.Int32)));
                dt.Columns.Add(new DataColumn("UnitPrice", typeof(System.Decimal)));
                dt.Columns.Add(new DataColumn("CurrencyCode", typeof(System.Int16)));

                foreach (PurchaseOrderItemsDataViewModel item in purchaseOrderData.items)
                {
                    DataRow dr = dt.NewRow();
                    dr["POId"] = purchaseOrderData.POId;
                    dr["VendorId"] = purchaseOrderData.VendorId;
                    dr["ID"] = item.ID;
                    dr["PurchaseID"] = item.PurchaseID;
                    dr["ProductID"] = item.ProductID;
                    dr["QtyOrdered"] = item.QtyOrdered;
                    dr["QtyReceived"] = item.QtyReceived;
                    dr["QtyOnHand"] = item.QtyOnHand;
                    dr["UnitPrice"] = item.UnitPrice;
                    dr["SkuStatus"] = item.SkuStatus;
                    dr["ProductTitle"] = item.ProductTitle;
                    dr["CurrencyCode"] = purchaseOrderData.CurrencyCode;

                    dt.Rows.Add(dr);
                }

                MySqlConnection con = new MySqlConnection(DOTconnStr);
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                con.Open();
                MySqlCommand cmd = new MySqlCommand("P_UpdatePurchaseOrderItems", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.UpdatedRowSource = UpdateRowSource.None;

                cmd.Parameters.Add("?_POId", MySqlDbType.Int32).SourceColumn = "POId";
                cmd.Parameters.Add("?_VendorId", MySqlDbType.Int32).SourceColumn = "VendorId";
                cmd.Parameters.Add("?_ID", MySqlDbType.Int32).SourceColumn = "ID";
                cmd.Parameters.Add("?_PurchaseID", MySqlDbType.Int32).SourceColumn = "PurchaseID";
                cmd.Parameters.Add("?_ProductID", MySqlDbType.String).SourceColumn = "ProductID";
                cmd.Parameters.Add("?_ProductTitle", MySqlDbType.String).SourceColumn = "ProductTitle";
                cmd.Parameters.Add("?_QtyOrdered", MySqlDbType.Int32).SourceColumn = "QtyOrdered";
                cmd.Parameters.Add("?_QtyOnHand", MySqlDbType.Int32).SourceColumn = "QtyOnHand";
                cmd.Parameters.Add("?_SkuStatus", MySqlDbType.Int32).SourceColumn = "SkuStatus";
                cmd.Parameters.Add("?_QtyReceived", MySqlDbType.Int32).SourceColumn = "QtyReceived";
                cmd.Parameters.Add("?_Currency", MySqlDbType.Int32).SourceColumn = "CurrencyCode";
                cmd.Parameters.Add("?_UnitPrice", MySqlDbType.Decimal).SourceColumn = "UnitPrice";

                MySqlDataAdapter da = new MySqlDataAdapter();
                da.InsertCommand = cmd;
                da.UpdateBatchSize = 100;
                int records = da.Update(dt);
                con.Close();
                status = true;
            }
            catch (Exception ex)
            {

                throw;
            }
            return status;
        }

        public void DeleteRemovedPOItems(string POItems, int poID)
        {

            try
            {
                using (MySqlConnection conn = new MySqlConnection(DOTconnStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_DeletePOItems", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("POItems", POItems);
                    cmd.Parameters.AddWithValue("_POID", poID);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
            }

        }


        public List<ProductSKUViewModel> GetAllSKUForAutoCompleteFromPO(string SKU, int POID)
        {
            List<ProductSKUViewModel> listViewModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(DOTconnStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetSkuFromPoForAutocomplete", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_SKU", SKU.Trim());
                    cmd.Parameters.AddWithValue("POID", POID);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            listViewModel = new List<ProductSKUViewModel>();
                            while (reader.Read())
                            {
                                ProductSKUViewModel ViewModel = new ProductSKUViewModel();
                                ViewModel.SKU = Convert.ToString(reader["ProductID"]);
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

        public List<ProductSKUViewModel> GetAllSKUForAutoComplete(string name, int VendorId)
        {
            List<ProductSKUViewModel> listViewModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(DOTconnStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetPurchaseOrdersItemsSKU", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_name", name.Trim());
                    cmd.Parameters.AddWithValue("_VendorId", VendorId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            listViewModel = new List<ProductSKUViewModel>();
                            while (reader.Read())
                            {
                                ProductSKUViewModel ViewModel = new ProductSKUViewModel();
                                ViewModel.SKU = Convert.ToString(reader["sku"]);
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
        public List<POIdViewModel> GetPOIdBySku(string name, int VendorId)
        {
            List<POIdViewModel> list = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(DOTconnStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetPOIDbySKU", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_name", name.Trim());
                    cmd.Parameters.AddWithValue("_VendorId", VendorId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            list = new List<POIdViewModel>();
                            while (reader.Read())
                            {
                                POIdViewModel viewModel = new POIdViewModel();
                                viewModel.POId = Convert.ToInt32(reader["POId"]);
                                viewModel.OpenQty = reader["OpenQty"] != DBNull.Value ? Convert.ToDecimal(reader["OpenQty"]) : 0;
                                viewModel.BalanceQty = reader["BalanceQty"] != DBNull.Value ? Convert.ToDecimal(reader["BalanceQty"]) : 0;
                                list.Add(viewModel);
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


        public bool DeletePOItems(int ID)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(DOTconnStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_DeletePOItemsFromlocal", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("_ID", ID);
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;
        }

        public bool DeletePO(int Id)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(DOTconnStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_DeleteProductByPOID", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Id", Id);
                    cmd.ExecuteNonQuery();
                    status = true;
                }

            }
            catch (Exception exp)
            {

            }
            return status;
        }

        public bool UpdateTitle(string Sku, string Title)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(DOTconnStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_UpdateProductTitle", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_sku", Sku);
                    cmd.Parameters.AddWithValue("_title", Title);

                    cmd.ExecuteNonQuery();
                    conn.Close();
                    status = true;
                }
            }
            catch (Exception exp)
            {
            }
            return status;
        }


    }
}
