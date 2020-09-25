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
using OfficeOpenXml;
using HLD.WebApi.Enum;
using System.Data;

namespace HLD.WebApi.Jobs
{
    [DisallowConcurrentExecution]
    public class WarehouseProductQtyJob : IJob
    {
        IConnectionString _connectionString = null;
        ServiceReference1.AuthHeader authHeader = null;
        ProductWarehouseQtyDataAccess productWarehouseQtyDataAccess = null;
        ProductDataAccess productDataAccess = null;
        private readonly ILogger logger;
        ChannelDecrytionDataAccess channelDecrytionDataAccess = null;
        EncDecChannel _EncDecChannel = null;
        GetChannelCredViewModel _getChannelCredViewModel = null;

        string ftpUsername = "predict@homelivingdream.ca";
        string ftpPassword = "$9oTy3GY%!&WLoi";

        string ftp = "ftp://homelivingdream.ca";

        public WarehouseProductQtyJob(IConnectionString connectionString, ILogger<WarehouseProductQtyJob> _logger)
        {
            _connectionString = connectionString;

            productWarehouseQtyDataAccess = new ProductWarehouseQtyDataAccess(_connectionString);
            productDataAccess = new ProductDataAccess(_connectionString);
            _EncDecChannel = new EncDecChannel(_connectionString);
            channelDecrytionDataAccess = new ChannelDecrytionDataAccess(connectionString);
            logger = _logger;
        }
        //public async Task Execute(IJobExecutionContext context)
        //{
        //    int status = channelDecrytionDataAccess.CheckZincJobsStatus("WarehouseProductQtyJob");
        //    if (status == 1)
        //    {
        //        logger.LogInformation("WarehouseProductQtyJob Job Started At =>" + DateTime.Now.ToString());
        //        try
        //        {
        //            List<string> skuList = productDataAccess.GetProductDetailForWarehouseQtyUpdate_ALLSKU();
        //            foreach (var item in skuList)
        //            {

        //                GetWarahouseQty(item);
        //            }
        //            //GetWarahouseQty("D7-1988-18");

        //        }
        //        catch (Exception ex)
        //        {
        //            System.Diagnostics.Debug.WriteLine(ex.Message + " :");
        //        }
        //    }


        //    await Task.CompletedTask;
        //}
        public async Task Execute(IJobExecutionContext context)
        {
            int status = channelDecrytionDataAccess.CheckZincJobsStatus("WarehouseProductQtyJob");
            if (status == 1)
            {
                logger.LogInformation("WarehouseProductQtyJob Job Started At =>" + DateTime.Now.ToString());
                try
                {
                    ReadExcelFile("Predict WH Stock.xlsx");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message + " :");
                }
                logger.LogInformation("WarehouseProductQtyJob Job Stoped At =>" + DateTime.Now.ToString());
            }


            await Task.CompletedTask;
        }
        public void ReadExcelFile(string fileName)
        {
            logger.LogInformation("WarehouseProductQtyJob Job FileName=>" + fileName.ToString());
            try
            {

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create((new Uri(ftp + @"/" + fileName)));
                logger.LogInformation("WarehouseProductQtyJob Job => set request method to download file.");
                //set request method to download file. 
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                //request.UsePassive = false;
                request.UseBinary = true;
                request.KeepAlive = false;
                logger.LogInformation("WarehouseProductQtyJob Job => set up credentials");
                //set up credentials. 
                request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
                logger.LogInformation("WarehouseProductQtyJob Job => initialize Ftp response.");
                //initialize Ftp response.
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                logger.LogInformation("WarehouseProductQtyJob Job => open readers to read data from ftp");
                //open readers to read data from ftp 
                Stream responseStream = response.GetResponseStream();

                var memoryStream = new MemoryStream();
                responseStream.CopyTo(memoryStream);
                var fileBytes = memoryStream.ToArray();
                using (MemoryStream memStream = new MemoryStream(fileBytes))
                {
                    logger.LogInformation("WarehouseProductQtyJob Job => Converting to Excel Pakages");
                    ExcelPackage package = new ExcelPackage(memStream);
                    logger.LogInformation("WarehouseProductQtyJob Job => Generating WorkSheet");
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;
                    logger.LogInformation("WarehouseProductQtyJob Job => Reading columns headers");
                    string SKU = worksheet.Cells[1, 1].Value.ToString().Trim();
                    string DropShip_Canada = worksheet.Cells[1, 2].Value.ToString().Trim();
                    string DropShip_USA = worksheet.Cells[1, 3].Value.ToString().Trim();
                    string FBA_Canada = worksheet.Cells[1, 4].Value.ToString().Trim();
                    string FBA_USA = worksheet.Cells[1, 5].Value.ToString().Trim();
                    string HLD_CA1 = worksheet.Cells[1, 6].Value.ToString().Trim();
                    string HLD_CA2 = worksheet.Cells[1, 7].Value.ToString().Trim();
                    string HLD_CN1 = worksheet.Cells[1, 8].Value.ToString().Trim();
                    string HLD_Interim = worksheet.Cells[1, 9].Value.ToString().Trim();
                    string HLD_Tech1 = worksheet.Cells[1, 10].Value.ToString().Trim();
                    string Interim_FBA_CA = worksheet.Cells[1, 11].Value.ToString().Trim();
                    string Interim_FBA_USA = worksheet.Cells[1, 12].Value.ToString().Trim();
                    string NY_14305 = worksheet.Cells[1, 13].Value.ToString().Trim();
                    string Shipito = worksheet.Cells[1, 14].Value.ToString().Trim();
                    if (SKU == "SKU" && DropShip_Canada == "DropShip Canada" && DropShip_USA == "DropShip USA" && FBA_Canada == "FBA Canada" &&
                       FBA_USA == "FBA USA" && HLD_CA1 == "HLD-CA1")
                    {
                        for (int row = 2; row <= rowCount; row++)
                        {


                            try
                            {
                                if (worksheet.Cells[row, 1].Value != null && worksheet.Cells[row, 1].Value.ToString() != String.Empty)
                                {

                                    //List<ProductWarehouseQtyViewModel> list = new List<ProductWarehouseQtyViewModel>();
                                    ProductwareHousesViewModel viewModel = new ProductwareHousesViewModel();
                                    //string ShadowOfRow = worksheet.Cells[row, 1].Value == null ? "" : worksheet.Cells[row, 1].Value.ToString().Trim();
                                    viewModel.SKU = worksheet.Cells[row, 1].Value == null ? "" : worksheet.Cells[row, 1].Value.ToString().Trim();
                                    viewModel.DropShip_Canada = Convert.ToInt32(worksheet.Cells[row, 2].Value == null ? "" : worksheet.Cells[row, 2].Value.ToString().Trim());
                                    viewModel.DropShip_USA = Convert.ToInt32(worksheet.Cells[row, 3].Value == null ? "" : worksheet.Cells[row, 3].Value.ToString().Trim());
                                    viewModel.FBA_Canada = Convert.ToInt32(worksheet.Cells[row, 4].Value == null ? "" : worksheet.Cells[row, 4].Value.ToString().Trim());
                                    viewModel.FBA_USA = Convert.ToInt32(worksheet.Cells[row, 5].Value == null ? "" : worksheet.Cells[row, 5].Value.ToString().Trim());
                                    viewModel.HLD_CA1 = Convert.ToInt32(worksheet.Cells[row, 6].Value == null ? "" : worksheet.Cells[row, 6].Value.ToString().Trim());
                                    viewModel.HLD_CA2 = Convert.ToInt32(worksheet.Cells[row, 7].Value == null ? "" : worksheet.Cells[row, 7].Value.ToString().Trim());
                                    viewModel.HLD_CN1 = Convert.ToInt32(worksheet.Cells[row, 8].Value == null ? "" : worksheet.Cells[row, 8].Value.ToString().Trim());
                                    viewModel.HLD_Interim = Convert.ToInt32(worksheet.Cells[row, 9].Value == null ? "" : worksheet.Cells[row, 9].Value.ToString().Trim());
                                    viewModel.HLD_Tech1 = Convert.ToInt32(worksheet.Cells[row, 10].Value == null ? "" : worksheet.Cells[row, 10].Value.ToString().Trim());
                                    viewModel.Interim_FBA_CA = Convert.ToInt32(worksheet.Cells[row, 11].Value == null ? "" : worksheet.Cells[row, 11].Value.ToString().Trim());
                                    viewModel.Interim_FBA_USA = Convert.ToInt32(worksheet.Cells[row, 12].Value == null ? "" : worksheet.Cells[row, 12].Value.ToString().Trim());
                                    viewModel.NY_14305 = Convert.ToInt32(worksheet.Cells[row, 13].Value == null ? "" : worksheet.Cells[row, 13].Value.ToString().Trim());
                                    viewModel.Shipito = Convert.ToInt32(worksheet.Cells[row, 14].Value == null ? "" : worksheet.Cells[row, 14].Value.ToString().Trim());
                                    productWarehouseQtyDataAccess.SaveProductWareHouses(viewModel);
                                    logger.LogInformation("WarehouseProductQtyJob Job Updating ShadowOF=>" + viewModel.SKU.ToString() + " row no => " + row.ToString());
                                    var PhyQtyt = viewModel.FBA_Canada + viewModel.FBA_USA + viewModel.HLD_CA1 + viewModel.HLD_CA2 + viewModel.HLD_CN1 + viewModel.HLD_Interim + viewModel.HLD_Tech1 + viewModel.Interim_FBA_CA + viewModel.Interim_FBA_USA + viewModel.NY_14305 + viewModel.Shipito;
                                    productWarehouseQtyDataAccess.UpdateProductCatalogByWHSKU(PhyQtyt, viewModel.SKU);
                                }
                                else
                                {
                                    string ShadowOfSKURow = worksheet.Cells[row, 1].Value == null ? "" : worksheet.Cells[row, 1].Value.ToString().Trim();
                                    logger.LogInformation("WarehouseProductQtyJob Job ShadowOF=>" + ShadowOfSKURow.ToString());
                                }

                            }
                            catch (Exception exp)
                            {
                                logger.LogInformation("WarehouseProductQtyJob Job Execption=>" + exp.ToString());
                                continue;
                            }

                        }

                    }

                }
                response.Close();
            }
            catch (Exception ex)
            {
                logger.LogInformation("WarehouseProductQtyJob Job Exeption=>" + ex.ToString());
            }
        }

        public void GetWarahouseQty(string SKU)
        {

            string ApiURL = "";
            ApiURL = "https://lp.api.sellercloud.com/rest/api";
            AuthenticateSCRestViewModel authenticate = new AuthenticateSCRestViewModel();
            _getChannelCredViewModel = _EncDecChannel.DecryptedData("sellercloud");
            authenticate = _EncDecChannel.AuthenticateSCForIMportOrder(_getChannelCredViewModel, ApiURL);

            List<WareHouseProductQty> warehouselist = new List<WareHouseProductQty>();
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest
                    .Create(ApiURL + "/Inventory/Warehouses?productID=" + SKU);
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
                warehouselist = JsonConvert.DeserializeObject<List<WareHouseProductQty>>(strResponse);

                var status = productWarehouseQtyDataAccess.SaveWarehouseProductQty_New(warehouselist);
                var Item = warehouselist.Where(s => s.WarehouseID != 364 && s.WarehouseID != 365).ToList();
                logger.LogInformation("WarehouseProductQtyJob Job SKU =>" + SKU.ToString());
                var PhyQtyt = Item.Sum(s => s.PhysicalQty);
                productWarehouseQtyDataAccess.UpdateProductCatalogByWHSKU(PhyQtyt, SKU);
            }
            catch (Exception exp)
            {
                logger.LogInformation("WarehouseProductQtyJob Job Exceptions =>" + exp.ToString());
                System.Diagnostics.Debug.WriteLine(exp.Message + " :");
            }

        }

        //public void GetWarahouseQty_New(List<string> skuList)
        //{
        //    List<WareHouseViewModel> wareHouses = new List<WareHouseViewModel>();
        //    wareHouses = productWarehouseQtyDataAccess.GetWareHousesNamesList();

        //    int i = 0;
        //    string SKU = "IPXR-VJ-RD";
        //    //foreach (var SKU in skuList)
        //    {
        //        i++;
        //        logger.LogInformation("WarehouseProductQtyJob Job Updating SKU=>" + SKU.ToString() + "RecordesUpdating" + i.ToString());
        //        string ApiURL = "";
        //        ApiURL = "https://lp.api.sellercloud.com/rest/api";
        //        AuthenticateSCRestViewModel authenticate = new AuthenticateSCRestViewModel();
        //        _getChannelCredViewModel = _EncDecChannel.DecryptedData("sellercloud");
        //        authenticate = _EncDecChannel.AuthenticateSCForIMportOrder(_getChannelCredViewModel, ApiURL);

        //        List<WareHouseProductQty> warehouselist = new List<WareHouseProductQty>();
        //        try
        //        {
        //            HttpWebRequest request = (HttpWebRequest)WebRequest
        //                .Create(ApiURL + "/Inventory/Warehouses?productID=" + SKU);
        //            request.Method = "GET";
        //            request.Accept = "application/json;";
        //            request.ContentType = "application/json";
        //            request.Headers["Authorization"] = "Bearer " + authenticate.access_token;

        //            string strResponse = "";
        //            using (WebResponse webResponse = request.GetResponse())
        //            {
        //                using (StreamReader stream = new StreamReader(webResponse.GetResponseStream()))
        //                {
        //                    strResponse = stream.ReadToEnd();
        //                }
        //            }
        //            warehouselist = JsonConvert.DeserializeObject<List<WareHouseProductQty>>(strResponse);

        //            //var status = productWarehouseQtyDataAccess.SaveWarehouseProductQty_New(warehouselist, wareHouses);
        //            var Item = warehouselist.Where(s => s.WarehouseID != 364 && s.WarehouseID != 365).ToList();
        //            logger.LogInformation("WarehouseProductQtyJob Job SKU =>" + SKU.ToString());
        //            var PhyQtyt = Item.Sum(s => s.PhysicalQty);
        //            productWarehouseQtyDataAccess.UpdateProductCatalogByWHSKU(PhyQtyt, SKU);
        //        }
        //        catch (Exception exp)
        //        {
        //            logger.LogInformation("WarehouseProductQtyJob Job Exceptions =>" + exp.ToString());
        //            System.Diagnostics.Debug.WriteLine(exp.Message + " :");
        //            //      continue;
        //        }
        //    }
        //}


        //public void ReadExcelFile(string fileName)
        //{
        //    logger.LogInformation("WarehouseProductQtyJob Job FileName=>" + fileName.ToString());
        //    try
        //    {

        //        FtpWebRequest request = (FtpWebRequest)WebRequest.Create((new Uri(ftp + @"/" + fileName)));
        //        logger.LogInformation("WarehouseProductQtyJob Job => set request method to download file.");
        //        //set request method to download file. 
        //        request.Method = WebRequestMethods.Ftp.DownloadFile;
        //        //request.UsePassive = false;
        //        request.UseBinary = true;
        //        request.KeepAlive = false;
        //        logger.LogInformation("WarehouseProductQtyJob Job => set up credentials");
        //        //set up credentials. 
        //        request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
        //        logger.LogInformation("WarehouseProductQtyJob Job => initialize Ftp response.");
        //        //initialize Ftp response.
        //        FtpWebResponse response = (FtpWebResponse)request.GetResponse();
        //        logger.LogInformation("WarehouseProductQtyJob Job => open readers to read data from ftp");
        //        //open readers to read data from ftp 
        //        Stream responseStream = response.GetResponseStream();

        //        var memoryStream = new MemoryStream();
        //        responseStream.CopyTo(memoryStream);
        //        var fileBytes = memoryStream.ToArray();
        //        using (MemoryStream memStream = new MemoryStream(fileBytes))
        //        {
        //            logger.LogInformation("WarehouseProductQtyJob Job => Converting to Excel Pakages");
        //            ExcelPackage package = new ExcelPackage(memStream);
        //            logger.LogInformation("WarehouseProductQtyJob Job => Generating WorkSheet");
        //            var worksheet = package.Workbook.Worksheets[0];
        //            var rowCount = worksheet.Dimension.Rows;
        //            logger.LogInformation("WarehouseProductQtyJob Job => Reading columns headers");
        //            string SKU = worksheet.Cells[1, 1].Value.ToString().Trim();
        //            string DropShip_Canada = worksheet.Cells[1, 2].Value.ToString().Trim();
        //            string DropShip_USA = worksheet.Cells[1, 3].Value.ToString().Trim();
        //            string FBA_Canada = worksheet.Cells[1, 4].Value.ToString().Trim();
        //            string FBA_USA = worksheet.Cells[1, 5].Value.ToString().Trim();
        //            string HLD_CA1 = worksheet.Cells[1, 6].Value.ToString().Trim();
        //            string HLD_CA2 = worksheet.Cells[1, 7].Value.ToString().Trim();
        //            string HLD_CN1 = worksheet.Cells[1, 8].Value.ToString().Trim();
        //            string HLD_Interim = worksheet.Cells[1, 9].Value.ToString().Trim();
        //            string HLD_Tech1 = worksheet.Cells[1, 10].Value.ToString().Trim();
        //            string Interim_FBA_CA = worksheet.Cells[1, 11].Value.ToString().Trim();
        //            string Interim_FBA_USA = worksheet.Cells[1, 12].Value.ToString().Trim();
        //            string NY_14305 = worksheet.Cells[1, 13].Value.ToString().Trim();
        //            string Shipito = worksheet.Cells[1, 14].Value.ToString().Trim();
        //            if (SKU == "SKU" && DropShip_Canada == "DropShip Canada" && DropShip_USA == "DropShip USA" && FBA_Canada == "FBA Canada" &&
        //               FBA_USA == "FBA USA" && HLD_CA1 == "HLD-CA1")
        //            {
                        
        //                //DataTable dt = new DataTable();
        //                //dt.Columns.Add(new DataColumn("sku", typeof(System.String)));
        //                //dt.Columns.Add(new DataColumn("PhysicalQty", typeof(System.Int32)));
        //                //dt.Columns.Add(new DataColumn("WarehouseID", typeof(System.Int32)));
        //                //dt.Columns.Add(new DataColumn("name", typeof(System.String)));
        //                for (int row = 2; row <= rowCount; row++)
        //                {
        //                    List<ProductWarehouseQtyViewModel> list = new List<ProductWarehouseQtyViewModel>();

        //                    //if (row == 10)
        //                    //{
        //                    //    break;
        //                    //}

        //                    try
        //                    {
        //                        if (worksheet.Cells[row, 1].Value != null && worksheet.Cells[row, 1].Value.ToString() != String.Empty)
        //                        {

        //                            //List<ProductWarehouseQtyViewModel> list = new List<ProductWarehouseQtyViewModel>();

        //                            //string ShadowOfRow = worksheet.Cells[row, 1].Value == null ? "" : worksheet.Cells[row, 1].Value.ToString().Trim();
        //                            string SKUc = worksheet.Cells[row, 1].Value == null ? "" : worksheet.Cells[row, 1].Value.ToString().Trim();
        //                            int DropShip_Canadac = Convert.ToInt32(worksheet.Cells[row, 2].Value == null ? "" : worksheet.Cells[row, 2].Value.ToString().Trim());
        //                            {
        //                                ProductWarehouseQtyViewModel model = new ProductWarehouseQtyViewModel();
        //                                model.WarehouseID = (int)WarehouseName.DropShipCanada;
        //                                model.AvailableQty = DropShip_Canadac;
        //                                model.WarehouseName = "DropShip Canada";
        //                                model.ProductSku = SKUc;
        //                                list.Add(model);
        //                                //DataRow dr = dt.NewRow();
        //                                //dr["sku"] = SKUc;
        //                                //dr["PhysicalQty"] = DropShip_Canadac;
        //                                //dr["WarehouseID"] = (int)WarehouseName.DropShipCanada;
        //                                //dr["name"] = "DropShip Canada";
        //                                //dt.Rows.Add(dr);
        //                            }
        //                            int DropShip_USAc = Convert.ToInt32(worksheet.Cells[row, 3].Value == null ? "" : worksheet.Cells[row, 3].Value.ToString().Trim());
        //                            {
        //                                ProductWarehouseQtyViewModel model = new ProductWarehouseQtyViewModel();
        //                                model.WarehouseID = (int)WarehouseName.DropShipUSA;
        //                                model.WarehouseName = "DropShip USA";
        //                                model.AvailableQty = DropShip_USAc;
        //                                model.ProductSku = SKUc;
        //                                list.Add(model);

        //                                //DataRow dr = dt.NewRow();
        //                                //dr["sku"] = SKUc;
        //                                //dr["PhysicalQty"] = DropShip_USAc;
        //                                //dr["WarehouseID"] = (int)WarehouseName.DropShipUSA;
        //                                //dr["name"] = "DropShip USA";
        //                                //dt.Rows.Add(dr);
        //                            }
        //                            int FBA_Canadac = Convert.ToInt32(worksheet.Cells[row, 4].Value == null ? "" : worksheet.Cells[row, 4].Value.ToString().Trim());
        //                            {
        //                                ProductWarehouseQtyViewModel model = new ProductWarehouseQtyViewModel();
        //                                model.WarehouseID = (int)WarehouseName.FBACanada;
        //                                model.AvailableQty = FBA_Canadac;
        //                                model.WarehouseName = "FBA Canada";
        //                                model.ProductSku = SKUc;
        //                                list.Add(model);
        //                                //DataRow dr = dt.NewRow();
        //                                //dr["sku"] = SKUc;
        //                                //dr["PhysicalQty"] = FBA_Canadac;
        //                                //dr["WarehouseID"] = (int)WarehouseName.FBACanada;
        //                                //dr["name"] = "FBA Canada";
        //                                //dt.Rows.Add(dr);
        //                            }
        //                            int FBA_USAc = Convert.ToInt32(worksheet.Cells[row, 5].Value == null ? "" : worksheet.Cells[row, 5].Value.ToString().Trim());
        //                            {
        //                                ProductWarehouseQtyViewModel model = new ProductWarehouseQtyViewModel();
        //                                model.WarehouseID = (int)WarehouseName.FBAUSA;
        //                                model.AvailableQty = FBA_USAc;
        //                                model.WarehouseName = "FBA USA";
        //                                model.ProductSku = SKUc;
        //                                list.Add(model);

        //                                //DataRow dr = dt.NewRow();
        //                                //dr["sku"] = SKUc;
        //                                //dr["PhysicalQty"] = FBA_USAc;
        //                                //dr["WarehouseID"] = (int)WarehouseName.FBAUSA;
        //                                //dr["name"] = "FBA USA";
        //                                //dt.Rows.Add(dr);
        //                            }
        //                            int HLD_CA1c = Convert.ToInt32(worksheet.Cells[row, 6].Value == null ? "" : worksheet.Cells[row, 6].Value.ToString().Trim());
        //                            {
        //                                ProductWarehouseQtyViewModel model = new ProductWarehouseQtyViewModel();
        //                                model.WarehouseID = (int)WarehouseName.HLDCA1;
        //                                model.AvailableQty = HLD_CA1c;
        //                                model.WarehouseName = "HLD-CA1";
        //                                model.ProductSku = SKUc;
        //                                list.Add(model);

        //                                //DataRow dr = dt.NewRow();
        //                                //dr["sku"] = SKUc;
        //                                //dr["PhysicalQty"] = HLD_CA1c;
        //                                //dr["WarehouseID"] = (int)WarehouseName.HLDCA1;
        //                                //dr["name"] = "HLD-CA1";
        //                                //dt.Rows.Add(dr);
        //                            }
        //                            int HLD_CA2c = Convert.ToInt32(worksheet.Cells[row, 7].Value == null ? "" : worksheet.Cells[row, 7].Value.ToString().Trim());
        //                            {
        //                                ProductWarehouseQtyViewModel model = new ProductWarehouseQtyViewModel();
        //                                model.WarehouseID = (int)WarehouseName.HLDCA2;
        //                                model.AvailableQty = HLD_CA2c;
        //                                model.WarehouseName = "HLD-CA2";
        //                                model.ProductSku = SKUc;
        //                                list.Add(model);

        //                                //DataRow dr = dt.NewRow();
        //                                //dr["sku"] = SKUc;
        //                                //dr["PhysicalQty"] = HLD_CA2c;
        //                                //dr["WarehouseID"] = (int)WarehouseName.HLDCA2;
        //                                //dr["name"] = "HLD-CA2";
        //                                //dt.Rows.Add(dr);
        //                            }
        //                            int HLD_CN1c = Convert.ToInt32(worksheet.Cells[row, 8].Value == null ? "" : worksheet.Cells[row, 8].Value.ToString().Trim());
        //                            {
        //                                ProductWarehouseQtyViewModel model = new ProductWarehouseQtyViewModel();
        //                                model.WarehouseID = (int)WarehouseName.HLDCN1;
        //                                model.AvailableQty = HLD_CN1c;
        //                                model.WarehouseName = "HLD-CN1";
        //                                model.ProductSku = SKUc;
        //                                list.Add(model);

        //                                //DataRow dr = dt.NewRow();
        //                                //dr["sku"] = SKUc;
        //                                //dr["PhysicalQty"] = HLD_CN1c;
        //                                //dr["WarehouseID"] = (int)WarehouseName.HLDCN1;
        //                                //dr["name"] = "HLD-CN1";
        //                                //dt.Rows.Add(dr);
        //                            }
        //                            int HLD_Interimc = Convert.ToInt32(worksheet.Cells[row, 9].Value == null ? "" : worksheet.Cells[row, 9].Value.ToString().Trim());
        //                            {
        //                                ProductWarehouseQtyViewModel model = new ProductWarehouseQtyViewModel();
        //                                model.WarehouseID = (int)WarehouseName.HLDInterim;
        //                                model.AvailableQty = HLD_Interimc;
        //                                model.WarehouseName = "HLD-Interim";
        //                                model.ProductSku = SKUc;
        //                                list.Add(model);

        //                                //DataRow dr = dt.NewRow();
        //                                //dr["sku"] = SKUc;
        //                                //dr["PhysicalQty"] = HLD_Interimc;
        //                                //dr["WarehouseID"] = (int)WarehouseName.HLDInterim;
        //                                //dr["name"] = "HLD-Interim";
        //                                //dt.Rows.Add(dr);
        //                            }
        //                            int HLD_Tech1c = Convert.ToInt32(worksheet.Cells[row, 10].Value == null ? "" : worksheet.Cells[row, 10].Value.ToString().Trim());
        //                            {
        //                                ProductWarehouseQtyViewModel model = new ProductWarehouseQtyViewModel();
        //                                model.WarehouseID = (int)WarehouseName.HLDTech1;
        //                                model.AvailableQty = HLD_Tech1c;
        //                                model.WarehouseName = "HLD-Tech1";
        //                                model.ProductSku = SKUc;
        //                                list.Add(model);

        //                                //DataRow dr = dt.NewRow();
        //                                //dr["sku"] = SKUc;
        //                                //dr["PhysicalQty"] = HLD_Tech1c;
        //                                //dr["WarehouseID"] = (int)WarehouseName.HLDTech1;
        //                                //dr["name"] = "HLD-Tech1";
        //                                //dt.Rows.Add(dr);
        //                            }
        //                            int Interim_FBA_CAc = Convert.ToInt32(worksheet.Cells[row, 11].Value == null ? "" : worksheet.Cells[row, 11].Value.ToString().Trim());
        //                            {
        //                                ProductWarehouseQtyViewModel model = new ProductWarehouseQtyViewModel();
        //                                model.WarehouseID = (int)WarehouseName.InterimFBACA;
        //                                model.AvailableQty = Interim_FBA_CAc;
        //                                model.WarehouseName = "Interim FBA CA";
        //                                model.ProductSku = SKUc;

        //                                list.Add(model);

        //                                //DataRow dr = dt.NewRow();
        //                                //dr["sku"] = SKUc;
        //                                //dr["PhysicalQty"] = Interim_FBA_CAc;
        //                                //dr["WarehouseID"] = (int)WarehouseName.InterimFBACA;
        //                                //dr["name"] = "Interim FBA CA";
        //                                //dt.Rows.Add(dr);
        //                            }
        //                            int Interim_FBA_USAc = Convert.ToInt32(worksheet.Cells[row, 12].Value == null ? "" : worksheet.Cells[row, 12].Value.ToString().Trim());
        //                            {
        //                                ProductWarehouseQtyViewModel model = new ProductWarehouseQtyViewModel();
        //                                model.WarehouseID = (int)WarehouseName.InterimFBAUSA;
        //                                model.AvailableQty = Interim_FBA_USAc;
        //                                model.WarehouseName = "Interim FBA USA";
        //                                model.ProductSku = SKUc;
        //                                list.Add(model);

        //                                //DataRow dr = dt.NewRow();
        //                                //dr["sku"] = SKUc;
        //                                //dr["PhysicalQty"] = Interim_FBA_USAc;
        //                                //dr["WarehouseID"] = (int)WarehouseName.InterimFBAUSA;
        //                                //dr["name"] = "Interim FBA USA";
        //                                //dt.Rows.Add(dr);
        //                            }
        //                            int NY_14305c = Convert.ToInt32(worksheet.Cells[row, 13].Value == null ? "" : worksheet.Cells[row, 13].Value.ToString().Trim());
        //                            {
        //                                ProductWarehouseQtyViewModel model = new ProductWarehouseQtyViewModel();
        //                                model.WarehouseID = (int)WarehouseName.NY14305;
        //                                model.WarehouseName = "NY-14305";
        //                                model.AvailableQty = NY_14305c;
        //                                model.ProductSku = SKUc;
        //                                list.Add(model);

        //                                //DataRow dr = dt.NewRow();
        //                                //dr["sku"] = SKUc;
        //                                //dr["PhysicalQty"] = NY_14305c;
        //                                //dr["WarehouseID"] = (int)WarehouseName.NY14305;
        //                                //dr["name"] = "NY-14305";
        //                                //dt.Rows.Add(dr);
        //                            }
        //                            int Shipitoc = Convert.ToInt32(worksheet.Cells[row, 14].Value == null ? "" : worksheet.Cells[row, 14].Value.ToString().Trim());
        //                            {
        //                                ProductWarehouseQtyViewModel model = new ProductWarehouseQtyViewModel();
        //                                model.WarehouseID = (int)WarehouseName.Shipito;
        //                                model.AvailableQty = Shipitoc;
        //                                model.WarehouseName = "Shipito";
        //                                model.ProductSku = SKUc;
        //                                list.Add(model);

        //                                //DataRow dr = dt.NewRow();
        //                                //dr["sku"] = SKUc;
        //                                //dr["PhysicalQty"] = Shipitoc;
        //                                //dr["WarehouseID"] = (int)WarehouseName.Shipito;
        //                                //dr["name"] = "Shipito";
        //                                //dt.Rows.Add(dr);
        //                            }
        //                            var status = productWarehouseQtyDataAccess.SaveWarehouseProductQty_New4(list);
        //                            // var status = productWarehouseQtyDataAccess.SaveWarehouseProductQty_New4(list);
        //                            var Item = list.Where(s => s.WarehouseID != 1 && s.WarehouseID != 2).ToList();

        //                            var PhyQtyt = Item.Sum(s => s.AvailableQty);
        //                            productWarehouseQtyDataAccess.UpdateProductCatalogByWHSKU(PhyQtyt, SKU);
        //                            logger.LogInformation("WarehouseProductQtyJob Job Updating ShadowOF=>" + SKUc + " row no => " + row.ToString());
        //                        }
        //                        else
        //                        {
        //                            string ShadowOfSKURow = worksheet.Cells[row, 1].Value == null ? "" : worksheet.Cells[row, 1].Value.ToString().Trim();
        //                            logger.LogInformation("WarehouseProductQtyJob Job ShadowOF=>" + ShadowOfSKURow.ToString());
        //                        }

        //                    }
        //                    catch (Exception exp)
        //                    {
        //                        logger.LogInformation("WarehouseProductQtyJob Job ShadowOF=>" + exp.ToString());
        //                        continue;
        //                    }

        //                }
                        
        //            }

        //        }
        //        response.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogInformation("WarehouseProductQtyJob Job ShadowOF=>" + ex.ToString());
        //    }
        //}
    }
}
