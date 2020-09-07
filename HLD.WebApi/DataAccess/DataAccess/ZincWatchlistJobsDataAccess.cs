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
    public class ZincWatchlistJobsDataAccess
    {
        public string connStr { get; set; }
        public ZincWatchlistJobsDataAccess(IConnectionString connectionString)
        {
            connStr = connectionString.GetConnectionString();
        }
        public List<SaveWatchlistForjobsViewModel> GetWatchlist()
        {

            List<SaveWatchlistForjobsViewModel> listViewModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetASINforWatchList", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            listViewModel = new List<SaveWatchlistForjobsViewModel>();
                            while (reader.Read())
                            {
                                SaveWatchlistForjobsViewModel ViewModel = new SaveWatchlistForjobsViewModel();
                                ViewModel.ASIN = Convert.ToString(reader["ASIN"]);
                                ViewModel.ProductSKU = Convert.ToString(reader["ProductSKU"]);
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

    }
}
