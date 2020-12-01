using DataAccess.Helper;
using DataAccess.ViewModels;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataAccess
{
    public class ManufactureDataAccess
    {
        public string connStr { get; set; }

        public ManufactureDataAccess(IConnectionString connectionString)
        {
            connStr = connectionString.GetConnectionString();
        }
        public List<GetManufactureViewModel> GetManufacture()
        {
            List<GetManufactureViewModel> listModel = new List<GetManufactureViewModel>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetMenufactureList", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            listModel = new List<GetManufactureViewModel>();
                            while (reader.Read())
                            {
                                GetManufactureViewModel model = new GetManufactureViewModel();
                                model.ManufactureId = Convert.ToInt32(reader["ManufacturesId"] != DBNull.Value ? reader["ManufacturesId"] : 0);
                                model.ManufactureName = Convert.ToString(reader["Manufacturer"] != DBNull.Value ? reader["Manufacturer"] : " ");
                                listModel.Add(model);
                            }
                        }
                    }
                }
            }


            catch (Exception ex)
            {

                throw ex;
            }
            return listModel;
        }
        public List<GetManufactureViewModel> GetManufacturelist()
        {
            List<GetManufactureViewModel> listViewModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetMenufactureList", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            listViewModel = new List<GetManufactureViewModel>();
                            while (reader.Read())
                            {
                                GetManufactureViewModel model = new GetManufactureViewModel();
                                model.ManufactureId = Convert.ToInt32(reader["ManufacturesId"] != DBNull.Value ? reader["ManufacturesId"] : 0);
                                model.ManufactureName = Convert.ToString(reader["Manufacturer"] != DBNull.Value ? reader["Manufacturer"] : " ");
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

        public List<GetManufactureViewModel> GetManufactureName(string name)
        {
            List<GetManufactureViewModel> listViewModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetAllManuFacByName", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Manuname", name.Trim());
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            listViewModel = new List<GetManufactureViewModel>();
                            while (reader.Read())
                            {
                                GetManufactureViewModel model = new GetManufactureViewModel();
                                model.ManufactureId = Convert.ToInt32(reader["ManufacturesId"]);
                                model.ManufactureName = Convert.ToString(reader["Manufacturer"] != DBNull.Value ? reader["Manufacturer"] : "");
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
        public List<GetManufactureModelViewModel> GetManufactureModel(int ManufactureId)
        {
            List<GetManufactureModelViewModel> listModel = new List<GetManufactureModelViewModel>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetManufactureModels", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_manufactureId", ManufactureId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            listModel = new List<GetManufactureModelViewModel>();
                            while (reader.Read())
                            {
                                GetManufactureModelViewModel model = new GetManufactureModelViewModel();
                                model.ManufactureId = Convert.ToInt32(reader["ManufacturesId"] != DBNull.Value ? reader["ManufacturesId"] : 0);
                                model.ManufactureModel = Convert.ToString(reader["ManufactureModel"] != DBNull.Value ? reader["ManufactureModel"] : "0");
                                listModel.Add(model);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return listModel;
        }


        public List<ProductManufactureListViewModel> GetManufactureList(int ManufactureId)
        {
            List<ProductManufactureListViewModel> listModel = new List<ProductManufactureListViewModel>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetAllManufactredModelList", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_ManufacturesId", ManufactureId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            listModel = new List<ProductManufactureListViewModel>();
                            while (reader.Read())
                            {
                                ProductManufactureListViewModel model = new ProductManufactureListViewModel();
                                model.ManufactureId = Convert.ToInt32(reader["ManufacturesId"] != DBNull.Value ? reader["ManufacturesId"] : 0);
                                model.ManufactureName = Convert.ToString(reader["Manufacturer"] != DBNull.Value ? reader["Manufacturer"] : "");
                                model.DeviceModel = Convert.ToString(reader["DeviceModel"] != DBNull.Value ? reader["DeviceModel"] : "");
                                model.ManufactureModel = Convert.ToString(reader["ManufactureModel"] != DBNull.Value ? reader["ManufactureModel"] : "");
                                listModel.Add(model);
                            }
                        }
                    }
                }
            }


            catch (Exception ex)
            {

                throw ex;
            }
            return listModel;
        }


        public List<GetDeviceModelViewMdel> GetDeviceModelModel(int ManufactureModel, int ManufactureId)

        {
            List<GetDeviceModelViewMdel> listModel = new List<GetDeviceModelViewMdel>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetDevicModelList", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_manufacturerModelId", ManufactureModel);
                    cmd.Parameters.AddWithValue("_manufactureId", ManufactureId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            listModel = new List<GetDeviceModelViewMdel>();
                            while (reader.Read())
                            {
                                GetDeviceModelViewMdel model = new GetDeviceModelViewMdel();
                                model.ManufactureId = Convert.ToInt32(reader["ManufacturesId"] != DBNull.Value ? reader["ManufacturesId"] : 0);
                                model.Manufacture = Convert.ToString(reader["Manufacturer"] != DBNull.Value ? reader["Manufacturer"] : "");
                                model.ManufactureModel = Convert.ToString(reader["ManufactureModel"] != DBNull.Value ? reader["ManufactureModel"] : "");
                                model.DeviceModel = Convert.ToString(reader["DeviceModel"] != DBNull.Value ? reader["DeviceModel"] : "");
                                listModel.Add(model);
                            }
                        }
                    }
                }
            }


            catch (Exception ex)
            {

                throw ex;
            }
            return listModel;
        }

        public bool AddManufacture(AddManufactureViewModel model)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_AddManufacturer", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_manufacturer", model.ManufactureName.ToUpper());
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }

        public bool AddManufacturerModel(AddManufacturerModelViewModel model)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_AddManufacturerModel", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_manufacturerModel", model.ManufactureModel);
                    cmd.Parameters.AddWithValue("_manufactureId", model.ManufactureId);
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }


        public bool AddDeviceModel(AddDeviceModelView model)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    MySqlCommand cmd = new MySqlCommand("P_AddDeviceModel", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_manufactureId", model.ManufactureId);
                    cmd.Parameters.AddWithValue("_manufacturerModelId", model.ManufactureModelId);
                    cmd.Parameters.AddWithValue("_deviceModel", model.DeviceModel);
                    cmd.ExecuteNonQuery();
                    status = true;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return status;

        }

        public bool CheckManufactureExists(string name)
        {

            bool status = false;
            try
            {

                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_CheckManufactureExists", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Manufacturer", name.Trim());
                    cmd.Parameters.Add("Statues", MySqlDbType.Bit, 10);
                    cmd.Parameters["Statues"].Direction = System.Data.ParameterDirection.Output;

                    cmd.ExecuteNonQuery();
                    status = Convert.ToBoolean(cmd.Parameters["Statues"].Value);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return status;
        }

        public bool EditManufactureList(EditManufactureListModelView model)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    MySqlCommand cmd = new MySqlCommand("P_EditManufactureList", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_manufacturerId", model.ManufactureId);
                    cmd.Parameters.AddWithValue("_manufacturer", model.Manufacturer);
                    cmd.Parameters.AddWithValue("_manufactureModel", model.ManufactureModel);
                    cmd.Parameters.AddWithValue("_deviceModel", model.DeviceModel);
                    cmd.ExecuteNonQuery();
                    status = true;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return status;

        }
        public bool AddStyle(AddStyleViewModel model)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_AddStyle", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_styleId", model.StyleId);
                    cmd.Parameters.AddWithValue("_styleName", model.StyleName);
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }

        public List<AddStyleViewModel> GetAllStyle()
        {
            List<AddStyleViewModel> listViewModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetAllStyle", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            listViewModel = new List<AddStyleViewModel>();
                            while (reader.Read())
                            {
                                AddStyleViewModel model = new AddStyleViewModel();
                                model.StyleId = Convert.ToInt32(reader["StyleId"] != DBNull.Value ? reader["StyleId"] : 0);
                                model.StyleName = Convert.ToString(reader["StyleName"] != DBNull.Value ? reader["StyleName"] : " ");
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

        public AddStyleViewModel GetStyleWithId(int styleId)

        {
           AddStyleViewModel listModel = new AddStyleViewModel();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    MySqlCommand cmd = new MySqlCommand("P_GetStyleWithId", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_styleId", styleId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            
                            while (reader.Read())
                            {
                                AddStyleViewModel model = new AddStyleViewModel();
                                model.StyleId = Convert.ToInt32(reader["StyleId"] != DBNull.Value ? reader["StyleId"] : 0);
                                model.StyleName = Convert.ToString(reader["StyleName"] != DBNull.Value ? reader["StyleName"] : "");
                                listModel=model;
                            }
                        }
                    }
                }
            }


            catch (Exception ex)
            {

                throw ex;
            }
            return listModel;
        }

        public bool UpdateManufactureGenerically(UpdateManufactureGenericallyViewModel updateManufactureGenericallyViewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_UpdateManufactureGenerically", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("_coloumnName", updateManufactureGenericallyViewModel.ColoumnName);
                    cmd.Parameters.AddWithValue("_coloumnValue", updateManufactureGenericallyViewModel.ColoumnValue);
                    cmd.Parameters.AddWithValue("_id", updateManufactureGenericallyViewModel.Id);
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
    }

}

