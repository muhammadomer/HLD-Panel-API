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
    public class ShipmentProductDataAccess
    {
        public string ConStr { get; set; }

        public ShipmentProductDataAccess(IConnectionString connectionString)
        {

            ConStr = connectionString.GetConnectionString();
        }

        public int SaveShipmentProduct(ShipmentProductViewModel ViewModel)
        {
            int Id = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmdd = new MySqlCommand("p_SaveShipmentProduct", conn);
                    cmdd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmdd.Parameters.AddWithValue("_ShipmentId", ViewModel.ShipmentId);
                    cmdd.Parameters.AddWithValue("_VendorId", ViewModel.VendorId);
                    cmdd.Parameters.AddWithValue("_POId", ViewModel.POId);
                    cmdd.Parameters.AddWithValue("_SKU", ViewModel.SKU);
                    cmdd.Parameters.AddWithValue("_BoxId", ViewModel.BoxId);
                    cmdd.Parameters.AddWithValue("_OpenQty", ViewModel.OpenQty);
                    cmdd.Parameters.AddWithValue("_ShipedQty", ViewModel.ShipedQty);
                    Id = Convert.ToInt32(cmdd.ExecuteScalar().ToString());
                }
            }
            catch (Exception ex)
            {

            }
            return Id;
        }

        public long GetShipmentProductListCount(int VendorId)
        {
            long Counter = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetShipmentProductsCount", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_VendorId", VendorId);
                    Counter = (long)cmd.ExecuteScalar();
                    conn.Close();
                }
            }
            catch (Exception exp)
            {
            }
            return Counter;
        }

        public List<ShipmentProductListViewModel> GetShipmentProductsList(int VendorId, int Limit, int Offset)
        {
            List<ShipmentProductListViewModel> list = new List<ShipmentProductListViewModel>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetShipmentProductsList", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_VendorId", VendorId);
                    cmd.Parameters.AddWithValue("_Limit", Limit);
                    cmd.Parameters.AddWithValue("_OffSet", Offset);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                ShipmentProductListViewModel viewModel = new ShipmentProductListViewModel
                                {
                                    idShipmentProducts = reader["idShipmentProducts"] != DBNull.Value ? (int)reader["idShipmentProducts"] : 0,
                                    ShipmentId = (string)reader["ShipmentId"],
                                    BoxId = reader["BoxId"] != DBNull.Value ? (string)reader["BoxId"] : "",
                                    Vendor = reader["Vendor"] != DBNull.Value ? (string)reader["Vendor"] : "",
                                    VendorId = (int)reader["VendorId"],
                                    SKU = reader["SKU"] != DBNull.Value ? (string)reader["SKU"] : "",
                                    POId = reader["POId"] != DBNull.Value ? (int)reader["POId"] : 0,
                                    OpenQty = reader["OpenQty"] != DBNull.Value ? (int)reader["OpenQty"] : 0,
                                    ShipedQty = reader["ShipedQty"] != DBNull.Value ? (int)reader["ShipedQty"] : 0,
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

        public ShipmentProductHeaderViewModel GetListByBoxId(string BoxId)
        {
            List<ShipmentProductListViewModel> list = new List<ShipmentProductListViewModel>();
            ShipmentProductHeaderViewModel Item = new ShipmentProductHeaderViewModel();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetShipmentProductHeaderWithList", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Id", BoxId);
                    DataSet ds = new DataSet();
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(ds);
                    DataTable dt1 = ds.Tables[0];
                    DataTable dt2 = ds.Tables[1];
                    foreach (DataRow reader in dt1.Rows)
                    {
                        ShipmentProductHeaderViewModel Obj = new ShipmentProductHeaderViewModel()
                        {
                            ShipmentId = (string)reader["ShipmentId"],
                            BoxId = reader["BoxId"] != DBNull.Value ? (string)reader["BoxId"] : "",
                            ShipmentName = (string)reader["ShipmentName"],
                            Notes = reader["Notes"] != DBNull.Value ? (string)reader["Notes"] : "",
                            Width = reader["Width"] != DBNull.Value ? (Decimal)reader["Width"] : 0,
                            Height = reader["Height"] != DBNull.Value ? (Decimal)reader["Height"] : 0,
                            Length = reader["Length"] != DBNull.Value ? (Decimal)reader["Length"] : 0,
                            Weight = reader["Weight"] != DBNull.Value ? (decimal)reader["Weight"] : 0,
                            Status = reader["Status"] != DBNull.Value ? Convert.ToInt32(reader["Status"]) : 0,
                            SKUs = reader["SKUs"] != DBNull.Value ? Convert.ToInt32(reader["SKUs"]) : 0,
                            POs = reader["POs"] != DBNull.Value ? Convert.ToInt32(reader["POs"]) : 0,
                        };
                        Item = Obj;
                    }

                    foreach (DataRow reader in dt2.Rows)
                    {
                        ShipmentProductListViewModel viewModel = new ShipmentProductListViewModel
                        {
                            idShipmentProducts = reader["idShipmentProducts"] != DBNull.Value ? (int)reader["idShipmentProducts"] : 0,
                            ShipmentId = reader["ShipmentId"] != DBNull.Value ? (string)reader["ShipmentId"] : "",
                            BoxId = reader["BoxId"] != DBNull.Value ? (string)reader["BoxId"] : "",
                            Vendor = reader["Vendor"] != DBNull.Value ? (string)reader["Vendor"] : "",
                            VendorId = (int)reader["VendorId"],
                            SKU = reader["SKU"] != DBNull.Value ? (string)reader["SKU"] : "",
                            POId = reader["POId"] != DBNull.Value ? (int)reader["POId"] : 0,
                            CompressedImage = reader["Compress_image"] != DBNull.Value ? (string)reader["Compress_image"] : "",
                            ImageName = reader["image_name"] != DBNull.Value ? (string)reader["image_name"] : "",
                            OpenQty = reader["QtyOpen"] != DBNull.Value ? Convert.ToInt32(reader["QtyOpen"]) : 0,
                            BalanceQty = reader["BalanceQty"] != DBNull.Value ? Convert.ToInt32(reader["BalanceQty"]) : 0,
                            ShipedQty = reader["ShipedQty"] != DBNull.Value ? (int)reader["ShipedQty"] : 0,
                            Title = reader["title"] != DBNull.Value ? (string)reader["title"] : "",
                        };
                        list.Add(viewModel);
                    }
                    conn.Close();
                }
                Item.list = list;
            }
            catch (Exception ex)
            {

            }
            return Item;
        }

        public ShipmentProductViewModel GetShipmentProductForedit(int id)
        {
            ShipmentProductViewModel viewModel = new ShipmentProductViewModel();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetShipmentProductForEdit", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("Id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {

                                ShipmentProductViewModel Obj = new ShipmentProductViewModel
                                {
                                    idShipmentProducts = reader["idShipmentProducts"] != DBNull.Value ? (int)reader["idShipmentProducts"] : 0,
                                    ShipmentId = (string)reader["ShipmentId"],
                                    BoxId = reader["BoxId"] != DBNull.Value ? (string)reader["BoxId"] : "",
                                    Vendor = reader["Vendor"] != DBNull.Value ? (string)reader["Vendor"] : "",
                                    VendorId = (int)reader["VendorId"],
                                    SKU = reader["SKU"] != DBNull.Value ? (string)reader["SKU"] : "",
                                    POId = reader["POId"] != DBNull.Value ? (int)reader["POId"] : 0,
                                    OpenQty = reader["OpenQty"] != DBNull.Value ? (int)reader["OpenQty"] : 0,
                                    ShipedQty = reader["ShipedQty"] != DBNull.Value ? (int)reader["ShipedQty"] : 0,
                                };
                                viewModel = Obj;
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

        public int UpdateShipmentProduct(ShipmentProductViewModel viewModel)
        {
            int Id = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_UpdateShipmentProduct", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Id", viewModel.idShipmentProducts);
                    cmd.Parameters.AddWithValue("_ShipedQty", viewModel.ShipedQty);
                    //cmd.ExecuteNonQuery();
                    Id = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                    conn.Close();
                    //status = true;
                }
            }
            catch (Exception exp)
            {
            }
            return Id;
        }

        public bool DeleteShipmentProduct(int Id)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_DeleteShipmentProduct", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Id", Id);
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

        public long GetShipmentProductListCountByBarcode(string BoxId)
        {
            long Counter = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetShipmentProductsByBarcodeCount", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Id", BoxId);
                    Counter = (long)cmd.ExecuteScalar();
                    conn.Close();
                }
            }
            catch (Exception exp)
            {
            }
            return Counter;
        }

        public List<ShipmentProductListViewModel> GetShipmentProductsListByBarcode(string BoxId, int Limit, int Offset)
        {
            List<ShipmentProductListViewModel> list = new List<ShipmentProductListViewModel>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetShipmentProductsByBarcode", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Id", BoxId);
                    cmd.Parameters.AddWithValue("_Limit", Limit);
                    cmd.Parameters.AddWithValue("_OffSet", Offset);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                ShipmentProductListViewModel viewModel = new ShipmentProductListViewModel
                                {
                                    idShipmentProducts = reader["idShipmentProducts"] != DBNull.Value ? (int)reader["idShipmentProducts"] : 0,
                                    ShipmentId = (string)reader["ShipmentId"],
                                    BoxId = reader["BoxId"] != DBNull.Value ? (string)reader["BoxId"] : "",
                                    Vendor = reader["Vendor"] != DBNull.Value ? (string)reader["Vendor"] : "",
                                    VendorId = (int)reader["VendorId"],
                                    SKU = reader["SKU"] != DBNull.Value ? (string)reader["SKU"] : "",
                                    Title = reader["title"] != DBNull.Value ? (string)reader["title"] : "",
                                    Description = reader["description"] != DBNull.Value ? (string)reader["description"] : "",
                                    CompressedImage = reader["Compress_image"] != DBNull.Value ? (string)reader["Compress_image"] : "",
                                    ImageName = reader["image_name"] != DBNull.Value ? (string)reader["image_name"] : "",
                                    POId = reader["POId"] != DBNull.Value ? (int)reader["POId"] : 0,
                                    SCItemID = reader["SCItemID"] != DBNull.Value ? Convert.ToInt32(reader["SCItemID"]) : 0,
                                    OpenQty = reader["OpenQty"] != DBNull.Value ? (int)reader["OpenQty"] : 0,
                                    ShipedQty = reader["ShipedQty"] != DBNull.Value ? (int)reader["ShipedQty"] : 0,
                                    ReceivedQty = reader["RecivedQty"] != DBNull.Value ? (int)reader["RecivedQty"] : 0,
                                    LocationNotes = reader["LocationNotes"] != DBNull.Value ? Convert.ToString(reader["LocationNotes"]) : "",
                                    PhysicalInventory = reader["PhysicalInventory"] != DBNull.Value ? Convert.ToString(reader["PhysicalInventory"]) : "",
                                    ShadowOf = reader["ShadowOf"] != DBNull.Value ? Convert.ToString(reader["ShadowOf"]) : "",
                                    QtyPerCase = reader["QtyPerCase"] != DBNull.Value ? Convert.ToInt32(reader["QtyPerCase"]) : 0,
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



        public int UpdateRecivedQty(ShipmentProductListViewModel Obj)
        {
            int Id = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_UpdatePOIShipedQty", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Id", Obj.idShipmentProducts);
                    cmd.Parameters.AddWithValue("_ReceivedQty", Obj.ReceivedQty);
                    Id = Convert.ToInt32(cmd.ExecuteScalar());
                    conn.Close();

                }
            }
            catch (Exception exp)
            {
            }
            return Id;
        }

        public int GetPOIID(int idShipmentProducts)
        {
            int Id = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetPOIID", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Id", idShipmentProducts);
                    // Id = Convert.ToInt64(cmd.ExecuteScalar());

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Id = reader["ID"] != DBNull.Value ? Convert.ToInt32(reader["ID"]) : 0;
                            }
                        }
                    }

                    conn.Close();

                }
            }
            catch (Exception exp)
            {

            }
            return Id;
        }
        public int UpdateShipmentProductInventory(ShipmentProductListViewModel viewModel)
        {
            int Id = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_UpdateShipmentProductInventory", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_SKU", viewModel.SKU);
                    cmd.Parameters.AddWithValue("_LocationNotes", viewModel.LocationNotes);
                    cmd.Parameters.AddWithValue("_PhysicalInventory", viewModel.PhysicalInventory);
                    cmd.Parameters.AddWithValue("_QtyPerCase", viewModel.QtyPerCase);
                    cmd.Parameters.AddWithValue("_ShadowOf", viewModel.ShadowOf);
                    cmd.ExecuteNonQuery();
                    //Id = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                    conn.Close();
                    //status = true;
                }
            }
            catch (Exception exp)
            {
            }
            return Id;
        }

        public int UpdateShipmentStatus(ShipmentViewModel Obj)
        {
            int Id = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_UpdateShipmentStatus", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Id", Obj.ShipmentId);
                    cmd.Parameters.AddWithValue("_Status", Obj.Status);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exp)
            {
            }
            return Id;
        }

        public int SetShipmentasReceived(ShipmentViewModel Obj)
        {
            int Id = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_SetShipmentasReceived", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Id", Obj.ShipmentId);
                    cmd.Parameters.AddWithValue("_Status", Obj.Status);
                    cmd.Parameters.AddWithValue("_Date", Obj.ReceivedDate);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exp)
            {

            }
            return Id;
        }

    }
}
