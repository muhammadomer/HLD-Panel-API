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
    public class OrderNotesDataAccess
    {
        public string connStr { get; set; }
        public OrderNotesDataAccess(IConnectionString connectionString)
        {
            connStr = connectionString.GetConnectionString();
        }

        public bool SaveOrderNotes(List<CreateOrderNotesViewModel> ViewModellist)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    foreach (var ViewModel in ViewModellist)
                    {
                        MySqlCommand cmd = new MySqlCommand("p_SaveOrderNotes", conn);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("_EntityID", ViewModel.EntityID);
                        cmd.Parameters.AddWithValue("_NoteID", ViewModel.NoteID);
                        cmd.Parameters.AddWithValue("_Note", ViewModel.Note);
                        cmd.Parameters.AddWithValue("_AuditData", ViewModel.AuditDate);
                        cmd.Parameters.AddWithValue("_CreatedByName", ViewModel.CreatedByName);

                        cmd.ExecuteNonQuery();
                    }
                   

                }
                status = true;


            }
            catch (Exception ex)
            {
            }
            return status;
        }
        public bool UpdateOrderAsHavingNotes(int orderID)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                  
                        MySqlCommand cmd = new MySqlCommand("P_UpdateOrderAsHavingNotes", conn);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("OrderID", orderID);
                       
                        cmd.ExecuteNonQuery();
                   
                }
                status = true;


            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public List<GetNotesOrderViewModel> GetOrderNotes(int OrderID)
        {
            List<GetNotesOrderViewModel> _ViewModels = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetOrderNotes", conn);
                    cmd.Parameters.AddWithValue("OrderID", OrderID);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            _ViewModels = new List<GetNotesOrderViewModel>();
                            while (reader.Read())
                            {
                                GetNotesOrderViewModel ViewModel = new GetNotesOrderViewModel();
                                ViewModel.EntityID = Convert.ToInt32(reader["EntityID"]);
                                ViewModel.AuditDate = Convert.ToString(reader["AuditData"]);
                                ViewModel.Note = Convert.ToString(reader["Note"]);
                                ViewModel.CreatedByName = Convert.ToString(reader["CreatedByName"]);
                                _ViewModels.Add(ViewModel);
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



    }
}
