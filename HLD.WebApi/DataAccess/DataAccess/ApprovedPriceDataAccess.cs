using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Helper;
using DataAccess.ViewModels;
using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;

namespace DataAccess.DataAccess
{
    public class ApprovedPriceDataAccess
    {
        public string connStr { get; set; }
        ProductWarehouseQtyDataAccess ProductWHQtyDataAccess = null;
        TagDataAccess _tagDataAccess = null;
        public ApprovedPriceDataAccess(IConnectionString connectionString)
        {
            connStr = connectionString.GetConnectionString();
            ProductWHQtyDataAccess = new ProductWarehouseQtyDataAccess(connectionString);
            _tagDataAccess = new TagDataAccess(connectionString);
        }

        public int SaveApprovedPrice(ApprovedPriceViewModel viewModel)
        {
            int id = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_SaveAprrovedPrice", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_SKU", viewModel.SKU);
                    cmd.Parameters.AddWithValue("_VendorId", viewModel.VendorId);
                    cmd.Parameters.AddWithValue("_VendorAlias", viewModel.VendorAlias);
                    cmd.Parameters.AddWithValue("_ApprovedUnitPrice", viewModel.ApprovedUnitPrice);
                    cmd.Parameters.AddWithValue("_Currency", viewModel.Currency);
                    cmd.Parameters.AddWithValue("_PriceStatus", viewModel.PriceStatus);
                    cmd.Parameters.AddWithValue("_Date", viewModel.Date);
                    cmd.Parameters.Add("_idApprovedPrice", MySqlDbType.Int32);
                    cmd.Parameters["_idApprovedPrice"].Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    id = (int)(cmd.Parameters["_idApprovedPrice"].Value);
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return id;
        }
        public bool EditApprovedPrice(ApprovedPriceViewModel viewModel)
        {
            bool id = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_EditApprovedPrice", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_SKU", viewModel.SKU);
                    cmd.Parameters.AddWithValue("_VendorId", viewModel.VendorId);
                    cmd.Parameters.AddWithValue("_VendorAlias", viewModel.VendorAlias);
                    cmd.Parameters.AddWithValue("_ApprovedUnitPrice", viewModel.ApprovedUnitPrice);
                    cmd.Parameters.AddWithValue("_Currency", viewModel.Currency);
                    cmd.Parameters.AddWithValue("_PriceStatus", viewModel.PriceStatus);
                    cmd.Parameters.AddWithValue("_Date", viewModel.Date);
                    cmd.Parameters.AddWithValue("_idApprovedPrice", viewModel.idApprovedPrice);

                    cmd.ExecuteNonQuery();

                    conn.Close();
                    id = true;
                }
            }
            catch (Exception exp)
            {
            }
            return id;
        }
        public int GetApprovedPriceCount(int VendorId, string SKU, string Title, string skuList)
        {
            if (string.IsNullOrEmpty(SKU) || SKU == "undefined")
                SKU = "";
            if (string.IsNullOrEmpty(Title) || Title == "undefined")
                Title = "";
            if (string.IsNullOrEmpty(skuList) || skuList == "undefined")
                skuList = "Nill";
            int Counter = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("ZTestingApprovedProceCountCopy", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_VendorId", VendorId);
                    cmd.Parameters.AddWithValue("_SKU", SKU);
                    cmd.Parameters.AddWithValue("_Title", Title);
                    cmd.Parameters.AddWithValue("skuList", skuList);
                    //cmd.Parameters.Add("Counter", MySqlDbType.Int32);
                    //cmd.Parameters["Counter"].Direction = ParameterDirection.Output;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Counter = reader["counter"] != DBNull.Value ? Convert.ToInt32(reader["counter"]) : 0;
                            }
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

        public List<ApprovedPriceViewModel> GetApprovedPricesList(int VendorId, int Limit, int Offset, string SKU, string Title, string skuList)
        {
            List<ApprovedPriceViewModel> list = new List<ApprovedPriceViewModel>();
            if (SKU == null)
                SKU = "";
            if (Title == null)
                Title = "";
            if (string.IsNullOrEmpty(skuList) || skuList == "undefined")
                skuList = "Nill";
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetApprovedPricesCopy", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_VendorId", VendorId);
                    cmd.Parameters.AddWithValue("_Limit", Limit);
                    cmd.Parameters.AddWithValue("_OffSet", Offset);
                    cmd.Parameters.AddWithValue("_SKU", SKU);
                    cmd.Parameters.AddWithValue("_Title", Title);
                    cmd.Parameters.AddWithValue("skuList", skuList);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                ApprovedPriceViewModel viewModel = new ApprovedPriceViewModel
                                {
                                    idApprovedPrice = (int)reader["idApprovedPrice"],
                                    SKU = (string)reader["SKU"],
                                    ImageName = reader["image_name"] != DBNull.Value ? (string)reader["image_name"] : "",
                                    CompressedImage = reader["compress_image"] != DBNull.Value ? (string)reader["compress_image"] : "",
                                    VendorId = (int)reader["VendorId"],
                                    VendorAlias = (string)reader["VendorAlias"],
                                    ApprovedUnitPrice = (decimal)reader["ApprovedUnitPrice"],
                                    USD = (decimal)reader["USD"],
                                    CAD = (decimal)reader["CAD"],
                                    YEN = (decimal)reader["CNY"],
                                    Currency = reader["Currency"] != DBNull.Value ? (string)reader["Currency"] : "",
                                    Date = (DateTime)reader["Date"],
                                    PriceStatus = (UInt64)reader["PriceStatus"] == 0 ? false : true,
                                    History = (UInt64)reader["Hostory"] == 0 ? false : true
                                };
                                viewModel.ProductTitle = reader["ProductTitle"] != DBNull.Value ? (string)reader["ProductTitle"] : "";
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

        public List<ApprovedPriceViewModel> GetApprovedPricesLog(int VendorId, string SKU)
        {
            List<ApprovedPriceViewModel> list = new List<ApprovedPriceViewModel>();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetApprovedPricesLogs", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_VendorId", VendorId);

                    cmd.Parameters.AddWithValue("_SKU", SKU);


                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                ApprovedPriceViewModel viewModel = new ApprovedPriceViewModel
                                {
                                    idApprovedPrice = (int)reader["idApprovedPriceHistoryLog"],
                                    SKU = (string)reader["SKU"],
                                    ImageName = reader["image_name"] != DBNull.Value ? (string)reader["image_name"] : "",
                                    CompressedImage = reader["compress_image"] != DBNull.Value ? (string)reader["compress_image"] : "",
                                    VendorId = (int)reader["VendorId"],
                                    VendorAlias = (string)reader["VendorAlias"],
                                    ApprovedUnitPrice = (decimal)reader["ApprovedUnitPrice"],
                                    USD = (decimal)reader["USD"],
                                    CAD = (decimal)reader["CAD"],
                                    YEN = (decimal)reader["CNY"],
                                    Currency = reader["Currency"] != DBNull.Value ? (string)reader["Currency"] : "",
                                    Date = (DateTime)reader["Date"],
                                    PriceStatus = (UInt64)reader["PriceStatus"] == 0 ? false : true,

                                };
                                viewModel.ProductTitle = reader["ProductTitle"] != DBNull.Value ? (string)reader["ProductTitle"] : "";
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

        public ApprovedPriceViewModel GetApprovedPricesForedit(int id)
        {
            ApprovedPriceViewModel viewModel = new ApprovedPriceViewModel();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetApprovedPriceForEdit", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("Id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {

                                viewModel.idApprovedPrice = (int)reader["idApprovedPrice"];
                                viewModel.SKU = (string)reader["SKU"];

                                viewModel.VendorId = (int)reader["VendorId"];
                                viewModel.VendorAlias = (string)reader["VendorAlias"];
                                viewModel.ApprovedUnitPrice = (decimal)reader["ApprovedUnitPrice"];

                                viewModel.Currency = reader["Currency"] != DBNull.Value ? (string)reader["Currency"] : "";
                                viewModel.Date = (DateTime)reader["Date"];
                                viewModel.PriceStatus = (UInt64)reader["PriceStatus"] == 0 ? false : true;


                            }
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {

            }
            return viewModel;
        }


        public List<GetVendorListViewModel> GetAllVendorForAutoComplete(string name)
        {
            List<GetVendorListViewModel> listViewModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetVendorList", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_UserAlias", name.Trim());
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            listViewModel = new List<GetVendorListViewModel>();
                            while (reader.Read())
                            {
                                GetVendorListViewModel ViewModel = new GetVendorListViewModel();
                                ViewModel.UserName = Convert.ToInt32(reader["UserName"]);
                                ViewModel.UserAlias = Convert.ToString(reader["UserAlias"]);
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

        public List<GetVendorListViewModel> GetAllVendorForAutoCompleteFocus()
        {
            List<GetVendorListViewModel> listViewModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetVendorListAutoComplete", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            listViewModel = new List<GetVendorListViewModel>();
                            while (reader.Read())
                            {
                                GetVendorListViewModel ViewModel = new GetVendorListViewModel();
                                ViewModel.UserName = Convert.ToInt32(reader["UserName"]);
                                ViewModel.UserAlias = Convert.ToString(reader["UserAlias"]);
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

        public bool AddNotesInApprovedPrice(ApprovedPriceViewModel viewModel)
        {
            bool status =false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_SaveNotesInApprovedPriceNotes", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Notes", viewModel.HasNotes);
                    cmd.ExecuteNonQuery();
                    status = true;
                    conn.Close();

                }
            }
            catch (Exception exp)
            {
            }
            return status;
        }
    }

}
