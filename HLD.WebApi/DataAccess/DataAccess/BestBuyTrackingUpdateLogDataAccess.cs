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
    public class BestBuyTrackingUpdateLogDataAccess
    {
        public string connStr { get; set; }
        public BestBuyTrackingUpdateLogDataAccess(IConnectionString connectionString)
        {
            connStr = connectionString.GetPhpConnectionString();
        }
        public List<BestBuyTrackingUpdate> GetAllBestBuyUpdateLog()
        {
            List<BestBuyTrackingUpdate> listModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(@"SELECT shipDate,scOrderID,bbOrderID,trackingNumber,shippingServiceCode,case when inBestbuy=1 then 'Update' else 'Pending' end as BBStatus FROM bestBuyE2.trackingExport
                                       order by shipDate desc limit 500; ", conn);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    da.Fill(dataTable);
                    if (dataTable.Rows.Count > 0)
                    {
                        listModel = new List<BestBuyTrackingUpdate>();
                        foreach (DataRow dataRow in dataTable.Rows)
                        {
                            BestBuyTrackingUpdate model = new BestBuyTrackingUpdate();
                            model.trackingNumber = Convert.ToString(dataRow["trackingNumber"] != DBNull.Value ? dataRow["trackingNumber"] : "0");
                            model.BBStatus = Convert.ToString(dataRow["BBStatus"] != DBNull.Value ? dataRow["BBStatus"] : "0");
                            model.shipDate = Convert.ToDateTime(dataRow["shipDate"] != DBNull.Value ? dataRow["shipDate"] : "0");
                            model.scOrderID = Convert.ToString(dataRow["scOrderID"] != DBNull.Value ? dataRow["scOrderID"] : "0");
                            model.shippingServiceCode = Convert.ToString(dataRow["shippingServiceCode"] != DBNull.Value ? dataRow["shippingServiceCode"] : "0");
                            model.bbOrderID = Convert.ToString(dataRow["bbOrderID"] != DBNull.Value ? dataRow["bbOrderID"] : "0");
                            listModel.Add(model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return listModel;
        }
        public List<BestBuyTrackingUpdate> GetByDynamicquery(string query)
        {
            List<BestBuyTrackingUpdate> listModel = new List<BestBuyTrackingUpdate>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    da.Fill(dataTable);
                    if (dataTable.Rows.Count > 0)
                    {
                      
                        foreach (DataRow dataRow in dataTable.Rows)
                        {
                            BestBuyTrackingUpdate model = new BestBuyTrackingUpdate();
                            model.trackingNumber = Convert.ToString(dataRow["trackingNumber"] != DBNull.Value ? dataRow["trackingNumber"] : "0");
                            model.BBStatus = Convert.ToString(dataRow["BBStatus"] != DBNull.Value ? dataRow["BBStatus"] : "0");
                            model.shipDate = Convert.ToDateTime(dataRow["shipDate"] != DBNull.Value ? dataRow["shipDate"] : "0");
                            model.scOrderID = Convert.ToString(dataRow["scOrderID"] != DBNull.Value ? dataRow["scOrderID"] : "0");
                            model.shippingServiceCode = Convert.ToString(dataRow["shippingServiceCode"] != DBNull.Value ? dataRow["shippingServiceCode"] : "0");
                            model.bbOrderID = Convert.ToString(dataRow["bbOrderID"] != DBNull.Value ? dataRow["bbOrderID"] : "0");
                            listModel.Add(model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return listModel;
        }
    }

}
