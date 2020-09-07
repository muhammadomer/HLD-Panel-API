using DataAccess.DataAccess;
using DataAccess.Helper;
using DataAccess.ViewModels;
using Newtonsoft.Json;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace HLD.WebApi.Jobs
{
    [DisallowConcurrentExecution]
    public class GetProductCatalogDetail : IJob
    {
        IConnectionString _connectionString = null;
        EncDecChannel _EncDecChannel = null;
        string ApiURL = null;
        private readonly ILogger logger;
        ProductDataAccess productData = null;
        GetChannelCredViewModel _getChannelCredViewModel = null;
        ChannelDecrytionDataAccess channelDecrytionDataAccess = null;
        public string connStr { get; set; }
        public GetProductCatalogDetail(IConnectionString connectionString, ILogger<GetProductCatalogDetail> _logger)
        {
            connStr = connectionString.GetConnectionString();
            _connectionString = connectionString;
            productData = new ProductDataAccess(_connectionString);
            ApiURL = "https://lp.api.sellercloud.com/rest/api";
            _EncDecChannel = new EncDecChannel(_connectionString);
            channelDecrytionDataAccess = new ChannelDecrytionDataAccess(connectionString);
            logger = _logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            int status = channelDecrytionDataAccess.CheckZincJobsStatus("GetProductCatalogDetail");
            if (status == 1)
            {
                var SKUs = productData.GetAllSKUsForCatalog();
                logger.LogInformation("GetProductCatalogDetail Job Started At =>" + DateTime.Now.ToString());
                foreach (var sku in SKUs)
                {

                    string ApiURL = "";
                    ApiURL = "https://lp.api.sellercloud.com/rest/api";
                    GetChannelCredViewModel _getChannelCredViewModel = new GetChannelCredViewModel();
                    AuthenticateSCRestViewModel authenticate = new AuthenticateSCRestViewModel();
                    _getChannelCredViewModel = _EncDecChannel.DecryptedData("sellercloud");
                    authenticate = _EncDecChannel.AuthenticateSCForIMportOrder(_getChannelCredViewModel, ApiURL);

                    CatalogViewModel item = new CatalogViewModel();
                    CatalogListViewModel responses = new CatalogListViewModel();
                    try
                    {
                        HttpWebRequest request = (HttpWebRequest)WebRequest
                        .Create(ApiURL + "/Catalog?model.sKU=" + sku);
                        request.Method = "GET";
                        request.Accept = "application/json;";
                        request.ContentType = "application/json";
                        request.Headers["Authorization"] = "Bearer " + authenticate.access_token;

                        string strResponse = "";
                        using (WebResponse webResponse = request.GetResponse())
                        {
                            using (StreamReader stream = new StreamReader(webResponse.GetResponseStream()))
                            {
                                strResponse = stream.ReadToEnd();
                            }
                        }
                        responses = JsonConvert.DeserializeObject<CatalogListViewModel>(strResponse);
                        item = responses.Items.FirstOrDefault();

                        if (item != null)
                        {
                            item.SKU = sku;
                            logger.LogInformation("GetProductCatalogDetail Job response from SC =>" + JsonConvert.SerializeObject(responses));
                            //productData.UpdateProductCatalog(item, sku);

                            item.IsEndOfLife = item.IsEndOfLife == true ? false : true;
                            item.PhysicalQty = item.PhysicalQty < 0 ? 0 : item.PhysicalQty;
                            var LowStock60 = Math.Round(((Convert.ToDecimal(item.QtySold60)) / Convert.ToDecimal(60)) * Convert.ToDecimal(60) - Convert.ToDecimal(item.PhysicalQty) - Convert.ToDecimal(item.OnOrder), 2);
                            decimal Velocity = Math.Round(Convert.ToDecimal(item.QtySold60) / Convert.ToDecimal(60), 2);
                            int CoverDays = 0;
                            if (Velocity != 0)
                            {
                                CoverDays = Convert.ToInt32(Math.Round((Convert.ToDecimal(item.AggregatePhysicalQty) + Convert.ToDecimal(item.OnOrder)) / Velocity));
                            }
                            try
                            {
                                using (MySqlConnection conn = new MySqlConnection(connStr))
                                {
                                    conn.Open();
                                    MySqlCommand cmd = new MySqlCommand("p_UpdateProductCatalogbySku", conn);
                                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("_LocationNotes", item.LocationNotes);
                                    cmd.Parameters.AddWithValue("_ShadowOf", item.ShadowOf);
                                    cmd.Parameters.AddWithValue("_QtySold15", item.QtySold15);
                                    cmd.Parameters.AddWithValue("_QtySold30", item.QtySold30);
                                    cmd.Parameters.AddWithValue("_QtySold60", item.QtySold60);
                                    cmd.Parameters.AddWithValue("_QtySold90", item.QtySold90);
                                    cmd.Parameters.AddWithValue("_QtySoldYTD", item.QtySoldYTD);
                                    cmd.Parameters.AddWithValue("_AggregatePhysicalQtyFBA", item.AggregatePhysicalQty);
                                    cmd.Parameters.AddWithValue("_AggregatedQty", item.AggregateQty);
                                    cmd.Parameters.AddWithValue("_PhysicalQty", item.PhysicalQty);
                                    cmd.Parameters.AddWithValue("_ReservedQty", item.ReservedQty);
                                    cmd.Parameters.AddWithValue("_LowStock60", LowStock60);
                                    cmd.Parameters.AddWithValue("_CoverDays", CoverDays);
                                    cmd.Parameters.AddWithValue("_OnOrder", item.OnOrder);
                                    cmd.Parameters.AddWithValue("_Continue", item.IsEndOfLife);
                                    cmd.Parameters.AddWithValue("_AggregateNonSellableQty", item.AggregateNonSellableQty);
                                    cmd.Parameters.AddWithValue("_ASINInActiveListing", item.ASINInActiveListing);
                                    cmd.Parameters.AddWithValue("_AmazonFBASKU", item.AmazonFBASKU);
                                    cmd.Parameters.AddWithValue("_SKU", sku);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            catch (Exception ex)
                            {
                                logger.LogInformation("GetProductCatalogDetail Job response from SC =>" + JsonConvert.SerializeObject(responses) + " Execption =>" + ex.ToString());
                                continue;
                            }

                        }
                    }
                    catch (Exception exp)
                    {
                        logger.LogInformation("GetProductCatalogDetail exeption on => " + "SKU " + sku + "Response From SC Catalog =>" + item + " =>Exeption =>" + exp);
                        continue;
                    }
                }
                logger.LogInformation("GetProductCatalogDetail Job Stopped At =>" + DateTime.Now.ToString());
            }
            await Task.CompletedTask;
        }

    }
}
