using DataAccess.Helper;
using DataAccess.ViewModels;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataAccess
{
    public class ProductWarehouseQtyDataAccess
    {
        public string connStr { get; set; }
        EncDecChannel _EncDecChannel = null;
        ApprovedPriceDataAccess approvedPriceDataAccess = null;
        public ProductWarehouseQtyDataAccess(IConnectionString connectionString)
        {
            connStr = connectionString.GetConnectionString();
            _EncDecChannel = new EncDecChannel(connectionString);
            approvedPriceDataAccess = new ApprovedPriceDataAccess(connectionString);
        }
        //new Job.
        public bool SaveProductQty(List<ProductWarehouseQtyViewModel> viewModel)
        {
            bool status = false;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("call p_DeleteProductWHQtyBySKU('" + viewModel[0].ProductSku + "'); ");
            foreach (var item in viewModel)
            {
                stringBuilder.Append(" INSERT INTO `bestBuyE2`.`product_warehouse_qty`(`warehouse_id`,`qty_avaiable`,`sku`)VALUES(" + item.WarehouseID + "," + item.AvailableQty + ", '" + item.ProductSku + "'); ");
            }
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand(stringBuilder.ToString(), conn);
                    mySqlCommand.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public bool DeleteProductQty(string sku)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_DeleteProductWHQtyBySKU", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_SKU", sku);
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public List<ProductWarehouseQtyViewModel> GetProductQtyBySKU(string SKU)
        {
            List<ProductWarehouseQtyViewModel> _ViewModels = new List<ProductWarehouseQtyViewModel>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetProductQtyBySKU", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_SKU", SKU);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                ProductWarehouseQtyViewModel model = new ProductWarehouseQtyViewModel();
                                model.WarehouseName = Convert.ToString(reader["sc_wh_name"] != DBNull.Value ? reader["sc_wh_name"] : "");
                                model.AvailableQty = Convert.ToInt32(reader["qty_avaiable"] != DBNull.Value ? reader["qty_avaiable"] : "0");
                                model.LocationNotes = Convert.ToString(reader["LocationNotes"] != DBNull.Value ? reader["LocationNotes"] : "");
                                _ViewModels.Add(model);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return _ViewModels;
        }


        public List<ProductWarehouseQtyViewModel> GetProductQtyBySKU_ForOrdersPage(string SKU, MySqlConnection conn)
        {
            List<ProductWarehouseQtyViewModel> _ViewModels = new List<ProductWarehouseQtyViewModel>();
            try
            {
                MySqlCommand cmd = new MySqlCommand("p_GetProductQtyBySKU_LimitedWH", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("_SKU", SKU);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow reader in dt.Rows)
                    {
                        ProductWarehouseQtyViewModel model = new ProductWarehouseQtyViewModel();
                        model.WarehouseName = Convert.ToString(reader["sc_wh_name"] != DBNull.Value ? reader["sc_wh_name"] : "");
                        model.AvailableQty = Convert.ToInt32(reader["qty_avaiable"] != DBNull.Value ? reader["qty_avaiable"] : "0");
                        _ViewModels.Add(model);
                    }
                }
            }

            catch (Exception ex)
            {
            }
            return _ViewModels;
        }

        public bool SaveWarehouseQtyForDropshipNoneSKU(ProductWarehouseQtyForDS_None_SKU_ViewModel viewModel)
        {
            bool status = false;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("p_SaveBestBuyDropshipNoneWarehouseQty_OrigionalQty", conn);
                    mySqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_product_sku", viewModel.product_sku);
                    mySqlCommand.Parameters.AddWithValue("_warehouse_qty", viewModel.warehouse_qty);
                    mySqlCommand.Parameters.AddWithValue("_warehouse_id", viewModel.warehouse_id);
                    mySqlCommand.Parameters.AddWithValue("_insert_datetime", viewModel.insert_datetime);
                    mySqlCommand.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public bool SaveBestBuyQtyMovementForDropshipNone_SKU(BestBuyDropShipQtyMovementViewModel viewModel)
        {
            bool status = false;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("p_SaveBestBuyQtyMovementForDropshipNone_SKU", conn);
                    mySqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_product_sku", viewModel.ProductSku);
                    mySqlCommand.Parameters.AddWithValue("_ds_status", viewModel.DropShipStatus);
                    mySqlCommand.Parameters.AddWithValue("_ds_qty", viewModel.DropShipQuantity);
                    mySqlCommand.Parameters.AddWithValue("_order_date", viewModel.OrderDate);
                    mySqlCommand.Parameters.AddWithValue("_dropshipComments", viewModel.DropshipComments);
                    mySqlCommand.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public ProductWarehouseQtyForDS_None_SKU_ViewModel GetDropShipNoneWarehouseQtyBySKU(string SKU)
        {
            ProductWarehouseQtyForDS_None_SKU_ViewModel model = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetDropshipNoneQarehouseQtyBySKU", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_sku", SKU);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            model = new ProductWarehouseQtyForDS_None_SKU_ViewModel();
                            while (reader.Read())
                            {
                                model.warehouse_qty = Convert.ToInt32(reader["warehouse_qty"] != DBNull.Value ? reader["warehouse_qty"] : 0);
                                if (model.warehouse_qty < -1)
                                {
                                    model.warehouse_qty = 0;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return model;
        }
        public List<ProductWarehouseQtyViewModel> GetProductQtyBySKU_ForOrdersPagefOriNVENTORY(string SKU)
        {
            List<ProductWarehouseQtyViewModel> _ViewModels = new List<ProductWarehouseQtyViewModel>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    MySqlCommand cmd = new MySqlCommand("p_GetProductQtyBySKU_LimitedWH", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_SKU", SKU);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow reader in dt.Rows)
                        {
                            ProductWarehouseQtyViewModel model = new ProductWarehouseQtyViewModel();
                            model.WarehouseName = Convert.ToString(reader["sc_wh_name"] != DBNull.Value ? reader["sc_wh_name"] : "");
                            model.AvailableQty = Convert.ToInt32(reader["qty_avaiable"] != DBNull.Value ? reader["qty_avaiable"] : "0");
                            model.ProductSku = SKU;
                            _ViewModels.Add(model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return _ViewModels;
        }


        public void GetWarahouseQty(string SKU)
        {

            string ApiURL = "";
            ApiURL = "https://lp.api.sellercloud.com/rest/api";
            AuthenticateSCRestViewModel authenticate = new AuthenticateSCRestViewModel();
            GetChannelCredViewModel _getChannelCredViewModel = new GetChannelCredViewModel();
            _getChannelCredViewModel = _EncDecChannel.DecryptedData("sellercloud");
            authenticate = _EncDecChannel.AuthenticateSCForIMportOrder(_getChannelCredViewModel, ApiURL);

            List<WareHouseProductQty> warehouselist = new List<WareHouseProductQty>();
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest
                    .Create(ApiURL + "/Inventory/Warehouses?productID=" + SKU);
                request.Method = "GET";
                request.Accept = "application/json;";
                request.ContentType = "application/json";
                request.Headers["Authorization"] = "Bearer " + authenticate.access_token;

                string strResponse = "";
                using (WebResponse webResponse = request.GetResponse())
                {
                    using (StreamReader stream = new StreamReader(webResponse.GetResponseStream()))
                    {
                        strResponse = stream.ReadToEnd();
                    }
                }
                warehouselist = JsonConvert.DeserializeObject<List<WareHouseProductQty>>(strResponse);

                ProductwareHousesViewModel viewModel = new ProductwareHousesViewModel();
                viewModel.SKU = warehouselist[0].ProductID;

                viewModel.DropShip_Canada = warehouselist.Where(s => s.WarehouseID == 364).Select(s => s.AvailableQty).FirstOrDefault();
                viewModel.DropShip_USA = warehouselist.Where(s => s.WarehouseID == 365).Select(s => s.AvailableQty).FirstOrDefault();
                viewModel.FBA_Canada = warehouselist.Where(s => s.WarehouseID == 368).Select(s => s.AvailableQty).FirstOrDefault();
                viewModel.FBA_USA = warehouselist.Where(s => s.WarehouseID == 359).Select(s => s.AvailableQty).FirstOrDefault();
                viewModel.HLD_CA1 = warehouselist.Where(s => s.WarehouseID == 358).Select(s => s.AvailableQty).FirstOrDefault();
                viewModel.HLD_CA2 = warehouselist.Where(s => s.WarehouseID == 367).Select(s => s.AvailableQty).FirstOrDefault();
                viewModel.HLD_CN1 = warehouselist.Where(s => s.WarehouseID == 376).Select(s => s.AvailableQty).FirstOrDefault();
                viewModel.HLD_Interim = warehouselist.Where(s => s.WarehouseID == 378).Select(s => s.AvailableQty).FirstOrDefault();
                viewModel.HLD_Tech1 = warehouselist.Where(s => s.WarehouseID == 372).Select(s => s.AvailableQty).FirstOrDefault();
                viewModel.Interim_FBA_CA = warehouselist.Where(s => s.WarehouseID == 373).Select(s => s.AvailableQty).FirstOrDefault();
                viewModel.Interim_FBA_USA = warehouselist.Where(s => s.WarehouseID == 360).Select(s => s.AvailableQty).FirstOrDefault();
                viewModel.NY_14305 = warehouselist.Where(s => s.WarehouseID == 369).Select(s => s.AvailableQty).FirstOrDefault();
                viewModel.Shipito = warehouselist.Where(s => s.WarehouseID == 366).Select(s => s.AvailableQty).FirstOrDefault();
                SaveProductWareHouses(viewModel);

                var PhyQtyt = viewModel.FBA_Canada + viewModel.FBA_USA + viewModel.HLD_CA1 + viewModel.HLD_CA2 + viewModel.HLD_CN1 + viewModel.HLD_Interim + viewModel.HLD_Tech1 + viewModel.Interim_FBA_CA + viewModel.Interim_FBA_USA + viewModel.NY_14305 + viewModel.Shipito;
                UpdateProductCatalogByWHSKU(PhyQtyt, viewModel.SKU);
                
                //var status = SaveWarehouseProductQty_New(warehouselist);
                //var Item = warehouselist.Where(s => s.WarehouseID != 364 && s.WarehouseID != 365).ToList();

                //var PhyQtyt = Item.Sum(s => s.PhysicalQty);
                //UpdateProductCatalogByWHSKU(PhyQtyt, SKU);
            }
            catch (Exception exp)
            {
                System.Diagnostics.Debug.WriteLine(exp.Message + " :");
            }

        }
        public bool SaveWarehouseProductQty(List<WareHouseProductQty> viewModel)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    foreach (var item in viewModel)
                    {
                        var id = item.WarehouseID == 364 ? 1
                        : item.WarehouseID == 365 ? 2 :
                        item.WarehouseID == 368 ? 3 :
                        item.WarehouseID == 359 ? 4 :
                        item.WarehouseID == 358 ? 5 :
                        item.WarehouseID == 367 ? 6 :
                        item.WarehouseID == 376 ? 7 :
                        item.WarehouseID == 378 ? 8 :
                        item.WarehouseID == 372 ? 9 :
                        item.WarehouseID == 373 ? 10 :
                        item.WarehouseID == 360 ? 11 :
                        item.WarehouseID == 369 ? 12 :
                        item.WarehouseID == 366 ? 13 : 13;
                        MySqlCommand cmd = new MySqlCommand("p_InsertUpdateWarehouseProductQuanty", conn);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("_sku", item.ProductID);
                        cmd.Parameters.AddWithValue("_PhysicalQty", item.PhysicalQty);
                        cmd.Parameters.AddWithValue("_WarehouseID", id);
                        cmd.ExecuteNonQuery();
                    }
                    return true;
                }

            }
            catch (Exception ex)
            {
                return false;
            }

        }


        public bool SaveWarehouseProductQty_New(List<WareHouseProductQty> viewModel)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("");
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("sku", typeof(System.String)));
                dt.Columns.Add(new DataColumn("PhysicalQty", typeof(System.Int32)));
                dt.Columns.Add(new DataColumn("WarehouseID", typeof(System.Int32)));
                foreach (var item in viewModel)
                {
                    var id = item.WarehouseID == 364 ? 1
                    : item.WarehouseID == 365 ? 2 :
                    item.WarehouseID == 368 ? 3 :
                    item.WarehouseID == 359 ? 4 :
                    item.WarehouseID == 358 ? 5 :
                    item.WarehouseID == 367 ? 6 :
                    item.WarehouseID == 376 ? 7 :
                    item.WarehouseID == 378 ? 8 :
                    item.WarehouseID == 372 ? 9 :
                    item.WarehouseID == 373 ? 10 :
                    item.WarehouseID == 360 ? 11 :
                    item.WarehouseID == 369 ? 12 :
                    item.WarehouseID == 366 ? 13 : 13;

                    DataRow dr = dt.NewRow();
                    dr["sku"] = item.ProductID;
                    dr["PhysicalQty"] = item.PhysicalQty;
                    dr["WarehouseID"] = id;

                    dt.Rows.Add(dr);
                    //string name = wareHouses.Where(s => s.Id == id).FirstOrDefault().Name;
                }
                MySqlConnection con = new MySqlConnection(connStr);
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                con.Open();
                MySqlCommand cmd = new MySqlCommand("p_InsertUpdateWarehouseProductQuanty", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.UpdatedRowSource = UpdateRowSource.None;

                cmd.Parameters.Add("?_sku", MySqlDbType.String).SourceColumn = "sku";
                cmd.Parameters.Add("?_PhysicalQty", MySqlDbType.Int32).SourceColumn = "PhysicalQty";
                cmd.Parameters.Add("?_WarehouseID", MySqlDbType.Decimal).SourceColumn = "WarehouseID";

                MySqlDataAdapter da = new MySqlDataAdapter();
                da.InsertCommand = cmd;
                da.UpdateBatchSize = 100;
                int records = da.Update(dt);
                con.Close();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool UpdateProductCatalogByWHSKU(int PhysicalQty, string SKU)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_UpdateProductCatalogbyWHSku", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_PhysicalQty", PhysicalQty);
                    cmd.Parameters.AddWithValue("_SKU", SKU);
                    cmd.ExecuteNonQuery();
                    status = true;
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }


        public List<WareHouseViewModel> GetWareHousesNamesList()
        {
            List<WareHouseViewModel> listViewModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetWareHousesNamesList", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            listViewModel = new List<WareHouseViewModel>();
                            while (reader.Read())
                            {
                                WareHouseViewModel ViewModel = new WareHouseViewModel();
                                ViewModel.Id = Convert.ToInt32(reader["Id"]);
                                ViewModel.Name = Convert.ToString(reader["WareHouseName"]);
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

        public List<ProductSKUViewModel> GetAllSKUForAutoComplete(string name)
        {
            List<ProductSKUViewModel> listViewModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetProductSKUList", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_name", name.Trim());
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
        public bool SaveWarehouseProductQty_New2(List<ProductWarehouseQtyViewModel> viewModel)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("");
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("sku", typeof(System.String)));
                dt.Columns.Add(new DataColumn("PhysicalQty", typeof(System.Int32)));
                dt.Columns.Add(new DataColumn("WarehouseID", typeof(System.Int32)));
                foreach (var item in viewModel)
                {
                    DataRow dr = dt.NewRow();
                    dr["sku"] = item.ProductSku;
                    dr["PhysicalQty"] = item.AvailableQty;
                    dr["WarehouseID"] = item.WarehouseID;

                    dt.Rows.Add(dr);
                    //string name = wareHouses.Where(s => s.Id == id).FirstOrDefault().Name;
                }
                MySqlConnection con = new MySqlConnection(connStr);
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                con.Open();
                MySqlCommand cmd = new MySqlCommand("p_InsertUpdateWarehouseProductQuanty", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.UpdatedRowSource = UpdateRowSource.None;

                cmd.Parameters.Add("?_sku", MySqlDbType.String).SourceColumn = "sku";
                cmd.Parameters.Add("?_PhysicalQty", MySqlDbType.Int32).SourceColumn = "PhysicalQty";
                cmd.Parameters.Add("?_WarehouseID", MySqlDbType.Decimal).SourceColumn = "WarehouseID";

                MySqlDataAdapter da = new MySqlDataAdapter();
                da.InsertCommand = cmd;
                da.UpdateBatchSize = 100;
                int records = da.Update(dt);
                con.Close();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SaveWarehouseProductQty_New4(List<ProductWarehouseQtyViewModel> viewModel)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("");
                foreach (var item in viewModel)
                {
                    stringBuilder.Append("update product_warehouse_qty SET qty_avaiable='" + item.AvailableQty + "',warehouseName='" + item.WarehouseName + "' where sku='" + item.ProductSku + "' and warehouse_id='" + item.WarehouseID + "';");
                }
                MySqlConnection con = new MySqlConnection(connStr);
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand(stringBuilder.ToString(), conn);
                    mySqlCommand.ExecuteNonQuery();
                    conn.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SaveProductWareHouses(ProductwareHousesViewModel viewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_SaveProductWarehouseQtydumy", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_SKU", viewModel.SKU);
                    cmd.Parameters.AddWithValue("_DropShip_Canada", viewModel.DropShip_Canada);
                    cmd.Parameters.AddWithValue("_DropShip_USA", viewModel.DropShip_USA);
                    cmd.Parameters.AddWithValue("_FBA_Canada", viewModel.FBA_Canada);
                    cmd.Parameters.AddWithValue("_FBA_USA", viewModel.FBA_USA);
                    cmd.Parameters.AddWithValue("_HLD_CA1", viewModel.HLD_CA1);
                    cmd.Parameters.AddWithValue("_HLD_CA2", viewModel.HLD_CA2);
                    cmd.Parameters.AddWithValue("_HLD_CN1", viewModel.HLD_CN1);
                    cmd.Parameters.AddWithValue("_HLD_Interim", viewModel.HLD_Interim);
                    cmd.Parameters.AddWithValue("_HLD_Tech1", viewModel.HLD_Tech1);
                    cmd.Parameters.AddWithValue("_Interim_FBA_CA", viewModel.Interim_FBA_CA);
                    cmd.Parameters.AddWithValue("_Interim_FBA_USA", viewModel.Interim_FBA_USA);
                    cmd.Parameters.AddWithValue("_NY_14305", viewModel.NY_14305);
                    cmd.Parameters.AddWithValue("_Shipito", viewModel.Shipito);
                    cmd.ExecuteNonQuery();
                    status = true;
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public List<ProductWarehouseQtyViewModel> GetWareHousesQtyList(string SKU)
        {
            List<ProductWarehouseQtyViewModel> list = new List<ProductWarehouseQtyViewModel>(); ;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetWhareHousesQtyBySKU", conn);
                    cmd.Parameters.AddWithValue("_SKU", SKU);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            
                            while (reader.Read())
                            {
                                string SKUc = Convert.ToString(reader["SKU"] != DBNull.Value ? reader["SKU"] : "");
                                decimal OnOrder = Convert.ToDecimal(reader["OnOrder"] != DBNull.Value ? reader["OnOrder"] : 0);
                                string LocationNotes = Convert.ToString(reader["LocationNotes"] != DBNull.Value ? reader["LocationNotes"] : "");
                                int DropShip_Canadac = Convert.ToInt32(reader["DropShip_Canada"] != DBNull.Value ? reader["DropShip_Canada"] : 0);
                                List<ApprovedPriceViewModel> approvedPrices = new List<ApprovedPriceViewModel>();
                                approvedPrices = approvedPriceDataAccess.GetApprovedPricesList(1278, 30, 0, SKUc, "", "");
                                {
                                    ProductWarehouseQtyViewModel model = new ProductWarehouseQtyViewModel();
                                    model.WarehouseID = (int)WarehouseNames.DropShipCanada;
                                    model.AvailableQty = DropShip_Canadac;
                                    model.WarehouseName = "DropShip Canada";
                                    model.ProductSku = SKUc;
                                    model.LocationNotes = LocationNotes;
                                    model.OnOrder = OnOrder;
                                    model.approvedPrices = approvedPrices.Where(s => s.PriceStatus == true).ToList();
                                    list.Add(model);
                                }
                                int DropShip_USAc = Convert.ToInt32(reader["DropShip_USA"] != DBNull.Value ? reader["DropShip_USA"] : 0);
                                {
                                    ProductWarehouseQtyViewModel model = new ProductWarehouseQtyViewModel();
                                    model.WarehouseID = (int)WarehouseNames.DropShipUSA;
                                    model.WarehouseName = "DropShip USA";
                                    model.AvailableQty = DropShip_USAc;
                                    model.ProductSku = SKUc;
                                    model.LocationNotes = LocationNotes;
                                    model.OnOrder = OnOrder;
                                    model.approvedPrices = approvedPrices.Where(s => s.PriceStatus == true).ToList();
                                    list.Add(model);

                                    
                                }
                                int FBA_Canadac = Convert.ToInt32(reader["FBA_Canada"] != DBNull.Value ? reader["FBA_Canada"] : 0);
                                {
                                    ProductWarehouseQtyViewModel model = new ProductWarehouseQtyViewModel();
                                    model.WarehouseID = (int)WarehouseNames.FBACanada;
                                    model.AvailableQty = FBA_Canadac;
                                    model.WarehouseName = "FBA Canada";
                                    model.ProductSku = SKUc;
                                    model.LocationNotes = LocationNotes;
                                    model.OnOrder = OnOrder;
                                    model.approvedPrices = approvedPrices.Where(s => s.PriceStatus == true).ToList();
                                    list.Add(model);
                                   
                                }
                                int FBA_USAc = Convert.ToInt32(reader["FBA_USA"] != DBNull.Value ? reader["FBA_USA"] : 0);
                                {
                                    ProductWarehouseQtyViewModel model = new ProductWarehouseQtyViewModel();
                                    model.WarehouseID = (int)WarehouseNames.FBAUSA;
                                    model.AvailableQty = FBA_USAc;
                                    model.WarehouseName = "FBA USA";
                                    model.ProductSku = SKUc;
                                    model.LocationNotes = LocationNotes;
                                    model.OnOrder = OnOrder;
                                    model.approvedPrices = approvedPrices.Where(s => s.PriceStatus == true).ToList();
                                    list.Add(model);

                                 
                                }
                                int HLD_CA1c = Convert.ToInt32(reader["HLD_CA1"] != DBNull.Value ? reader["HLD_CA1"] : 0);
                                {
                                    ProductWarehouseQtyViewModel model = new ProductWarehouseQtyViewModel();
                                    model.WarehouseID = (int)WarehouseNames.HLDCA1;
                                    model.AvailableQty = HLD_CA1c;
                                    model.WarehouseName = "HLD-CA1";
                                    model.ProductSku = SKUc;
                                    model.LocationNotes = LocationNotes;
                                    model.OnOrder = OnOrder;
                                    model.approvedPrices = approvedPrices.Where(s => s.PriceStatus == true).ToList();
                                    list.Add(model);

                                    
                                }
                                int HLD_CA2c = Convert.ToInt32(reader["HLD_CA2"] != DBNull.Value ? reader["HLD_CA2"] : 0);
                                {
                                    ProductWarehouseQtyViewModel model = new ProductWarehouseQtyViewModel();
                                    model.WarehouseID = (int)WarehouseNames.HLDCA2;
                                    model.AvailableQty = HLD_CA2c;
                                    model.WarehouseName = "HLD-CA2";
                                    model.ProductSku = SKUc;
                                    model.LocationNotes = LocationNotes;
                                    model.OnOrder = OnOrder;
                                    model.approvedPrices = approvedPrices.Where(s => s.PriceStatus == true).ToList();
                                    list.Add(model);

                                    
                                }
                                int HLD_CN1c = Convert.ToInt32(reader["HLD_CN1"] != DBNull.Value ? reader["HLD_CN1"] : 0);
                                {
                                    ProductWarehouseQtyViewModel model = new ProductWarehouseQtyViewModel();
                                    model.WarehouseID = (int)WarehouseNames.HLDCN1;
                                    model.AvailableQty = HLD_CN1c;
                                    model.WarehouseName = "HLD-CN1";
                                    model.ProductSku = SKUc;
                                    model.LocationNotes = LocationNotes;
                                    model.OnOrder = OnOrder;
                                    model.approvedPrices = approvedPrices.Where(s => s.PriceStatus == true).ToList();
                                    list.Add(model);

                                   
                                }
                                int HLD_Interimc = Convert.ToInt32(reader["HLD_Interim"] != DBNull.Value ? reader["HLD_Interim"] : 0);
                                {
                                    ProductWarehouseQtyViewModel model = new ProductWarehouseQtyViewModel();
                                    model.WarehouseID = (int)WarehouseNames.HLDInterim;
                                    model.AvailableQty = HLD_Interimc;
                                    model.WarehouseName = "HLD-Interim";
                                    model.ProductSku = SKUc;
                                    model.LocationNotes = LocationNotes;
                                    model.OnOrder = OnOrder;
                                    model.approvedPrices = approvedPrices.Where(s => s.PriceStatus == true).ToList();
                                    list.Add(model);

                                    
                                }
                                int HLD_Tech1c = Convert.ToInt32(reader["HLD_Tech1"] != DBNull.Value ? reader["HLD_Tech1"] : 0);
                                {
                                    ProductWarehouseQtyViewModel model = new ProductWarehouseQtyViewModel();
                                    model.WarehouseID = (int)WarehouseNames.HLDTech1;
                                    model.AvailableQty = HLD_Tech1c;
                                    model.WarehouseName = "HLD-Tech1";
                                    model.ProductSku = SKUc;
                                    model.LocationNotes = LocationNotes;
                                    model.OnOrder = OnOrder;
                                    model.approvedPrices = approvedPrices.Where(s => s.PriceStatus == true).ToList();
                                    list.Add(model);

                                   
                                }
                                int Interim_FBA_CAc = Convert.ToInt32(reader["Interim_FBA_CA"] != DBNull.Value ? reader["Interim_FBA_CA"] : 0);
                                {
                                    ProductWarehouseQtyViewModel model = new ProductWarehouseQtyViewModel();
                                    model.WarehouseID = (int)WarehouseNames.InterimFBACA;
                                    model.AvailableQty = Interim_FBA_CAc;
                                    model.WarehouseName = "Interim FBA CA";
                                    model.ProductSku = SKUc;
                                    model.LocationNotes = LocationNotes;
                                    model.OnOrder = OnOrder;
                                    model.approvedPrices = approvedPrices.Where(s => s.PriceStatus == true).ToList();
                                    list.Add(model);
                                }
                                int Interim_FBA_USAc = Convert.ToInt32(reader["Interim_FBA_USA"] != DBNull.Value ? reader["Interim_FBA_USA"] : 0);
                                {
                                    ProductWarehouseQtyViewModel model = new ProductWarehouseQtyViewModel();
                                    model.WarehouseID = (int)WarehouseNames.InterimFBAUSA;
                                    model.AvailableQty = Interim_FBA_USAc;
                                    model.WarehouseName = "Interim FBA USA";
                                    model.LocationNotes = LocationNotes;
                                    model.ProductSku = SKUc;
                                    model.OnOrder = OnOrder;
                                    model.approvedPrices = approvedPrices.Where(s => s.PriceStatus == true).ToList();
                                    list.Add(model);
                                }
                                int NY_14305c = Convert.ToInt32(reader["NY_14305"] != DBNull.Value ? reader["NY_14305"] : 0);
                                {
                                    ProductWarehouseQtyViewModel model = new ProductWarehouseQtyViewModel();
                                    model.WarehouseID = (int)WarehouseNames.NY14305;
                                    model.WarehouseName = "NY-14305";
                                    model.AvailableQty = NY_14305c;
                                    model.ProductSku = SKUc;
                                    model.LocationNotes = LocationNotes;
                                    model.OnOrder = OnOrder;
                                    model.approvedPrices = approvedPrices.Where(s => s.PriceStatus == true).ToList();
                                    list.Add(model);
                                }
                                int Shipitoc = Convert.ToInt32(reader["Shipito"] != DBNull.Value ? reader["Shipito"] : 0);
                                {
                                    ProductWarehouseQtyViewModel model = new ProductWarehouseQtyViewModel();
                                    model.WarehouseID = (int)WarehouseNames.Shipito;
                                    model.AvailableQty = Shipitoc;
                                    model.WarehouseName = "Shipito";
                                    model.ProductSku = SKUc;
                                    model.LocationNotes = LocationNotes;
                                    model.OnOrder = OnOrder;
                                    model.approvedPrices = approvedPrices.Where(s => s.PriceStatus == true).ToList();
                                    list.Add(model);
                                }
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

    }

    public enum WarehouseNames
    {
        DropShipCanada = 1,
        DropShipUSA = 2,
        FBACanada = 3,
        FBAUSA = 4,
        HLDCA1 = 5,
        HLDCA2 = 6,
        HLDCN1 = 7,
        HLDInterim = 8,
        HLDTech1 = 9,
        InterimFBACA = 10,
        InterimFBAUSA = 11,
        NY14305 = 12,
        Shipito = 13
    }
}
