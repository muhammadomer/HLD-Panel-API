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
    public class ShipmentMultiBoxDataAccess
    {
        public string ConStr { get; set; }
        public ShipmentMultiBoxDataAccess(IConnectionString connectionString)
        {

            ConStr = connectionString.GetConnectionString();
        }

        public bool SaveShipment(ShipmentMultiBoxViewModel ViewModel)
        {
            bool status = false;
            try
            {
                for (int i = 0; i < ViewModel.Boxes; i++)
                {
                    using (MySqlConnection conn = new MySqlConnection(ConStr))
                    {
                        conn.Open();
                        MySqlCommand cmdd = new MySqlCommand("p_SaveShipment", conn);
                        cmdd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmdd.Parameters.AddWithValue("_ShipmentId", ViewModel.ShipmentId);
                        cmdd.Parameters.AddWithValue("_Height", ViewModel.Height);
                        cmdd.Parameters.AddWithValue("_Width", ViewModel.Width);
                        cmdd.Parameters.AddWithValue("_Length", ViewModel.Length);
                        cmdd.Parameters.AddWithValue("_Weight", ViewModel.Weight);
                        cmdd.Parameters.AddWithValue("_VendorId", ViewModel.VendorId);
                        cmdd.Parameters.AddWithValue("_POId", ViewModel.POId);
                        cmdd.Parameters.AddWithValue("_SKU", ViewModel.SKU);
                        cmdd.Parameters.AddWithValue("_ShipedQty", ViewModel.QtyPerBox);
                        cmdd.ExecuteNonQuery();
                        status = true;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return status;
        }
    }
}
