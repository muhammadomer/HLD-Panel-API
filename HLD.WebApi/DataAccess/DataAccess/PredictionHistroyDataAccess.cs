using DataAccess.Helper;
using DataAccess.ViewModels;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class PredictionHistroyDataAccess
    {
        public string connStr { get; set; }
        public PredictionHistroyDataAccess(IConnectionString connectionString)
        {
            connStr = connectionString.GetConnectionString();
        }
        public int PredictionSummaryCount(int VendorId, string SKU, string Title, bool Approved, bool Excluded, bool KitShadowStatus, bool Continue, string SearchFromSkuListPredict,  int Type = 0)
        {
            if (string.IsNullOrEmpty(SKU) || SKU == "undefined")
                SKU = "";
            if (string.IsNullOrEmpty(Title) || Title == "undefined")
                Title = "";

            if (string.IsNullOrEmpty(SKU) || SKU == "undefined")
                SKU = "";
            if (string.IsNullOrEmpty(Title) || Title == "undefined")
                Title = "";
            if (string.IsNullOrEmpty(SearchFromSkuListPredict) || SearchFromSkuListPredict == "undefined" || SearchFromSkuListPredict == "Nill")
                SearchFromSkuListPredict = "Nill";
            int count = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    //MySqlCommand cmd = new MySqlCommand("p_GetPredictionHistoryCountCopyExcluded", conn);//my change adeeeeel
                    MySqlCommand cmd = new MySqlCommand("p_GetPredictionHistoryCountCopyExcludedV1", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_SkuList", SearchFromSkuListPredict);
                    cmd.Parameters.AddWithValue("_VendorId", VendorId);
                    cmd.Parameters.AddWithValue("_SKU", SKU);
                    cmd.Parameters.AddWithValue("_Approved", Approved);
                    cmd.Parameters.AddWithValue("_Title", Title);
                    cmd.Parameters.AddWithValue("_Type", Type);
                    cmd.Parameters.AddWithValue("_Excluded", Excluded);
                    cmd.Parameters.AddWithValue("_Continue", Continue);
                    cmd.Parameters.AddWithValue("_KitShadowStatus", KitShadowStatus);
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
        public int SavePO(PurchaseOrderDataViewModel model)
        {
            int Id = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_SavePO", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_VendorId", model.VendorId);
                    cmd.Parameters.AddWithValue("_POId", model.POId);
                    cmd.Parameters.AddWithValue("_OrderedOn", model.OrderedOn);
                    cmd.Parameters.AddWithValue("_CurrencyCode", model.CurrencyCode);
                    cmd.Parameters.AddWithValue("_POStatus", model.POStatus);
                    cmd.Parameters.AddWithValue("_InternalPOId", model.InternalPOId);
                    cmd.Parameters.AddWithValue("_Notes", model.Notes);

                    MySqlDataAdapter mySqlDataAdap = new MySqlDataAdapter(cmd);
                    DataTable data = new DataTable();
                    mySqlDataAdap.Fill(data);
                    if (data.Rows.Count > 0)
                    {
                        foreach (DataRow datarow in data.Rows)
                        {
                            Id = Convert.ToInt32(datarow["Id"] != DBNull.Value ? datarow["Id"] : "0");
                        }

                    }
                }
                return Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int SavePOItem(PredictPOItemViewModel model)
        {
            int Id = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_SavePOItem", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_VendorId", model.VendorID);
                    cmd.Parameters.AddWithValue("_POId", model.POId);
                    cmd.Parameters.AddWithValue("_ProductID", model.ProductID);
                    cmd.Parameters.AddWithValue("_Currency", model.Currency);
                    cmd.Parameters.AddWithValue("_QtyOrdered", model.QtyOrdered);
                    cmd.Parameters.AddWithValue("_UnitPrice", model.UnitPrice);
                    cmd.Parameters.AddWithValue("_InternalPOId", model.InternalPOId);
                    cmd.Parameters.AddWithValue("_idPurchaseOrdersItems", model.idPurchaseOrdersItems);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                return Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<PredictionHistroyViewModel> GetAllPredictionCopy(int startLimit, int offset, string SearchFromSkuListPredict)
        {

            List<PredictionHistroyViewModel> listViewModel = new List<PredictionHistroyViewModel>();
            List<PredictionHistroyViewModel> list = new List<PredictionHistroyViewModel>();
            if (string.IsNullOrEmpty(SearchFromSkuListPredict) || SearchFromSkuListPredict == "undefined" || SearchFromSkuListPredict == "Nill")
                SearchFromSkuListPredict = "Nill";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("sp_PredictionHistoryCopyExcludedV1", conn);//change adeel

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("_offset", offset);
                    cmd.Parameters.AddWithValue("_Limit", startLimit);
                    cmd.Parameters.AddWithValue("_SkuList", SearchFromSkuListPredict);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            while (reader.Read())
                            {
                                PredictionHistroyViewModel ViewModel = new PredictionHistroyViewModel();
                                ViewModel.SKU = Convert.ToString(reader["sku"] != DBNull.Value ? (string)reader["sku"] : "");
                                ViewModel.ProductTitle = Convert.ToString(reader["title"]);
                                ViewModel.VendorAlias = Convert.ToString(reader["VendorAlias"]);
                                ViewModel.ApprovedUnitPrice = Convert.ToDecimal(reader["ApprovedUnitPrice"] != DBNull.Value ? reader["ApprovedUnitPrice"] : 0);
                                ViewModel.VendorId = Convert.ToInt32(reader["VendorId"] != DBNull.Value ? reader["VendorId"] : 0);
                                ViewModel.Currency = Convert.ToString(reader["Currency"]);
                                ViewModel.QtySold60 = Convert.ToInt32(reader["QtySold60"] != DBNull.Value ? reader["QtySold60"] : 0);
                                ViewModel.AggregatedQty = Convert.ToInt32(reader["AggregatedQty"] != DBNull.Value ? reader["AggregatedQty"] : 0);
                                ViewModel.PhysicalQty = Convert.ToInt32(reader["PhysicalQty"] != DBNull.Value ? reader["PhysicalQty"] : 0);
                                ViewModel.OnOrder = Convert.ToInt32(reader["OnOrder"] != DBNull.Value ? reader["OnOrder"] : 0);
                                ViewModel.QtyPerBox = Convert.ToInt32(reader["QtyPerBox"] != DBNull.Value ? reader["QtyPerBox"] : 0);
                                ViewModel.CompressedImage = reader["Compress_image"] != DBNull.Value ? (string)reader["Compress_image"] : "";
                                ViewModel.ImageName = reader["image_name"] != DBNull.Value ? (string)reader["image_name"] : "";
                                ViewModel.LowStock60 = Convert.ToDecimal(reader["LowStock60"] != DBNull.Value ? reader["LowStock60"] : 0);
                                ViewModel.LowStock90 = Convert.ToDecimal(reader["LowStock90"] != DBNull.Value ? reader["LowStock90"] : 0);
                                ViewModel.Continue = Convert.ToBoolean(reader["Continue"] != DBNull.Value ? reader["Continue"] : "false");
                                ViewModel.CoverDays = Convert.ToInt32(reader["CoverDays"] != DBNull.Value ? reader["CoverDays"] : 0);
                                ViewModel.CoverPhy = Convert.ToInt32(reader["CoverPhy"] != DBNull.Value ? reader["CoverPhy"] : 0);
                                ViewModel.ReservedQty = Convert.ToInt32(reader["ReservedQty"] != DBNull.Value ? reader["ReservedQty"] : 0);
                                ViewModel.LocationNotes = reader["LocationNotes"] != DBNull.Value ? (string)reader["LocationNotes"] : "";
                                ViewModel.InternalPOId = reader["InternalPOId"] != DBNull.Value ? Convert.ToInt32(reader["InternalPOId"]) : 0;
                                ViewModel.ShadowOf = reader["ShadowOf"] != DBNull.Value ? (string)reader["ShadowOf"] : "";
                                ViewModel.PredictIncluded = Convert.ToBoolean(reader["PredictIncluded"] != DBNull.Value ? reader["PredictIncluded"] : "false");
                                //ViewModel.KitShadowStatus = Convert.ToBoolean(reader["ProductDependency"] != DBNull.Value ? reader["ProductDependency"] : "false");
                                ViewModel.ProductDependency = reader["ProductDependency"] != DBNull.Value ? Convert.ToInt32(reader["ProductDependency"]) : 2;
                                //ViewModel.ProductDependency = Convert.ToBoolean(reader["ProductDependency"] != DBNull.Value ? reader["ProductDependency"] : "false");
                                //if (reader["ProductDependency"] == DBNull.Value)
                                //{
                                //    ViewModel.ProductType = "";
                                //}
                                //else
                                //{
                                //    if(ViewModel.ProductDependency==true)
                                //    {
                                //        ViewModel.ProductType = "Shadow";
                                //    }
                                //    else
                                //    {
                                //        ViewModel.ProductType = "Kit";

                                //    }
                                //}

                                listViewModel.Add(ViewModel);
                            }
                            list = listViewModel.GroupBy(s => s.SKU).Select(p => p.FirstOrDefault()).Distinct().ToList();
                            foreach (var item in list)
                            {
                                var Vendors = listViewModel.Where(s => s.SKU == item.SKU).Select(x => new Vendorlist
                                {
                                    VendorAlias = x.VendorAlias,
                                    VendorId = x.VendorId,
                                    Currency = x.Currency,
                                    ApprovedUnitPrice = x.ApprovedUnitPrice
                                }).ToList();
                                var obj = listViewModel.Where(s => s.SKU == item.SKU && item.InternalPOId != 0).FirstOrDefault();
                                item.InternalPOId = obj == null ? 0 : obj.InternalPOId;
                                item.list = Vendors;
                                //item.InternalPOs = GetInternalPOIdBySKU(item.SKU);
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
        public List<PredictionHistroyViewModel> GetAllPrediction(int startLimit, int offset, int VendorId, string SKU, string Title, bool Approved, bool Excluded, bool KitShadowStatus, bool Continue, string Sort, string SortedType,  int Type = 0)
        {

            List<PredictionHistroyViewModel> listViewModel = new List<PredictionHistroyViewModel>();
            List<PredictionHistroyViewModel> list = new List<PredictionHistroyViewModel>();
            if (string.IsNullOrEmpty(SKU) || SKU == "undefined")
                SKU = "";
            if (string.IsNullOrEmpty(Title) || Title == "undefined")
                Title = "";
            if (string.IsNullOrEmpty(Sort) || Sort == "undefined")
                Sort = "";
            if (string.IsNullOrEmpty(SortedType) || SortedType == "undefined")
                SortedType = "";
          
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("sp_PredictionHistoryCopyExcluded", conn);//change adeel
                   
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("_offset", offset);
                    cmd.Parameters.AddWithValue("_Limit", startLimit);
                    cmd.Parameters.AddWithValue("_VendorId", VendorId);
                    cmd.Parameters.AddWithValue("_SKU", SKU);
                    cmd.Parameters.AddWithValue("_Approved", Approved);
                    cmd.Parameters.AddWithValue("_Title", Title);
                    cmd.Parameters.AddWithValue("_Sort", Sort);
                    cmd.Parameters.AddWithValue("_SortedType", SortedType);
                    //cmd.Parameters.AddWithValue("_d_90", d_90);
                    cmd.Parameters.AddWithValue("_Type", Type);
                    cmd.Parameters.AddWithValue("_Excluded", Excluded);
                    cmd.Parameters.AddWithValue("_Continue", Continue);
                    cmd.Parameters.AddWithValue("_KitShadowStatus", KitShadowStatus);
                   
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            while (reader.Read())
                            {
                                PredictionHistroyViewModel ViewModel = new PredictionHistroyViewModel();
                                ViewModel.SKU = Convert.ToString(reader["sku"]);
                                ViewModel.ProductTitle = Convert.ToString(reader["title"]);
                                ViewModel.VendorAlias = Convert.ToString(reader["VendorAlias"]);
                                ViewModel.ApprovedUnitPrice = Convert.ToDecimal(reader["ApprovedUnitPrice"] != DBNull.Value ? reader["ApprovedUnitPrice"] : 0);
                                ViewModel.VendorId = Convert.ToInt32(reader["VendorId"] != DBNull.Value ? reader["VendorId"] : 0);
                                ViewModel.Currency = Convert.ToString(reader["Currency"]);
                                ViewModel.QtySold60 = Convert.ToInt32(reader["QtySold60"] != DBNull.Value ? reader["QtySold60"] : 0);
                                ViewModel.AggregatedQty = Convert.ToInt32(reader["AggregatedQty"] != DBNull.Value ? reader["AggregatedQty"] : 0);
                                ViewModel.PhysicalQty = Convert.ToInt32(reader["PhysicalQty"] != DBNull.Value ? reader["PhysicalQty"] : 0);
                                ViewModel.OnOrder = Convert.ToInt32(reader["OnOrder"] != DBNull.Value ? reader["OnOrder"] : 0);
                                ViewModel.QtyPerBox = Convert.ToInt32(reader["QtyPerBox"] != DBNull.Value ? reader["QtyPerBox"] : 0);
                                ViewModel.CompressedImage = reader["Compress_image"] != DBNull.Value ? (string)reader["Compress_image"] : "";
                                ViewModel.ImageName = reader["image_name"] != DBNull.Value ? (string)reader["image_name"] : "";
                                ViewModel.LowStock60 = Convert.ToDecimal(reader["LowStock60"] != DBNull.Value ? reader["LowStock60"] : 0);
                                ViewModel.LowStock90 = Convert.ToDecimal(reader["LowStock90"] != DBNull.Value ? reader["LowStock90"] : 0);
                                ViewModel.Continue = Convert.ToBoolean(reader["Continue"] != DBNull.Value ? reader["Continue"] : "false");
                                ViewModel.CoverDays = Convert.ToInt32(reader["CoverDays"] != DBNull.Value ? reader["CoverDays"] : 0);
                                ViewModel.CoverPhy = Convert.ToInt32(reader["CoverPhy"] != DBNull.Value ? reader["CoverPhy"] : 0);
                                ViewModel.ReservedQty = Convert.ToInt32(reader["ReservedQty"] != DBNull.Value ? reader["ReservedQty"] : 0);
                                ViewModel.LocationNotes = reader["LocationNotes"] != DBNull.Value ? (string)reader["LocationNotes"] : "";
                                ViewModel.InternalPOId = reader["InternalPOId"] != DBNull.Value ? Convert.ToInt32(reader["InternalPOId"]) : 0;
                                ViewModel.ShadowOf = reader["ShadowOf"] != DBNull.Value ? (string)reader["ShadowOf"] : "";
                                ViewModel.PredictIncluded = Convert.ToBoolean(reader["PredictIncluded"] != DBNull.Value ? reader["PredictIncluded"] : "false");
                                //ViewModel.KitShadowStatus = Convert.ToBoolean(reader["ProductDependency"] != DBNull.Value ? reader["ProductDependency"] : "false");
                                ViewModel.ProductDependency = reader["ProductDependency"] != DBNull.Value ? Convert.ToInt32(reader["ProductDependency"]) : 2;
                                //ViewModel.ProductDependency = Convert.ToBoolean(reader["ProductDependency"] != DBNull.Value ? reader["ProductDependency"] : "false");
                                //if (reader["ProductDependency"] == DBNull.Value)
                                //{
                                //    ViewModel.ProductType = "";
                                //}
                                //else
                                //{
                                //    if(ViewModel.ProductDependency==true)
                                //    {
                                //        ViewModel.ProductType = "Shadow";
                                //    }
                                //    else
                                //    {
                                //        ViewModel.ProductType = "Kit";

                                //    }
                                //}
                                    
                                listViewModel.Add(ViewModel);
                            }
                            list = listViewModel.GroupBy(s => s.SKU).Select(p => p.FirstOrDefault()).Distinct().ToList();
                            foreach (var item in list)
                            {
                                var Vendors = listViewModel.Where(s => s.SKU == item.SKU).Select(x => new Vendorlist
                                {
                                    VendorAlias = x.VendorAlias,
                                    VendorId = x.VendorId,
                                    Currency = x.Currency,
                                    ApprovedUnitPrice = x.ApprovedUnitPrice
                                }).ToList();
                                var obj = listViewModel.Where(s => s.SKU == item.SKU && item.InternalPOId != 0).FirstOrDefault();
                                item.InternalPOId = obj == null ? 0 : obj.InternalPOId;
                                item.list = Vendors;
                                //item.InternalPOs = GetInternalPOIdBySKU(item.SKU);
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
        public PredictionPOViewModel GetAllPredictionbyId(int Id)
        {

            List<PredictionPOViewModel> listViewModel = new List<PredictionPOViewModel>();
            List<PredictionPOViewModel> list = new List<PredictionPOViewModel>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetInternalPObyId", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Id", Id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                PredictionPOViewModel ViewModel = new PredictionPOViewModel();
                                ViewModel.idPurchaseOrdersItems = Convert.ToInt32(reader["idPurchaseOrdersItems"] != DBNull.Value ? reader["idPurchaseOrdersItems"] : 0);
                                ViewModel.SKU = Convert.ToString(reader["sku"]);
                                ViewModel.ApprovedUnitPrice = Convert.ToDecimal(reader["ApprovedUnitPrice"] != DBNull.Value ? reader["ApprovedUnitPrice"] : 0);
                                ViewModel.POQty = Convert.ToInt32(reader["POQty"] != DBNull.Value ? reader["POQty"] : 0);
                                ViewModel.LowStock90 = Convert.ToInt32(reader["LowStock90"] != DBNull.Value ? reader["LowStock90"] : 0);
                                ViewModel.POstatus = Convert.ToInt32(reader["POstatus"] != DBNull.Value ? reader["POstatus"] : 0);
                                ViewModel.VendorId = Convert.ToInt32(reader["VendorId"] != DBNull.Value ? reader["VendorId"] : 0);
                                ViewModel.Vendor = Convert.ToString(reader["Vendor"]);
                                ViewModel.Currency = Convert.ToString(reader["Currency"]);
                                ViewModel.Notes = Convert.ToString(reader["Notes"]);
                                ViewModel.OrderedOn = reader["OrderedOn"] != DBNull.Value ? Convert.ToDateTime(reader["OrderedOn"]) : DateTime.MinValue;
                                ViewModel.SmallImage = reader["SmallImage"] != DBNull.Value ? (string)reader["SmallImage"] : "";
                                ViewModel.LargImage = reader["LargImage"] != DBNull.Value ? (string)reader["LargImage"] : "";
                                ViewModel.CasePackQty = reader["CasePackQty"] != DBNull.Value ? Convert.ToInt32(reader["CasePackQty"]) : 0;
                                ViewModel.PhysicalQty = Convert.ToInt32(reader["PhysicalQty"] != DBNull.Value ? reader["PhysicalQty"] : 0);
                                ViewModel.QtySold60 = Convert.ToInt32(reader["QtySold60"] != DBNull.Value ? reader["QtySold60"] : 0);
                                ViewModel.QtySold90 = Convert.ToInt32(reader["QtySold90"] != DBNull.Value ? reader["QtySold90"] : 0);
                                ViewModel.CoverDays = Convert.ToInt32(reader["CoverDays"] != DBNull.Value ? reader["CoverDays"] : 0);
                                ViewModel.CoverPhy = Convert.ToInt32(reader["CoverPhy"] != DBNull.Value ? reader["CoverPhy"] : 0);
                                ViewModel.OnOrder = Convert.ToInt32(reader["OnOrder"] != DBNull.Value ? reader["OnOrder"] : 0);
                                ViewModel.ReservedQty = Convert.ToInt32(reader["ReservedQty"] != DBNull.Value ? reader["ReservedQty"] : 0);
                                ViewModel.Velocity = Convert.ToDecimal(reader["Velocity"] != DBNull.Value ? reader["Velocity"] : 0);
                                ViewModel.CoverDays = Convert.ToInt32(reader["CoverDays"] != DBNull.Value ? reader["CoverDays"] : 0);
                                //ViewModel.CoverDays = ViewModel.Velocity != 0 ? Convert.ToInt32(Convert.ToDecimal(ViewModel.POQty + ViewModel.PhysicalQty + ViewModel.OnOrder + ViewModel.ReservedQty) / ViewModel.Velocity) : 0;
                                ViewModel.InternalPOID = Id;
                                listViewModel.Add(ViewModel);
                            }
                            list = listViewModel.GroupBy(s => s.InternalPOID).Select(p => p.FirstOrDefault()).Distinct().ToList();

                            foreach (var item in list)
                            {
                                var Vendors = listViewModel.Where(s => s.InternalPOID == Id).Select(x => new PredictionSKUs
                                {
                                    idPurchaseOrdersItems = x.idPurchaseOrdersItems,
                                    SKU = x.SKU,
                                    POQty = x.POQty,
                                    LargImage = x.LargImage,
                                    SmallImage = x.SmallImage,
                                    ApprovedUnitPrice = x.ApprovedUnitPrice,
                                    CasePackQty = x.CasePackQty,
                                    PhysicalQty = x.PhysicalQty,
                                    QtySold60 = x.QtySold60,
                                    QtySold90 = x.QtySold90,
                                    OnOrder = x.OnOrder,
                                    Velocity = x.Velocity,
                                    CoverDays = x.CoverDays,
                                    CoverPhy=x.CoverPhy,
                                    ReservedQty=x.ReservedQty,
                                    LowStock90=x.LowStock90,
                                }).ToList();

                                item.list = Vendors;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return list.FirstOrDefault();
        }
        public List<PredictionInternalPOList> GetInternalPOIdBySKU(string SKU)
        {
            List<PredictionInternalPOList> list = new List<PredictionInternalPOList>();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetInternalPOIDBySku", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_SKU", SKU);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            while (reader.Read())
                            {
                                PredictionInternalPOList obj = new PredictionInternalPOList();
                                obj.InternalPOID = Convert.ToInt32(reader["InternalPOId"] != DBNull.Value ? reader["InternalPOId"] : 0);
                                obj.Vendor = Convert.ToString(reader["Vendor"]);
                                list.Add(obj);
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


        public bool DeletePO(int Id)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_DeletePOByInternalPOId", conn);
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

        public bool DeletePOItem(int Id)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_DeletePOIById", conn);
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

        public SoldQtyDaysViewModel GetSoldQtyDays(string SKU)
        {

            SoldQtyDaysViewModel item = new SoldQtyDaysViewModel();
            if (string.IsNullOrEmpty(SKU) || SKU == "undefined")
                SKU = "";
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetProductSOldQty", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_SKU", SKU);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            while (reader.Read())
                            {
                                SoldQtyDaysViewModel ViewModel = new SoldQtyDaysViewModel();
                                ViewModel.QtySold15 = Convert.ToInt32(reader["QtySold15"] != DBNull.Value ? reader["QtySold15"] : 0);
                                ViewModel.QtySold30 = Convert.ToInt32(reader["QtySold30"] != DBNull.Value ? reader["QtySold30"] : 0);
                                ViewModel.QtySold60 = Convert.ToInt32(reader["QtySold60"] != DBNull.Value ? reader["QtySold60"] : 0);
                                ViewModel.QtySold90 = Convert.ToInt32(reader["QtySold90"] != DBNull.Value ? reader["QtySold90"] : 0);
                                ViewModel.QtySoldYTD = Convert.ToInt32(reader["QtySoldYTD"] != DBNull.Value ? reader["QtySoldYTD"] : 0);
                                item = ViewModel;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return item;
        }

        public List<SKUDetailModel> GetSKUDetailBySku(string SKU)
        {

            List<SKUDetailModel> list = new List<SKUDetailModel>();
            if (string.IsNullOrEmpty(SKU) || SKU == "undefined")
                SKU = "";
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetPODeltailBySKU", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_SKU", SKU);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            while (reader.Read())
                            {
                                SKUDetailModel ViewModel = new SKUDetailModel();
                                ViewModel.ShipmentId = Convert.ToString(reader["ShipmentId"] != DBNull.Value ? reader["ShipmentId"] : "");
                                ViewModel.Type = Convert.ToString(reader["Type"] != DBNull.Value ? reader["Type"] : "");
                                ViewModel.ProductTitle = Convert.ToString(reader["ProductTitle"] != DBNull.Value ? reader["ProductTitle"] : "");
                                ViewModel.Vendor = Convert.ToString(reader["Vendor"] != DBNull.Value ? reader["Vendor"] : "");
                                ViewModel.VendorId = Convert.ToInt32(reader["VendorId"] != DBNull.Value ? reader["VendorId"] : 0);
                                ViewModel.POId = Convert.ToInt32(reader["POId"] != DBNull.Value ? reader["POId"] : 0);
                                ViewModel.QtyOrdered = Convert.ToInt32(reader["QtyOrdered"] != DBNull.Value ? reader["QtyOrdered"] : 0);
                                ViewModel.ShippedQty = Convert.ToInt32(reader["ShippedQty"] != DBNull.Value ? reader["ShippedQty"] : 0);
                                ViewModel.OrderedOn = Convert.ToDateTime(reader["OrderedOn"] != DBNull.Value ? reader["OrderedOn"] : DateTime.MinValue);
                                ViewModel.ShippedOn = Convert.ToDateTime(reader["ShippedOn"] != DBNull.Value ? reader["ShippedOn"] : DateTime.MinValue);
                                list.Add(ViewModel);
                            }
                            var dumylist = list.GroupBy(s => s.POId).Select(p => p.FirstOrDefault()).Distinct().ToList();
                            foreach (var item in dumylist)
                            {
                                var shipmentslist = list.Where(s => s.POId == item.POId).Select(x => new ShipmentList
                                {
                                    ShipmentId = x.ShipmentId,
                                    ShippedOn = x.ShippedOn,
                                    ShippedQty = x.ShippedQty,
                                    Type = x.Type,
                                }).ToList();

                                item.list = shipmentslist;
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

        public List<DraftPOViewModel> DraftPOList()
        {

            List<DraftPOViewModel> list = new List<DraftPOViewModel>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetDraftPOList", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            while (reader.Read())
                            {
                                DraftPOViewModel ViewModel = new DraftPOViewModel();
                                ViewModel.Notes = Convert.ToString(reader["Notes"] != DBNull.Value ? reader["Notes"] : "");
                                ViewModel.Vendor = Convert.ToString(reader["Vendor"] != DBNull.Value ? reader["Vendor"] : "");
                                ViewModel.VendorId = Convert.ToInt32(reader["VendorId"] != DBNull.Value ? reader["VendorId"] : 0);
                                ViewModel.SKUs = Convert.ToInt32(reader["SKUs"] != DBNull.Value ? reader["SKUs"] : 0);
                                ViewModel.InternalPOId = Convert.ToInt32(reader["InternalPOId"] != DBNull.Value ? reader["InternalPOId"] : 0);
                                ViewModel.OrderedOn = Convert.ToDateTime(reader["OrderedOn"] != DBNull.Value ? reader["OrderedOn"] : DateTime.MinValue);
                                list.Add(ViewModel);

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

        public List<WareHouseProductQuantitylistViewModel> GetWareHouseProductQuantitylistBySku(string SKU)
        {

            List<WareHouseProductQuantitylistViewModel> list = new List<WareHouseProductQuantitylistViewModel>();
            if (string.IsNullOrEmpty(SKU) || SKU == "undefined")
                SKU = "";
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetWareHouseProductQuantityByName", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_SKU", SKU);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            while (reader.Read())
                            {
                                WareHouseProductQuantitylistViewModel ViewModel = new WareHouseProductQuantitylistViewModel();
                                ViewModel.sku = Convert.ToString(reader["sku"] != DBNull.Value ? reader["sku"] : "");
                                ViewModel.warehouseName = Convert.ToString(reader["warehouseName"] != DBNull.Value ? reader["warehouseName"] : "");
                                ViewModel.physical_qty = Convert.ToInt32(reader["physical_qty"] != DBNull.Value ? reader["physical_qty"] : "");
                                ViewModel.warehouseId = Convert.ToInt32(reader["warehouseId"] != DBNull.Value ? reader["warehouseId"] : 0);
                                list.Add(ViewModel);
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

        public bool PredictIncludedExcluded(List<PredictIncludedExcludedViewModel> ListViewModel)
        {
            bool status = false;
            try
            {
                foreach (var item in ListViewModel)
                {
                    using (MySqlConnection conn = new MySqlConnection(connStr))
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand("P_UpdatePredictIncludedExcluded", conn);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("_SKU", item.SKU);
                        cmd.Parameters.AddWithValue("_includedexcluded", item.PredictIncludedExcluded);

                        cmd.ExecuteNonQuery();
                        status = true;
                        conn.Close();

                    }
                }

            }
            catch (Exception ex)
            {

            }
            return status;
        }
        public PredictionPOViewModel GetDataForPOCreation(List<PredictionInternalSKUList> SKUlist)
        {

            List<PredictionPOViewModel> listViewModel = new List<PredictionPOViewModel>();
            PredictionPOViewModel Obj = new PredictionPOViewModel();

            try
            {
                foreach (var item in SKUlist)
                {
                    using (MySqlConnection conn = new MySqlConnection(connStr))
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand("p_GetDataForPOCreation", conn);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("_SKU", item.SKU);
                        cmd.Parameters.AddWithValue("_VendorId", item.VendorId);
                        using (var reader = cmd.ExecuteReader())
                            if (reader.HasRows)
                                while (reader.Read())
                                {
                                    var model = new PredictionPOViewModel()
                                    {
                                        SKU = Convert.ToString(reader["SKU"]),
                                        VendorId = Convert.ToInt32(reader["VendorId"] != DBNull.Value ? reader["VendorId"] : 0),
                                        Vendor = Convert.ToString(reader["VendorAlias"] != DBNull.Value ? reader["VendorAlias"] : 0),
                                        ApprovedUnitPrice = Convert.ToDecimal(reader["ApprovedUnitPrice"] != DBNull.Value ? reader["ApprovedUnitPrice"] : 0),
                                        Currency = Convert.ToString(reader["Currency"]),
                                        POQty = Convert.ToInt32(reader["POQty"] != DBNull.Value ? reader["POQty"] : 0),
                                        CasePackQty = Convert.ToInt32(reader["CasePackQty"] != DBNull.Value ? reader["CasePackQty"] : 0),
                                        PhysicalQty = Convert.ToInt32(reader["PhysicalQty"] != DBNull.Value ? reader["PhysicalQty"] : 0),
                                        QtySold60 = Convert.ToInt32(reader["QtySold60"] != DBNull.Value ? reader["QtySold60"] : 0),
                                        QtySold90 = Convert.ToInt32(reader["QtySold90"] != DBNull.Value ? reader["QtySold90"] : 0),
                                        CoverDays = Convert.ToInt32(reader["CoverDays"] != DBNull.Value ? reader["CoverDays"] : 0),


                                        Velocity = Convert.ToDecimal(reader["Velocity"] != DBNull.Value ? reader["Velocity"] : 0),
                                        SmallImage = reader["Compress_image"] != DBNull.Value ? Convert.ToString(reader["Compress_image"]) : "",
                                        LargImage = reader["image_name"] != DBNull.Value ? Convert.ToString(reader["image_name"]) : "",
                                        OnOrder = Convert.ToInt32(reader["OnOrder"] != DBNull.Value ? reader["OnOrder"] : 0),
                                        CoverPhy = Convert.ToInt32(reader["CoverPhy"] != DBNull.Value ? reader["CoverPhy"] : 0),
                                        ReservedQty = Convert.ToInt32(reader["ReservedQty"] != DBNull.Value ? reader["ReservedQty"] : 0),
                                        LowStock90 = Convert.ToDecimal(reader["LowStock90"] != DBNull.Value ? reader["LowStock90"] : 0),
                                    };
                                    model.CoverDays = model.CoverDays = Convert.ToInt32(reader["CoverDays"] != DBNull.Value ? reader["CoverDays"] : 0);
                                    listViewModel.Add(model);
                                }
                                    

                    }
                }

                if (listViewModel.Count > 0)
                {
                    var list = listViewModel.Select(s => new PredictionSKUs()
                    {
                        SKU = s.SKU,
                        ApprovedUnitPrice = s.ApprovedUnitPrice,
                        Currency = s.Currency,
                        POQty = s.POQty,
                        CasePackQty = s.CasePackQty,
                        PhysicalQty = s.PhysicalQty,
                        QtySold60 = s.QtySold60,
                        QtySold90 = s.QtySold90,
                        LowStock90=s.LowStock90,
                        ReservedQty=s.ReservedQty,
                        CoverPhy=s.CoverPhy,

                        Velocity = s.Velocity,
                        SmallImage = s.SmallImage,
                        LargImage = s.LargImage,
                        OnOrder = s.OnOrder,
                        CoverDays = s.CoverDays,
                    }).ToList();
                    Obj = listViewModel.FirstOrDefault();
                    Obj.list = list;
                }
                
            }
            catch (Exception ex)
            {
            }
            return Obj;
        }

    }


}
