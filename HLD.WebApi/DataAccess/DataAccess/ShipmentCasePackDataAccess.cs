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
    public class ShipmentCasePackDataAccess
    {
        public string ConStr { get; set; }

        public ShipmentCasePackDataAccess(IConnectionString connectionString)
        {

            ConStr = connectionString.GetConnectionString();
        }


        public int Save(ShipmentCasePackProductViewModel ViewModel)
        {
            ViewModel.SKU = ViewModel.SKU.Replace("\t", "");
            int Id = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmdd = new MySqlCommand("p_SaveShipmentCasePackProducts", conn);
                    cmdd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmdd.Parameters.AddWithValue("_ShipmentId", ViewModel.ShipmentId);
                    cmdd.Parameters.AddWithValue("_VendorId", ViewModel.VendorId);
                    cmdd.Parameters.AddWithValue("_POId", ViewModel.POId);
                    cmdd.Parameters.AddWithValue("_SKU",ViewModel.SKU.Trim());
                    cmdd.Parameters.AddWithValue("_ShipedQty", ViewModel.ShipedQty);
                    cmdd.Parameters.AddWithValue("_QtyPerBox", ViewModel.QtyPerBox);
                    cmdd.Parameters.AddWithValue("_Heigth", ViewModel.Height);
                    cmdd.Parameters.AddWithValue("_Width", ViewModel.Width);
                    cmdd.Parameters.AddWithValue("_Length", ViewModel.Length);
                    cmdd.Parameters.AddWithValue("_Weight", ViewModel.Weight);
                    cmdd.Parameters.AddWithValue("_NoOfBoxes", ViewModel.NoOfBoxes);
                    cmdd.Parameters.AddWithValue("_CasePackId", ViewModel.CasePackId);


                    Id = Convert.ToInt32(cmdd.ExecuteScalar().ToString());
                }
            }
            catch (Exception ex)
            {

            }
            return Id;
        }

        public int Update(ShipmentCasePackProductViewModel ViewModel)
        {
            int Id = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmdd = new MySqlCommand("p_UpdateShipmentCasePackProduct", conn);
                    cmdd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmdd.Parameters.AddWithValue("_Id", ViewModel.idShipmentProducts);
                    cmdd.Parameters.AddWithValue("_ShipedQty", ViewModel.ShipedQty);
                    cmdd.Parameters.AddWithValue("_NoOfBoxes", ViewModel.NoOfBoxes);
                    cmdd.ExecuteNonQuery();
                    Id = ViewModel.idShipmentProducts;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Id;
        }

        public CasePackViewModel GetTemplateCasePack(int VendorId, string SKU)
        {
            CasePackViewModel Item = new CasePackViewModel();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetCasePackTemplateForShipment", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_VendorId", VendorId);
                    cmd.Parameters.AddWithValue("_SKU", SKU);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                CasePackViewModel viewModel = new CasePackViewModel
                                {
                                    CasePackId = reader["CasePackId"] != DBNull.Value ? Convert.ToInt32(reader["CasePackId"]) : 0,
                                    VendorId = Convert.ToInt32(reader["VendorId"]),
                                    QtyPerBox = reader["QtyPerBox"] != DBNull.Value ? Convert.ToInt32(reader["QtyPerBox"]) : 0,
                                    SKU = reader["SKU"] != DBNull.Value ? Convert.ToString(reader["SKU"]) : "",
                                    Width = reader["Width"] != DBNull.Value ? Convert.ToDecimal(reader["Width"]) : 0,
                                    Height = reader["Heigth"] != DBNull.Value ? Convert.ToDecimal(reader["Heigth"]) : 0,
                                    Length = reader["Length"] != DBNull.Value ? Convert.ToDecimal(reader["Length"]) : 0,
                                    Weight = reader["Weight"] != DBNull.Value ? Convert.ToDecimal(reader["Weight"]) : 0,
                                };
                                Item = viewModel;
                            }
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {

            }
            return Item;
        }

        public List<ShipmentCasePackProductViewModel> GetShipmentCasePackProducts(string Id)
        {
            List<ShipmentCasePackProductViewModel> list = new List<ShipmentCasePackProductViewModel>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetShipmentCasePackProduct", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Id", Id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                ShipmentCasePackProductViewModel viewModel = new ShipmentCasePackProductViewModel
                                {
                                    CasePackId = reader["CasePackId"] != DBNull.Value ? Convert.ToInt32(reader["CasePackId"]) : 0,
                                    idShipmentProducts = reader["idShipmentProducts"] != DBNull.Value ? Convert.ToInt32(reader["idShipmentProducts"]) : 0,
                                    ShipmentId = reader["ShipmentId"] != DBNull.Value ? Convert.ToString(reader["ShipmentId"]) : "",
                                    SKU = reader["SKU"] != DBNull.Value ? Convert.ToString(reader["SKU"]) : "",
                                    POId = reader["POId"] != DBNull.Value ? Convert.ToInt32(reader["POId"]) : 0,
                                    ShipedQty = reader["ShipedQty"] != DBNull.Value ? Convert.ToInt32(reader["ShipedQty"]) : 0,
                                    RecivedQty = reader["RecivedQty"] != DBNull.Value ? Convert.ToInt32(reader["RecivedQty"]) : 0,
                                    NoOfBoxes = reader["NoOfBoxes"] != DBNull.Value ? Convert.ToInt32(reader["NoOfBoxes"]) : 0,
                                    //VendorId = Convert.ToInt32(reader["VendorId"]),
                                    QtyPerBox = reader["QtyPerBox"] != DBNull.Value ? Convert.ToInt32(reader["QtyPerBox"]) : 0,
                                    Width = reader["Width"] != DBNull.Value ? Convert.ToDecimal(reader["Width"]) : 0,
                                    Height = reader["Heigth"] != DBNull.Value ? Convert.ToDecimal(reader["Heigth"]) : 0,
                                    Length = reader["Length"] != DBNull.Value ? Convert.ToDecimal(reader["Length"]) : 0,
                                    Weight = reader["Weight"] != DBNull.Value ? Convert.ToDecimal(reader["Weight"]) : 0,
                                    ImageName = reader["image_name"] != DBNull.Value ? Convert.ToString(reader["image_name"]) : "",
                                    CompressedImage = reader["Compress_image"] != DBNull.Value ? Convert.ToString(reader["Compress_image"]) : "",
                                    Title = reader["title"] != DBNull.Value ? Convert.ToString(reader["title"]) : "",
                                    OpenQty = reader["OpenQty"] != DBNull.Value ? Convert.ToInt32(reader["OpenQty"]) : 0,
                                    BalanceQty = reader["BalanceQty"] != DBNull.Value ? Convert.ToInt32(reader["BalanceQty"]) : 0,

                                    //LocationNotes = reader["LocationNotes"] != DBNull.Value ? Convert.ToString(reader["LocationNotes"]) : "",
                                    //PhysicalInventory = reader["PhysicalInventory"] != DBNull.Value ? Convert.ToString(reader["PhysicalInventory"]) : "",
                                    //ShadowOf = reader["ShadowOf"] != DBNull.Value ? Convert.ToString(reader["ShadowOf"]) : "",
                                    //QtyPerCase = reader["QtyPerCase"] != DBNull.Value ? Convert.ToInt32(reader["QtyPerCase"]) : 0,

                                };
                                if (viewModel.idShipmentProducts > 0)
                                {
                                    list.Add(viewModel);
                                }
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

        public ShipmentCasePackProductHeader GetShipmentCasePackProductHeader(string Id)
        {
            ShipmentCasePackProductHeader Item = new ShipmentCasePackProductHeader();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetShipmentCasePackProductHeader", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Id", Id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                ShipmentCasePackProductHeader viewModel = new ShipmentCasePackProductHeader
                                {
                                    ShipmentId = (string)reader["ShipmentId"],
                                    ShipmentName = (string)reader["ShipmentName"],
                                    ShipedQty = reader["ShipedQty"] != DBNull.Value ? Convert.ToInt32(reader["ShipedQty"]) : 0,
                                    SKUs = reader["SKUs"] != DBNull.Value ? Convert.ToInt32(reader["SKUs"]) : 0,
                                    Status = reader["Status"] != DBNull.Value ? Convert.ToInt32(reader["Status"]) : 0,
                                    POs = reader["POs"] != DBNull.Value ? Convert.ToInt32(reader["POs"]) : 0,
                                    NoOfBoxes = reader["NoOfBoxes"] != DBNull.Value ? Convert.ToInt32(reader["NoOfBoxes"]) : 0,
                                };
                                Item = (viewModel);
                            }
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {

            }
            return Item;
        }

        public ShipmentViewHeaderViewModel GetShipmentViewCasePackHeader(string ShipmentId)
        {
            var Item = new ShipmentViewHeaderViewModel();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetShipmentViewCasePackHeader", conn);
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
                            TotalShipedQty = reader["TotalShipedQty"] != DBNull.Value ? Convert.ToInt32(reader["TotalShipedQty"]) : 0,
                            ShippedDate = reader["CreatedAt"] != DBNull.Value ? Convert.ToDateTime(reader["CreatedAt"]) : DateTime.MinValue,
                            ReceivedDate = reader["ReceivedDate"] != DBNull.Value ? Convert.ToDateTime(reader["ReceivedDate"]) : DateTime.MinValue,
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

        public List<ShipmentViewProducListViewModel> GetShipmentViewProductCasPackList(string ShipmentId)
        {
            List<ShipmentViewProducListViewModel> list = new List<ShipmentViewProducListViewModel>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetShipmentViewCasepackProductList", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Id", ShipmentId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                ShipmentViewProducListViewModel viewModel = new ShipmentViewProducListViewModel
                                {
                                    idShipmentProducts = reader["idShipmentProducts"] != DBNull.Value ? Convert.ToInt32(reader["idShipmentProducts"]) : 0,
                                    SCID = reader["SCID"] != DBNull.Value ? Convert.ToInt32(reader["SCID"]) : 0,
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
                                    NoOfBoxes = reader["NoOfBoxes"] != DBNull.Value ? Convert.ToInt32(reader["NoOfBoxes"]) : 0,

                                    LocationNotes = reader["LocationNotes"] != DBNull.Value ? Convert.ToString(reader["LocationNotes"]) : "",
                                    PhysicalInventory = reader["PhysicalInventory"] != DBNull.Value ? Convert.ToString(reader["PhysicalInventory"]) : "",
                                    ShadowOf = reader["ShadowOf"] != DBNull.Value ? Convert.ToString(reader["ShadowOf"]) : "",
                                    QtyPerCase = reader["QtyPerCase"] != DBNull.Value ? Convert.ToInt32(reader["QtyPerCase"]) : 0,

                                    QtyPerBox = reader["QtyPerBox"] != DBNull.Value ? Convert.ToInt32(reader["QtyPerBox"]) : 0,
                                    Width = reader["Width"] != DBNull.Value ? Convert.ToDecimal(reader["Width"]) : 0,
                                    Height = reader["Heigth"] != DBNull.Value ? Convert.ToDecimal(reader["Heigth"]) : 0,
                                    Length = reader["Length"] != DBNull.Value ? Convert.ToDecimal(reader["Length"]) : 0,
                                    Weight = reader["Weight"] != DBNull.Value ? Convert.ToDecimal(reader["Weight"]) : 0,

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

        public int Delete(int Id)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmdd = new MySqlCommand("p_DeleteShipmentCasePackProduct", conn);
                    cmdd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmdd.Parameters.AddWithValue("_Id", Id);
                    cmdd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {

            }
            return Id;
        }



        public int SaveShipmentSKUCasePackTemplate(CasePackViewModel ViewModel)
        {
            int Id = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmdd = new MySqlCommand("p_SaveShipmentSKUCasePackTemplate", conn);
                    cmdd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmdd.Parameters.AddWithValue("_VendorId", ViewModel.VendorId);
                    cmdd.Parameters.AddWithValue("_SKU", ViewModel.SKU);
                    cmdd.Parameters.AddWithValue("_QtyPerBox", ViewModel.QtyPerBox);
                    cmdd.Parameters.AddWithValue("_Heigth", ViewModel.Height);
                    cmdd.Parameters.AddWithValue("_Width", ViewModel.Width);
                    cmdd.Parameters.AddWithValue("_Length", ViewModel.Length);
                    cmdd.Parameters.AddWithValue("_Weight", ViewModel.Weight);
                    Id = Convert.ToInt32(cmdd.ExecuteScalar().ToString());
                }
            }
            catch (Exception ex)
            {

            }
            return Id;
        }

        public int GetTemplateCasePackCount(int VendorId, string SKU = "", string Title = "")
        {
            int counter = 0;
            try
            {
                if (string.IsNullOrEmpty(SKU) || SKU == "undefined")
                    SKU = "";
                if (string.IsNullOrEmpty(Title) || Title == "undefined")
                    Title = "";
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetShipmentCasePackTemplateCount", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_VendorId", VendorId);
                    cmd.Parameters.AddWithValue("_SKU", SKU);
                    cmd.Parameters.AddWithValue("_Title", Title);
                    counter = Convert.ToInt32(cmd.ExecuteScalar());
                    conn.Close();
                }
            }
            catch (Exception exp)
            {

            }
            return counter;
        }

        public List<CasePackViewModel> GetTemplateCasePackList(int VendorId, string SKU, string Title, int limit, int offset)
        {
            if (string.IsNullOrEmpty(SKU) || SKU == "undefined")
                SKU = "";
            if (string.IsNullOrEmpty(Title) || Title == "undefined")
                Title = "";

            List<CasePackViewModel> list = new List<CasePackViewModel>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetShipmentCasePackTemplateList", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_VendorId", VendorId);
                    cmd.Parameters.AddWithValue("_SKU", SKU);
                    cmd.Parameters.AddWithValue("_Title", Title);
                    cmd.Parameters.AddWithValue("_Limit", limit);
                    cmd.Parameters.AddWithValue("_OffSet", offset);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                CasePackViewModel viewModel = new CasePackViewModel
                                {
                                    CasePackId = reader["CasePackId"] != DBNull.Value ? Convert.ToInt32(reader["CasePackId"]) : 0,
                                    VendorId = Convert.ToInt32(reader["VendorId"]),
                                    UserAlias = reader["UserAlias"] != DBNull.Value ? (string)reader["UserAlias"] : "",
                                    QtyPerBox = reader["QtyPerBox"] != DBNull.Value ? Convert.ToInt32(reader["QtyPerBox"]) : 0,
                                    SKU = reader["SKU"] != DBNull.Value ? Convert.ToString(reader["SKU"]) : "",
                                    Width = reader["Width"] != DBNull.Value ? Convert.ToDecimal(reader["Width"]) : 0,
                                    Height = reader["Heigth"] != DBNull.Value ? Convert.ToDecimal(reader["Heigth"]) : 0,
                                    Length = reader["Length"] != DBNull.Value ? Convert.ToDecimal(reader["Length"]) : 0,
                                    Weight = reader["Weight"] != DBNull.Value ? Convert.ToDecimal(reader["Weight"]) : 0,
                                    Title = reader["title"] != DBNull.Value ? (string)reader["title"] : "",
                                    CompressedImage = reader["Compress_image"] != DBNull.Value ? (string)reader["Compress_image"] : "",
                                    ImageName = reader["image_name"] != DBNull.Value ? (string)reader["image_name"] : "",
                                    Counter = reader["Counter"] != DBNull.Value ? Convert.ToInt32(reader["Counter"]) : 0,
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


        public int DeleteCasePack(int Id)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmdd = new MySqlCommand("p_DeleteShipmentSKUCasePack", conn);
                    cmdd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmdd.Parameters.AddWithValue("_Id", Id);
                    cmdd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {

            }
            return Id;
        }
    }
}
