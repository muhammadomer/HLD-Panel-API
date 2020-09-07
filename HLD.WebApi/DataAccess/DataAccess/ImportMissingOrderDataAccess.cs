using DataAccess.Helper;
using DataAccess.ViewModels;
using Hld.WebApi.ViewModel;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataAccess
{
   public class ImportMissingOrderDataAccess
    {
        public string connStr { get; set; }
        public ImportMissingOrderDataAccess(IConnectionString connectionString)
        {
            connStr = connectionString.GetConnectionString();
        }

        public MissingOrderReturnViewModel CheckOrderINDB(List<CheckMissingOrderViewModel> viewModel)
        {
            List<int> extid = new List<int>();
            List<int> msdid = new List<int>();
            MissingOrderReturnViewModel listModel = new MissingOrderReturnViewModel();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    foreach (var item in viewModel)
                    {
                        MySqlCommand cmdd = new MySqlCommand(@"SELECT sellerCloudID FROM bestBuyE2.orders inner join bestBuyE2.SellerCloudOrders on orders.sellerCloudID = SellerCloudOrders.seller_cloud_order_id where seller_cloud_order_id= "+item.OrderID, conn);
                        cmdd.CommandType = System.Data.CommandType.Text;
                        MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(cmdd);
                        DataTable dt = new DataTable();
                        mySqlDataAdapter.Fill(dt);
                       
                        if (dt.Rows.Count > 0)
                        {

                            foreach (DataRow dr in dt.Rows)
                            {
                                 
                                
                                int id = Convert.ToInt32(dr["sellerCloudID"]);
                                if ( id != 0)
                                {
                                    listModel.ExistingOrderCount++;

                                    extid.Add(id);
                                       
                                    
                                }
                               
                            }

                        }
                        else
                        {
                            listModel.MissingOrderCount++;
                            msdid.Add(item.OrderID);
                        }

                    }
                    listModel.ExistingOrder = extid;
                    listModel.MissingOrder = msdid;

                }
                return listModel;
            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}
