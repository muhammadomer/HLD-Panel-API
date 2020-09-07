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
    public class AutoControlDataAccess
    {
        public string connStr { get; set; }
        public AutoControlDataAccess(IConnectionString connectionString)
        {
            connStr = connectionString.GetConnectionString();
        }
        public List<AutoControlViewModel> GetControls()
        {
            List<AutoControlViewModel> listModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmdd = new MySqlCommand(@"SELECT Contorls,status FROM bestBuyE2.AutoContorls;", conn);
                    cmdd.CommandType = System.Data.CommandType.Text;
                    MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(cmdd);
                    DataTable dt = new DataTable();
                    mySqlDataAdapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        listModel = new List<AutoControlViewModel>();
                        foreach (DataRow dr in dt.Rows)
                        {
                            AutoControlViewModel model = new AutoControlViewModel();
                          
                            model.JobName = Convert.ToString(dr["Contorls"]);
                            model.StatusID = Convert.ToInt32(dr["status"]);
                            listModel.Add(model);
                        }
                    }


                }
                return listModel;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public bool EnableDisableZincJobs(AutoControlViewModel ViewModel)
        {

            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_UpdateAutoControlJobs", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("StatusId", ViewModel.StatusID);
                    cmd.Parameters.AddWithValue("JobName", ViewModel.JobName);
                  
                    cmd.ExecuteNonQuery();

                }
                status = true;


            }
            catch (Exception ex)
            {
            }
            return status;
        }


    }
}
