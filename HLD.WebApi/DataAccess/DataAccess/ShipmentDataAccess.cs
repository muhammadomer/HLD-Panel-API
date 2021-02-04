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
    public class ShipmentDataAccess

    {
        public string ConStr { get; set; }
        public ShipmentDataAccess(IConnectionString connectionString)
        {

            ConStr = connectionString.GetConnectionString();
        }
        public string SaveShipment(ShipmentViewModel ViewModel)
        {
            string status = "";
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmdd = new MySqlCommand("p_SaveShipmentDumy", conn);
                    cmdd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmdd.Parameters.AddWithValue("_ShipmentId", ViewModel.ShipmentId);
                    cmdd.Parameters.AddWithValue("_VendorId", ViewModel.VendorId);
                    cmdd.Parameters.AddWithValue("_ShipmentName", ViewModel.ShipmentName);
                    cmdd.Parameters.AddWithValue("_Notes", ViewModel.Notes);
                    cmdd.Parameters.AddWithValue("_CreatedOn", ViewModel.CreatedOn);
                    cmdd.Parameters.AddWithValue("_Type", ViewModel.Type);
                    status=(string)cmdd.ExecuteScalar();
                    
                }
            }
            catch (Exception ex)
            {

            }
            return status;
        }
        public long GetShipmentsListCount(int VendorId, string CurrentDate, string PreviousDate, string ShipmentId, string TrakingNumber, string Status, string Type)
        {
            if (ShipmentId == null)
                ShipmentId = "";
            if (TrakingNumber == null)
                TrakingNumber = "";
            if (Status == null)
                Status = "";
            if (Type == null)
                Type = "";
            long Counter = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetShipmentslistCount", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_VendorId", VendorId);
                    cmd.Parameters.AddWithValue("_ShipmentId", ShipmentId);
                    cmd.Parameters.AddWithValue("_TrankingNumber", TrakingNumber);
                    cmd.Parameters.AddWithValue("_Status", Status);
                    cmd.Parameters.AddWithValue("_Type", Type);
                    cmd.Parameters.AddWithValue("dateFrom", PreviousDate);
                    cmd.Parameters.AddWithValue("dateTo", CurrentDate);

                    Counter = (long)cmd.ExecuteScalar();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Counter;
        }

        public List<ShipmentGetDataViewModel> GetShipmentsList(int VendorId, int Limit, int Offset, string CurrentDate, string PreviousDate, string ShipmentId, string TrakingNumber, string Status, string Type)
        {
            if (string.IsNullOrEmpty(ShipmentId) || ShipmentId == "undefined")
                ShipmentId = "";
            if (string.IsNullOrEmpty(TrakingNumber) || TrakingNumber == "undefined")
                TrakingNumber = "";
            if (string.IsNullOrEmpty(Status) || Status == "undefined")
                Status = "";
            if (string.IsNullOrEmpty(Type) || Type == "undefined")
                Type = "";
            List<ShipmentGetDataViewModel> list = new List<ShipmentGetDataViewModel>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetShipmentslistCopy", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_VendorId", VendorId);
                    cmd.Parameters.AddWithValue("_Limit", Limit);
                    cmd.Parameters.AddWithValue("_OffSet", Offset);
                    cmd.Parameters.AddWithValue("_ShipmentId", ShipmentId);
                    cmd.Parameters.AddWithValue("_TrankingNumber", TrakingNumber);
                    cmd.Parameters.AddWithValue("_Status", Status);
                    cmd.Parameters.AddWithValue("_Type", Type);
                    cmd.Parameters.AddWithValue("dateFrom", PreviousDate);
                    cmd.Parameters.AddWithValue("dateTo", CurrentDate);
                    DataSet ds = new DataSet();
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(ds);
                    DataTable dt = ds.Tables[0];
                    DataTable dt2 = ds.Tables[1];
                    foreach (DataRow reader in dt.Rows)
                    {
                        ShipmentGetDataViewModel viewModel = new ShipmentGetDataViewModel
                        {
                            ShipmentAutoID = (int)reader["ShipmentAutoID"],
                            ShipmentId = (string)reader["ShipmentId"],
                            ShipmentName = reader["ShipmentName"] != DBNull.Value ? (string)reader["ShipmentName"] : "",
                            Vendor = reader["Vendor"] != DBNull.Value ? (string)reader["Vendor"] : "",
                            VendorId = (int)reader["VendorId"],
                            Status = reader["Status"] != DBNull.Value ? Convert.ToInt32(reader["Status"]) : 0,
                            CreatedOn = (DateTime)reader["CreatedOn"],
                            Notes = reader["Notes"] != DBNull.Value ? (string)reader["Notes"] : "",
                            NoOfBoxes = reader["NoOfBoxs"] != DBNull.Value ? Convert.ToInt32(reader["NoOfBoxs"]) : 0,
                            TrakingNumber = reader["TrakingNumber"] != DBNull.Value ? (string)reader["TrakingNumber"] : "",
                            TrakingURL = reader["TrakingURL"] != DBNull.Value ? (string)reader["TrakingURL"] : "",
                            CourierCode = reader["CourierCode"] != DBNull.Value ? (string)reader["CourierCode"] : "",
                            ShippedDate = reader["ShippedDate"] != DBNull.Value ? Convert.ToDateTime(reader["ShippedDate"]) : DateTime.MinValue,
                            ReceivedDate = reader["ReceivedDate"] != DBNull.Value ? Convert.ToDateTime(reader["ReceivedDate"]) : DateTime.MinValue,
                            ExpectedDelivery = reader["Expected_Delivery_Shipped_PO"] != DBNull.Value ? Convert.ToDateTime(reader["Expected_Delivery_Shipped_PO"]) : DateTime.MinValue,
                            GrossWt = reader["GrossWt"] != DBNull.Value ? Convert.ToDecimal(reader["GrossWt"]) : 0,
                            Type = reader["Type"] != DBNull.Value ? Convert.ToString(reader["Type"]) : "",


                            // NoOfSKUs = reader["NoOfSKUs"] != DBNull.Value ? Convert.ToInt32(reader["NoOfSKUs"]) : 0,
                            //TotalShipedQty = reader["TotalShipedQty"] != DBNull.Value ? Convert.ToInt32(reader["TotalShipedQty"]) : 0,
                        };
                        list.Add(viewModel);
                    }
                    foreach (DataRow reader in dt2.Rows)
                    {
                        ShipmentGetDataViewModel viewModel = new ShipmentGetDataViewModel
                        {
                            ShipmentId = (string)reader["ShipmentId"],
                            NoOfSKUs = reader["NoOfSKUs"] != DBNull.Value ? Convert.ToInt32(reader["NoOfSKUs"]) : 0,
                            TotalShipedQty = reader["TotalShipedQty"] != DBNull.Value ? Convert.ToInt32(reader["TotalShipedQty"]) : 0,
                            NoOfPOs = reader["NoOfPOs"] != DBNull.Value ? Convert.ToInt32(reader["NoOfPOs"]) : 0,
                            QtyReceived = reader["QtyReceived"] != DBNull.Value ? Convert.ToInt32(reader["QtyReceived"]) : 0,
                            NoOfBoxes = reader["Boxs"] != DBNull.Value ? Convert.ToInt32(reader["Boxs"]) : 0,
                            AmountReceived = reader["AmountReceived"] != DBNull.Value ? Convert.ToDecimal(reader["AmountReceived"]) : 0,
                            RecivedAmountCNY = reader["RecivedAmountCNY"] != DBNull.Value ? Convert.ToDecimal(reader["RecivedAmountCNY"]) : 0,
                            GrossWt = reader["GrossWt"] != DBNull.Value ? Convert.ToDecimal(reader["GrossWt"]) : 0,
                        };
                        var item = list.Where(s => s.ShipmentId == viewModel.ShipmentId).FirstOrDefault();
                        if (item != null)
                        {
                            item.NoOfSKUs = viewModel.NoOfSKUs;
                            item.TotalShipedQty = viewModel.TotalShipedQty;
                            item.NoOfPOs = viewModel.NoOfPOs;
                            item.QtyReceived = viewModel.QtyReceived;
                            item.AmountReceived = viewModel.AmountReceived;
                            item.RecivedAmountCNY = viewModel.RecivedAmountCNY;
                            if (item.NoOfBoxes == 0 && viewModel.NoOfBoxes > 0)
                            {
                                item.GrossWt = viewModel.GrossWt;
                                item.NoOfBoxes = viewModel.NoOfBoxes;
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

        public ShipmentHeaderViewModel GetShipmentByShipmentId(string ShipmentId)
        {
            ShipmentHeaderViewModel Item = new ShipmentHeaderViewModel();
            List<ShipmentBoxListViewModel> list = new List<ShipmentBoxListViewModel>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetShipmentsByShipmentId", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Id", ShipmentId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                ShipmentHeaderViewModel viewModel = new ShipmentHeaderViewModel
                                {
                                    ShipmentAutoID = (int)reader["ShipmentAutoID"],
                                    ShipmentId = (string)reader["ShipmentId"],
                                    ShipmentName = reader["ShipmentName"] != DBNull.Value ? (string)reader["ShipmentName"] : "",
                                    Vendor = reader["Vendor"] != DBNull.Value ? (string)reader["Vendor"] : "",
                                    VendorId = (int)reader["VendorId"],
                                    Status = reader["Status"] != DBNull.Value ? Convert.ToInt32(reader["Status"]) : 0,
                                    CreatedOn = (DateTime)reader["CreatedOn"],
                                    Notes = reader["Notes"] != DBNull.Value ? (string)reader["Notes"] : "",

                                };
                                Item = viewModel;
                            }
                        }
                    }
                    conn.Close();
                }
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {

                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetShipmentBoxsByShipmentId", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Id", ShipmentId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                ShipmentBoxListViewModel viewModel = new ShipmentBoxListViewModel
                                {
                                    IdShipmentBox = (int)reader["idShipmentBox"],
                                    ShipmentId = (string)reader["ShipmentId"],
                                    BoxId = reader["BoxId"] != DBNull.Value ? (string)reader["BoxId"] : "",
                                    //Vendor = reader["Vendor"] != DBNull.Value ? (string)reader["Vendor"] : "",
                                    //VendorId = (int)reader["VendorId"],
                                    //Status = reader["Status"] != DBNull.Value ? (string)reader["Status"] : "",
                                    //CreatedOn = (DateTime)reader["CreatedOn"],
                                    Width = reader["Width"] != DBNull.Value ? (Decimal)reader["Width"] : 0,
                                    Height = reader["Height"] != DBNull.Value ? (Decimal)reader["Height"] : 0,
                                    Length = reader["Length"] != DBNull.Value ? (Decimal)reader["Length"] : 0,
                                    Weight = reader["Weight"] != DBNull.Value ? (Decimal)reader["Weight"] : 0,
                                    TotalShipedQty = reader["TotalShipedQty"] != DBNull.Value ? Convert.ToInt32(reader["TotalShipedQty"]) : 0,
                                    TotalSKUs = reader["TotalSKUs"] != DBNull.Value ? Convert.ToInt32(reader["TotalSKUs"]) : 0,
                                    TotalPOs = reader["TotalPOs"] != DBNull.Value ? Convert.ToInt32(reader["TotalPOs"]) : 0,
                                };
                                list.Add(viewModel);
                            }
                        }
                    }
                    conn.Close();

                }
                Item.List = list;
            }
            catch (Exception ex)
            {

            }
            return Item;
        }
        public bool UpdateShipment(ShipmentViewModel viewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_UpdateShipment", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("Id", viewModel.ShipmentAutoID);
                    cmd.Parameters.AddWithValue("_Notes", viewModel.Notes);
                    cmd.Parameters.AddWithValue("_ShipmentName", viewModel.ShipmentName);
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
        public bool DeleteShipment(string Id)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmdd = new MySqlCommand("p_DeleteShipment", conn);
                    cmdd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmdd.Parameters.AddWithValue("_Id", Id);
                    cmdd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {

            }
            return status;
        }
    
        public ShipmentViewHeaderViewModel GetShipmentViewHeaderdetail(string ShipmentId)
        {
            var Item = new ShipmentViewHeaderViewModel();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetShipmentViewHeader", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Id", ShipmentId);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    foreach (DataRow reader in dt.Rows)
                    {
                        var viewModel = new ShipmentViewHeaderViewModel()
                        {
                            ShipingCompany = reader["ShipingCompany"] != DBNull.Value ? Convert.ToString(reader["ShipingCompany"]) : "",
                            ShipmentId = reader["ShipmentId"] != DBNull.Value ? (string)reader["ShipmentId"] : "",
                            ShipmentName = reader["ShipmentName"] != DBNull.Value ? (string)reader["ShipmentName"] : "",
                            Vendor = reader["Vendor"] != DBNull.Value ? (string)reader["Vendor"] : "",
                            VendorId = (int)reader["VendorId"],
                            CreatedOn = (DateTime)reader["CreatedOn"],
                            Notes = reader["Notes"] != DBNull.Value ? (string)reader["Notes"] : "",
                            Boxes = reader["Boxes"] != DBNull.Value ? Convert.ToInt32(reader["Boxes"]) : 0,
                            SKUs = reader["SKUs"] != DBNull.Value ? Convert.ToInt32(reader["SKUs"]) : 0,
                            POs = reader["POs"] != DBNull.Value ? Convert.ToInt32(reader["POs"]) : 0,
                            Status = reader["Status"] != DBNull.Value ? Convert.ToInt32(reader["Status"]) : 0,
                            ShipedAmountUSD = reader["ShipedAmountUSD"] != DBNull.Value ? Convert.ToDecimal(reader["ShipedAmountUSD"]) : 0,
                            ReceivedAmountUSD = reader["ReceivedAmountUSD"] != DBNull.Value ? Convert.ToDecimal(reader["ReceivedAmountUSD"]) : 0,
                            ShipedAmountCNY = reader["ShipedAmountCNY"] != DBNull.Value ? Convert.ToDecimal(reader["ShipedAmountCNY"]) : 0,
                            ReceivedAmountCNY = reader["ReceivedAmountCNY"] != DBNull.Value ? Convert.ToDecimal(reader["ReceivedAmountCNY"]) : 0,
                            TotalReceivedQty = reader["TotalReceivedQty"] != DBNull.Value ? Convert.ToInt32(reader["TotalReceivedQty"]) : 0,
                            TotalOrderedQty = reader["TotalOrderedQty"] != DBNull.Value ? Convert.ToInt32(reader["TotalOrderedQty"]) : 0,
                            TotalOpenQty = reader["TotalOpenQty"] != DBNull.Value ? Convert.ToInt32(reader["TotalOpenQty"]) : 0,
                            TotalShipedQty = reader["TotalShipedQty"] != DBNull.Value ? Convert.ToInt32(reader["TotalShipedQty"]) : 0,
                            ShippedDate = reader["CreatedAt"] != DBNull.Value ? Convert.ToDateTime(reader["CreatedAt"]) : DateTime.MinValue,
                            ReceivedDate = reader["ReceivedDate"] != DBNull.Value ? Convert.ToDateTime(reader["ReceivedDate"]) : DateTime.MinValue,
                            ExpectedDelivery = Convert.ToDateTime(reader["Expected_Delivery_Shipped_PO"] != DBNull.Value ? reader["Expected_Delivery_Shipped_PO"] : DateTime.MinValue),
                            TrakingNumber = reader["TrakingNumber"] != DBNull.Value ? (string)reader["TrakingNumber"] : "",
                            TrakingURL = reader["TrakingURL"] != DBNull.Value ? (string)reader["TrakingURL"] : "",
                            CourierCode = reader["CourierCode"] != DBNull.Value ? (string)reader["CourierCode"] : "",
                            GrossWt = reader["GrossWt"] != DBNull.Value ? Convert.ToDecimal(reader["GrossWt"]) : 0,
                            CBM = reader["CBM"] != DBNull.Value ? Convert.ToDecimal(reader["CBM"]) : 0,
                        };
                        Item = viewModel;
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
            }
            return Item;
        }

        public int GetShipmentViewProductsListCount(string ShipmentId, int POID, string SKU = "", string Title = "", string OpenItem = "", string ReceivedItem = "", string OrderdItem = "")
        {
            int counter = 0;
            try
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
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetShipmentViewProductListCount", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Id", ShipmentId);
                    cmd.Parameters.AddWithValue("_SKU", SKU);
                    cmd.Parameters.AddWithValue("_Title", Title);
                    cmd.Parameters.AddWithValue("_OpenItem", OpenItem);
                    cmd.Parameters.AddWithValue("_OrderedItem", OrderdItem);
                    cmd.Parameters.AddWithValue("_ReceivedItem", ReceivedItem);
                    cmd.Parameters.AddWithValue("_POID", POID);
                    DataTable dt = new DataTable();
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                    conn.Close();
                    counter = dt.Rows.Count;
                }
            }
            catch (Exception exp)
            {

            }
            return counter;
        }

        public List<ShipmentViewProducListViewModel> GetShipmentViewProductsList(string ShipmentId, int StartLimit, int EndLimit, int POID, string SKU = "", string Title = "", string OpenItem = "", string ReceivedItem = "", string OrderdItem = "")
        {
            List<ShipmentViewProducListViewModel> list = new List<ShipmentViewProducListViewModel>();
            try
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
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetShipmentViewProductList", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Id", ShipmentId);

                    cmd.Parameters.AddWithValue("_Limit", StartLimit);
                    cmd.Parameters.AddWithValue("_OffSet", EndLimit);
                    cmd.Parameters.AddWithValue("_SKU", SKU);
                    cmd.Parameters.AddWithValue("_Title", Title);
                    cmd.Parameters.AddWithValue("_OpenItem", OpenItem);
                    cmd.Parameters.AddWithValue("_OrderedItem", OrderdItem);
                    cmd.Parameters.AddWithValue("_ReceivedItem", ReceivedItem);
                    cmd.Parameters.AddWithValue("_POID", POID);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                ShipmentViewProducListViewModel viewModel = new ShipmentViewProducListViewModel
                                {
                                    BoxId = reader["BoxId"] != DBNull.Value ? (string)reader["BoxId"] : "",
                                    SKU = reader["SKU"] != DBNull.Value ? (string)reader["SKU"] : "",
                                    Title = reader["title"] != DBNull.Value ? (string)reader["title"] : "",
                                    Description = reader["description"] != DBNull.Value ? (string)reader["description"] : "",
                                    CompressedImage = reader["Compress_image"] != DBNull.Value ? (string)reader["Compress_image"] : "",
                                    ImageName = reader["image_name"] != DBNull.Value ? (string)reader["image_name"] : "",
                                    POId = reader["POId"] != DBNull.Value ? (int)reader["POId"] : 0,
                                    OpenQty = reader["QtyOpen"] != DBNull.Value ? (int)reader["QtyOpen"] : 0,
                                    ShipedQty = reader["ShipedQty"] != DBNull.Value ? (int)reader["ShipedQty"] : 0,
                                    ReceivedQty = reader["RecivedQty"] != DBNull.Value ? (int)reader["RecivedQty"] : 0,
                                    OrderedQty = reader["QtyOrdered"] != DBNull.Value ? (int)reader["QtyOrdered"] : 0,
                                    UnitPrice = reader["UnitPrice"] != DBNull.Value ? Convert.ToDecimal(reader["UnitPrice"]) : 0,
                                    UnitPriceUSD = reader["UnitPriceUSD"] != DBNull.Value ? Convert.ToDecimal(reader["UnitPriceUSD"]) : 0,
                                    CurrencyCode = reader["CurrencyCode"] != DBNull.Value ? Convert.ToInt32(reader["CurrencyCode"]) : 0,
                                    BoxNo = reader["BoxNo"] != DBNull.Value ? Convert.ToInt32(reader["BoxNo"]) : 0,
                                };
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

        public bool DeleteShipmentCourier(string ShipmentId)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmdd = new MySqlCommand("p_DeleteShipmentCourierDetails", conn);
                    cmdd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmdd.Parameters.AddWithValue("_ShipmentId", ShipmentId);
                    cmdd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {

            }
            return status;
        }

        public List<ProductSKUViewModel> GetAllSKUForAutoComplete(string name, string ShipmentId)
        {
            List<ProductSKUViewModel> listViewModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetSkuForAutoCompletefromShipment", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_name", name.Trim());
                    cmd.Parameters.AddWithValue("_Id", ShipmentId);
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

        public GetShipedAndRecQtyViewModel GetShipmentHistoryCount(string DateTo, string DateFrom, int VendorId, string ShipmentId, string SKU = "", string Title = "", string Status = "")
        {
            int counter = 0;
            GetShipedAndRecQtyViewModel model = new GetShipedAndRecQtyViewModel();

            try
            {
                if (string.IsNullOrEmpty(SKU) || SKU == "undefined")
                    SKU = "";
                if (string.IsNullOrEmpty(ShipmentId) || ShipmentId == "undefined")
                    ShipmentId = "";
                if (string.IsNullOrEmpty(Title) || Title == "undefined")
                    Title = "";
                if (string.IsNullOrEmpty(Status) || Status == "undefined")
                    Status = "";
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                   // MySqlCommand cmd = new MySqlCommand("p_GetShipmentHistoryCount", conn);
                    MySqlCommand cmd = new MySqlCommand("P_GetShipmentHistoryReportCount", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_VendorId", VendorId);
                    cmd.Parameters.AddWithValue("_SKU", SKU);
                    cmd.Parameters.AddWithValue("_ShipmentId", ShipmentId);
                    cmd.Parameters.AddWithValue("_Title", Title);
                    cmd.Parameters.AddWithValue("_Status", Status);
                    cmd.Parameters.AddWithValue("dateFrom", DateFrom);
                    cmd.Parameters.AddWithValue("dateTo", DateTo);
                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                GetShipedAndRecQtyViewModel viewModel = new GetShipedAndRecQtyViewModel()
                                {
                                   ShipedQty = Convert.ToDouble(dr["ShippedQTY"] != DBNull.Value ? dr["ShippedQTY"] : 0),
                                   RecivedQty = Convert.ToDouble(dr["ReceivedQTY"] != DBNull.Value ? dr["ReceivedQTY"] : 0),
                                   TotalCount = Convert.ToDouble(dr["counter"] != DBNull.Value ? dr["counter"] : 0),
                                };
                                model = viewModel;
                            }
                        }

                    }
                    conn.Close();
                }
            }
            catch (Exception exp)
            {

            }
            return model;
        }

        public List<ShipmentHistoryViewModel> GetShipmentHistoryList(string DateTo, string DateFrom, int VendorId, string ShipmentId, string SKU, string Title, int limit, int offset, string Status)
        {
            if (string.IsNullOrEmpty(SKU) || SKU == "undefined")
                SKU = "";
            if (string.IsNullOrEmpty(ShipmentId) || ShipmentId == "undefined")
                ShipmentId = "";
            if (string.IsNullOrEmpty(Title) || Title == "undefined")
                Title = "";
            if (string.IsNullOrEmpty(Status) || Status == "undefined")
                Status = "";
            List<ShipmentHistoryViewModel> list = new List<ShipmentHistoryViewModel>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                  //  MySqlCommand cmd = new MySqlCommand("p_GetShipmentHistoryList", conn);
                    MySqlCommand cmd = new MySqlCommand("P_GetShipmentHistoryReport", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_VendorId", VendorId);
                    cmd.Parameters.AddWithValue("_SKU", SKU);
                    cmd.Parameters.AddWithValue("_ShipmentId", ShipmentId);
                    cmd.Parameters.AddWithValue("_Title", Title);
                    cmd.Parameters.AddWithValue("_Status", Status);
                    cmd.Parameters.AddWithValue("dateFrom", DateFrom);
                    cmd.Parameters.AddWithValue("dateTo", DateTo);
                    cmd.Parameters.AddWithValue("_Limit", limit);
                    cmd.Parameters.AddWithValue("_OffSet", offset);
                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                ShipmentHistoryViewModel viewModel = new ShipmentHistoryViewModel
                                {
                                    ShipmentId = Convert.ToString(dr["ShipmentId"] != DBNull.Value ? dr["ShipmentId"] : "0"),
                                    Vendor = Convert.ToString(dr["VendorName"] != DBNull.Value ? dr["VendorName"] : ""),
                                    VendorId = Convert.ToInt32(dr["VendorId"] != DBNull.Value ? dr["VendorId"] : "0"),
                                    SKU = Convert.ToString(dr["SKU"] != DBNull.Value ? dr["SKU"] : "0"),
                                    Title = Convert.ToString(dr["title"] != DBNull.Value ? dr["title"] : "0"),
                                    ReceivedDate = Convert.ToDateTime(dr["ReceivedOn"] != DBNull.Value ? dr["ReceivedOn"] : DateTime.MinValue),
                                    ExpectedDelivery = Convert.ToDateTime(dr["Expected_Delivery_Shipped_PO"] != DBNull.Value ? dr["Expected_Delivery_Shipped_PO"] : DateTime.MinValue),
                                    ShippedDate = Convert.ToDateTime(dr["ShippdeOn"] != DBNull.Value ? dr["ShippdeOn"] : DateTime.MinValue),
                                    CreatedOn = Convert.ToDateTime(dr["CreatedOn"] != DBNull.Value ? dr["CreatedOn"] : DateTime.MinValue),
                                    ShipedQty = Convert.ToInt32(dr["ShippedQTY"] != DBNull.Value ? dr["ShippedQTY"] : 0),
                                    ReceivedQty = Convert.ToInt32(dr["ReceivedQTY"] != DBNull.Value ? dr["ReceivedQTY"] : 0),
                                    CompressedImage = Convert.ToString(dr["Compress_image"] != DBNull.Value ? dr["Compress_image"] : ""),
                                    ImageName = Convert.ToString(dr["image_name"] != DBNull.Value ? dr["image_name"] : ""),
                                    Type = Convert.ToString(dr["Type"] != DBNull.Value ? dr["Type"] : ""),
                                    Status = Convert.ToInt32(dr["Status"] != DBNull.Value ? dr["Status"] : 0),                                   
                                    TrakingNumber = Convert.ToString(dr["TrakingNumber"] != DBNull.Value ? dr["TrakingNumber"] : ""),
                                    CourierCode = Convert.ToString(dr["CourierCode"] != DBNull.Value ? dr["CourierCode"] : ""),
                                    TrakingURL = Convert.ToString(dr["TrakingURL"] != DBNull.Value ? dr["TrakingURL"] : ""),
                                    POId = Convert.ToInt32(dr["POId"] != DBNull.Value ? dr["POId"] : "0"),
                                };
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
        public List<ShipmentHistoryViewModel> GetShipmentHistoryListforReport(string DateTo, string DateFrom, int VendorId, string ShipmentId, string SKU, string Title, int limit, int offset, string Status)
        {
            if (string.IsNullOrEmpty(SKU) || SKU == "undefined")
                SKU = "";
            if (string.IsNullOrEmpty(ShipmentId) || ShipmentId == "undefined")
                ShipmentId = "";
            if (string.IsNullOrEmpty(Title) || Title == "undefined")
                Title = "";
            if (string.IsNullOrEmpty(Status) || Status == "undefined")
                Status = "";
            List<ShipmentHistoryViewModel> list = new List<ShipmentHistoryViewModel>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                      MySqlCommand cmd = new MySqlCommand("p_GetShipmentHistoryList", conn);
                  //  MySqlCommand cmd = new MySqlCommand("P_GetShipmentHistoryReport", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_VendorId", VendorId);
                    cmd.Parameters.AddWithValue("_SKU", SKU);
                    cmd.Parameters.AddWithValue("_ShipmentId", ShipmentId);
                    cmd.Parameters.AddWithValue("_Title", Title);
                    cmd.Parameters.AddWithValue("_Status", Status);
                    cmd.Parameters.AddWithValue("dateFrom", DateFrom);
                    cmd.Parameters.AddWithValue("dateTo", DateTo);
                    cmd.Parameters.AddWithValue("_Limit", limit);
                    cmd.Parameters.AddWithValue("_OffSet", offset);
                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                ShipmentHistoryViewModel viewModel = new ShipmentHistoryViewModel
                                {
                                    ShipmentId = Convert.ToString(dr["ShipmentId"] != DBNull.Value ? dr["ShipmentId"] : "0"),
                                    Vendor = Convert.ToString(dr["Vendor"] != DBNull.Value ? dr["Vendor"] : ""),
                                    VendorId = Convert.ToInt32(dr["VendorId"] != DBNull.Value ? dr["VendorId"] : "0"),
                                    SKU = Convert.ToString(dr["SKU"] != DBNull.Value ? dr["SKU"] : "0"),
                                    Title = Convert.ToString(dr["title"] != DBNull.Value ? dr["title"] : "0"),
                                    ReceivedDate = Convert.ToDateTime(dr["ReceivedDate"] != DBNull.Value ? dr["ReceivedDate"] : DateTime.MinValue),
                                    ShippedDate = Convert.ToDateTime(dr["ShipedDate"] != DBNull.Value ? dr["ShipedDate"] : DateTime.MinValue),
                                    ExpectedDelivery = Convert.ToDateTime(dr["Expected_Delivery_Shipped_PO"] != DBNull.Value ? dr["Expected_Delivery_Shipped_PO"] : DateTime.MinValue),
                                    CreatedOn = Convert.ToDateTime(dr["CreatedOn"] != DBNull.Value ? dr["CreatedOn"] : DateTime.MinValue),
                                    ShipedQty = Convert.ToInt32(dr["ShipedQty"] != DBNull.Value ? dr["ShipedQty"] : 0),
                                    ReceivedQty = Convert.ToInt32(dr["RecivedQty"] != DBNull.Value ? dr["RecivedQty"] : 0),
                                    CompressedImage = Convert.ToString(dr["Compress_image"] != DBNull.Value ? dr["Compress_image"] : ""),
                                    ImageName = Convert.ToString(dr["image_name"] != DBNull.Value ? dr["image_name"] : ""),
                                    Type = Convert.ToString(dr["Type"] != DBNull.Value ? dr["Type"] : ""),
                                    Status = Convert.ToInt32(dr["Status"] != DBNull.Value ? dr["Status"] : 0),
                                    TrakingNumber = Convert.ToString(dr["TrakingNumber"] != DBNull.Value ? dr["TrakingNumber"] : ""),
                                    CourierCode = Convert.ToString(dr["CourierCode"] != DBNull.Value ? dr["CourierCode"] : ""),
                                    TrakingURL = Convert.ToString(dr["TrakingURL"] != DBNull.Value ? dr["TrakingURL"] : ""),
                                    POId = Convert.ToInt32(dr["POId"] != DBNull.Value ? dr["POId"] : "0"),
                                };
                                list.Add(viewModel);
                            }
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }
        public List<GetTemZincDataViewModel> GetDataFromTempZinc()
        {
            List<GetTemZincDataViewModel> list = new List<GetTemZincDataViewModel>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetDataFromTempZincDetails", conn);
                    //  MySqlCommand cmd = new MySqlCommand("P_GetShipmentHistoryReport", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                GetTemZincDataViewModel viewModel = new GetTemZincDataViewModel
                                {
                                    order_type = Convert.ToString(dr["order_type"] != DBNull.Value ? dr["order_type"] : "0"),
                                    request_id = Convert.ToString(dr["request_id"] != DBNull.Value ? dr["request_id"] : "0"),
                                    sc_order_id = Convert.ToString(dr["sc_order_id"] != DBNull.Value ? dr["sc_order_id"] : "0"),
                                    zinc_order_status_internal = Convert.ToString(dr["zinc_order_status_internal"] != DBNull.Value ? dr["zinc_order_status_internal"] :""),
                                    order_code = Convert.ToString(dr["order_code"] != DBNull.Value ? dr["order_code"] :""),
                                    order_message = Convert.ToString(dr["order_message"] != DBNull.Value ? dr["order_message"] : ""),
                                   
                                };
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

        public List<ShipmentHistoryViewModel> GetShipmentHistoryBySKU(int POID, string SKU)
        {
            if (string.IsNullOrEmpty(SKU) || SKU == "undefined")
                SKU = "";
            List<ShipmentHistoryViewModel> list = new List<ShipmentHistoryViewModel>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetShipmentHistoryBySKU", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_SKU", SKU);
                    cmd.Parameters.AddWithValue("_POID", POID);
                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                ShipmentHistoryViewModel viewModel = new ShipmentHistoryViewModel
                                {
                                    ShipmentId = Convert.ToString(dr["ShipmentId"] != DBNull.Value ? dr["ShipmentId"] : "0"),
                                    Vendor = Convert.ToString(dr["Vendor"] != DBNull.Value ? dr["Vendor"] : "0"),
                                    VendorId = Convert.ToInt32(dr["VendorId"] != DBNull.Value ? dr["VendorId"] : "0"),
                                    SKU = Convert.ToString(dr["SKU"] != DBNull.Value ? dr["SKU"] : "0"),
                                    Title = Convert.ToString(dr["title"] != DBNull.Value ? dr["title"] : "0"),
                                    ReceivedDate = Convert.ToDateTime(dr["ReceivedDate"] != DBNull.Value ? dr["ReceivedDate"] : DateTime.MinValue),
                                    ShippedDate = Convert.ToDateTime(dr["ShipedDate"] != DBNull.Value ? dr["ShipedDate"] : DateTime.MinValue),
                                    CreatedOn = Convert.ToDateTime(dr["CreatedOn"] != DBNull.Value ? dr["CreatedOn"] : DateTime.MinValue),
                                    ShipedQty = Convert.ToInt32(dr["ShipedQty"] != DBNull.Value ? dr["ShipedQty"] : "0"),
                                    ReceivedQty = Convert.ToInt32(dr["RecivedQty"] != DBNull.Value ? dr["RecivedQty"] : "0"),
                                    CompressedImage = Convert.ToString(dr["Compress_image"] != DBNull.Value ? dr["Compress_image"] : ""),
                                    ImageName = Convert.ToString(dr["image_name"] != DBNull.Value ? dr["image_name"] : ""),
                                    Type = Convert.ToString(dr["Type"] != DBNull.Value ? dr["Type"] : ""),
                                    Status = Convert.ToInt32(dr["Status"] != DBNull.Value ? dr["Status"] : 0),
                                    POId = Convert.ToInt32(dr["POId"] != DBNull.Value ? dr["POId"] : "0"),
                                };
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
        public ShipmentCourierInfoViewModel GetShipmentCourierInfo(string ShipmentId)
        {
            var Item = new ShipmentCourierInfoViewModel();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetTrackingNumberForEdit", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_ShipmentId", ShipmentId);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    foreach (DataRow reader in dt.Rows)
                    {
                        var viewModel = new ShipmentCourierInfoViewModel()
                        {
                            CourierCode = reader["CourierCode"] != DBNull.Value ? Convert.ToString(reader["CourierCode"]) : "",
                            TrakingNumber = reader["TrakingNumber"] != DBNull.Value ? (string)reader["TrakingNumber"] : "",

                            CreatedAt = reader["CreatedAt"] != DBNull.Value ? Convert.ToDateTime(reader["CreatedAt"]) : DateTime.MinValue,
                        };
                        Item = viewModel;
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
            }
            return Item;
        }

        public bool UpdateShipmentCourierInfo(ShipmentCourierInfoViewModel viewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_UpdateShipmentCourierInfo", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_ShipmentId", viewModel.ShipmentId);
                    cmd.Parameters.AddWithValue("_CourierCode", viewModel.CourierCode);
                    cmd.Parameters.AddWithValue("_TrakingNumber", viewModel.TrakingNumber);
                    cmd.Parameters.AddWithValue("_CreatedAt", viewModel.CreatedAt);
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


        public bool UpdateShipmentHistoryReport(List<ShipmentHistoryViewModel> Data)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    foreach (var viewModel in Data)
                    {
                        MySqlCommand cmd = new MySqlCommand("P_SaveShipmentHistoryReport", conn);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("_ShipmentID", viewModel.ShipmentId);
                        cmd.Parameters.AddWithValue("_SKU", viewModel.SKU);
                        cmd.Parameters.AddWithValue("_VendorID", viewModel.VendorId);
                        cmd.Parameters.AddWithValue("_VendorName", viewModel.Vendor);
                        cmd.Parameters.AddWithValue("_ShippedQTY", viewModel.ShipedQty);
                        cmd.Parameters.AddWithValue("_ReceivedQTY", viewModel.ReceivedQty);
                        cmd.Parameters.AddWithValue("_CreatedOn", viewModel.CreatedOn);
                        cmd.Parameters.AddWithValue("_ShippdeOn", viewModel.ShippedDate);
                        cmd.Parameters.AddWithValue("_ReceivedOn", viewModel.ReceivedDate);
                        cmd.Parameters.AddWithValue("_ExpectedDelivery", viewModel.ExpectedDelivery);
                        cmd.Parameters.AddWithValue("_Status", viewModel.Status);
                        cmd.Parameters.AddWithValue("_Type", viewModel.Type);
                        cmd.Parameters.AddWithValue("_POId", viewModel.POId);
                        cmd.Parameters.AddWithValue("_title", viewModel.Title);
                        cmd.Parameters.AddWithValue("_image_name", viewModel.ImageName);
                        cmd.Parameters.AddWithValue("_Compress_image", viewModel.CompressedImage);
                        cmd.Parameters.AddWithValue("_vendortitle", viewModel.Title);
                        cmd.Parameters.AddWithValue("_TrakingNumber", viewModel.TrakingNumber);
                        cmd.Parameters.AddWithValue("_TrakingURL", viewModel.TrakingURL);
                        cmd.Parameters.AddWithValue("_CourierCode", viewModel.CourierCode);
                        cmd.ExecuteNonQuery();
                    }
                  
                    conn.Close();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }

        public bool UpdateTempZincdata(List<GetTemZincDataViewModel> Data)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    foreach (var viewModel in Data)
                    {
                        MySqlCommand cmd = new MySqlCommand("P_UpdateTempDataToSCOrders", conn);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("_order_type", viewModel.order_type);
                        cmd.Parameters.AddWithValue("_request_id", viewModel.request_id);
                        cmd.Parameters.AddWithValue("_sc_order_id", viewModel.sc_order_id);
                        cmd.Parameters.AddWithValue("_zinc_order_status_internal", viewModel.zinc_order_status_internal);
                        cmd.Parameters.AddWithValue("_order_code", viewModel.order_code);
                        cmd.Parameters.AddWithValue("_order_message", viewModel.order_message);
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public bool UpdateExpectedDelivery(Expected_Delivery_Shipped_POViewModel ViewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_UpdateExpectedDelivery", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_ShipmentId", ViewModel.ShipmentId);
                    cmd.Parameters.AddWithValue("_Expected_Delivery_Shipped_PO", ViewModel.ExpectedDelivery);
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }
        public Expected_Delivery_Shipped_POViewModel GetDeliveryDateById(string id)
        {
            Expected_Delivery_Shipped_POViewModel ViewModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("GetDeliveryDateById", conn);
                    cmd.Parameters.AddWithValue("_ShipmentId", id);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            ViewModel = new Expected_Delivery_Shipped_POViewModel();
                            while (reader.Read())
                            {
                                ViewModel.ShipmentId = Convert.ToString(reader["ShipmentId"]);
                                ViewModel.ExpectedDelivery = Convert.ToDateTime(reader["Expected_Delivery_Shipped_PO"]);
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

        public string BBtrackingCodes(BBtrackingCodesViewModel ViewModel)
        {
            string status = "false";
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmdd = new MySqlCommand("p_SaveBBtrackingCodes", conn);
                    cmdd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmdd.Parameters.AddWithValue("_CourierId", ViewModel.IdBBtrackingCodes);
                    cmdd.Parameters.AddWithValue("_CarrierCode", ViewModel.CarrierCode);
                    cmdd.Parameters.AddWithValue("_CarrierName", ViewModel.CarrierName);
                    cmdd.Parameters.AddWithValue("_CarrierUrl", ViewModel.CarrierUrl);
                    cmdd.Parameters.AddWithValue("_TrackingNumberCode", ViewModel.TrackingNumberCode);
                
                    cmdd.ExecuteNonQuery();
                    status = "true";
                }
            }
            catch (Exception ex)
            {

            }
            return status;
        }
        public List<BBtrackingCodesViewModel> GetBBtrackingCodesList()
        {
            List<BBtrackingCodesViewModel> listViewModel = new List<BBtrackingCodesViewModel>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetBBtrackingCodes", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            //listViewModel = new List<BBtrackingCodesViewModel>();
                            while (reader.Read())
                            {
                                BBtrackingCodesViewModel model = new BBtrackingCodesViewModel();
                                model.IdBBtrackingCodes = Convert.ToInt32(reader["IdBBtrackingCodes"] != DBNull.Value ? reader["IdBBtrackingCodes"] : 0);
                                model.CarrierCode = Convert.ToString(reader["CarrierCode"] != DBNull.Value ? reader["CarrierCode"] : " ");
                                model.CarrierName = Convert.ToString(reader["CarrierName"] != DBNull.Value ? reader["CarrierName"] : " ");
                                model.CarrierUrl = Convert.ToString(reader["CarrierUrl"] != DBNull.Value ? reader["CarrierUrl"] : " ");
                                model.TrackingNumberCode = Convert.ToString(reader["TrackingNumberCode"] != DBNull.Value ? reader["TrackingNumberCode"] : " ");
                                listViewModel.Add(model);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listViewModel;
        }

        public BBtrackingCodesViewModel EditBBtrackingCodesById(int id)
        {
            BBtrackingCodesViewModel model = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmdd = new MySqlCommand(@"SELECT * FROM bestBuyE2.BBtrackingCodes where IdBBtrackingCodes=" + id, conn);
                    cmdd.CommandType = System.Data.CommandType.Text;
                    using (var reader = cmdd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())

                            {
                                model = new BBtrackingCodesViewModel();
                                model.CarrierCode = Convert.ToString(reader["CarrierCode"]);
                                model.CarrierName = Convert.ToString(reader["CarrierName"]);
                                model.CarrierUrl = Convert.ToString(reader["CarrierUrl"]);
                                model.TrackingNumberCode = Convert.ToString(reader["TrackingNumberCode"]);
                                model.IdBBtrackingCodes = Convert.ToInt32(reader["IdBBtrackingCodes"]);
                               


                            }

                        }
                    }

                }


                return model;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public bool CheckTrackingNumberExists(string name)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_CheckTrackingNumberExists", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_trackingnumber", name.Trim());
                    cmd.Parameters.Add("Statues", MySqlDbType.Bit, 10);
                    cmd.Parameters["Statues"].Direction = System.Data.ParameterDirection.Output;

                    cmd.ExecuteNonQuery();
                    status = Convert.ToBoolean(cmd.Parameters["Statues"].Value);
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }
        public int GetTrackingNumberCount()
        {
            int count = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(@"SELECT count(IdBBtrackingCodes) AS Records FROM bestBuyE2.BBtrackingCodes;", conn);
                    cmd.CommandType = System.Data.CommandType.Text;
                    MySqlDataAdapter mySqlDataAdap = new MySqlDataAdapter(cmd);
                    DataTable data = new DataTable();
                    mySqlDataAdap.Fill(data);
                    if (data.Rows.Count > 0)
                    {
                        foreach (DataRow datarow in data.Rows)
                        {
                            ZincWatchListSummaryViewModal model = new ZincWatchListSummaryViewModal();
                            count = Convert.ToInt32(datarow["Records"] != DBNull.Value ? datarow["Records"] : "0");


                        }

                    }
                }
                return count;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public List<BBtrackingCodesViewModel> GetBBtrackingRulesList(int offset)
        {
            List<BBtrackingCodesViewModel> listModel = null;
            // List<ZincWatchListSummaryViewModal> listModel = new List<ZincWatchListSummaryViewModal>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmdd = new MySqlCommand(@"SELECT * FROM bestBuyE2.BBtrackingCodes ORDER BY IdBBtrackingCodes desc limit 25 offset " + offset, conn);
                    cmdd.CommandType = System.Data.CommandType.Text;
                    MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(cmdd);
                    DataTable dt = new DataTable();
                    mySqlDataAdapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        listModel = new List<BBtrackingCodesViewModel>();
                        foreach (DataRow reader in dt.Rows)
                        {
                            BBtrackingCodesViewModel model = new BBtrackingCodesViewModel();
                            model.IdBBtrackingCodes = Convert.ToInt32(reader["IdBBtrackingCodes"] != DBNull.Value ? reader["IdBBtrackingCodes"] : 0);
                            model.CarrierCode = Convert.ToString(reader["CarrierCode"] != DBNull.Value ? reader["CarrierCode"] : " ");
                            model.CarrierName = Convert.ToString(reader["CarrierName"] != DBNull.Value ? reader["CarrierName"] : " ");
                            model.CarrierUrl = Convert.ToString(reader["CarrierUrl"] != DBNull.Value ? reader["CarrierUrl"] : " ");
                            model.TrackingNumberCode = Convert.ToString(reader["TrackingNumberCode"] != DBNull.Value ? reader["TrackingNumberCode"] : " ");
                            listModel.Add(model);
                        }
                    }
                }
                return listModel;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
