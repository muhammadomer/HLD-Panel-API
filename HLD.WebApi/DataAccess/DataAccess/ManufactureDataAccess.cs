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

        public List<GetManufactureModelViewModel> GetManufactureModel(int Id)
        {
            List<GetManufactureModelViewModel> listModel = new List<GetManufactureModelViewModel>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetManufactureModels", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("Id", Id);

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

        public List<GetDeviceModelViewMdel> GetDeviceModelModel(int Id)
        {
            List<GetDeviceModelViewMdel> listModel = new List<GetDeviceModelViewMdel>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetDevicModelList", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("Id", Id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            listModel = new List<GetDeviceModelViewMdel>();
                            while (reader.Read())
                            {
                                GetDeviceModelViewMdel model = new GetDeviceModelViewMdel();
                                model.ManufactureId = Convert.ToInt32(reader["ManufacturesId"] != DBNull.Value ? reader["ManufacturesId"] : 0);
                                model.DeviceModel = Convert.ToString(reader["DeviceModel"] != DBNull.Value ? reader["DeviceModel"] : "0");
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
                    cmd.Parameters.AddWithValue("_manufacturer", model.Manufacturer);
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
                    cmd.Parameters.AddWithValue("_manufacturerModel", model.ManufacturerModel);
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


    }
}
