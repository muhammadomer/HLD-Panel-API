using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using AutoMapper.Configuration;
using DataAccess.DataAccess;
using DataAccess.Helper;
using DataAccess.ViewModels;
using ImageMagick;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using Quartz;
using ServiceReference1;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;


namespace HLD.WebApi.Jobs
{
    [DisallowConcurrentExecution]
    public class S3FileReadingJob : IJob
    {
        IConnectionString _connectionString = null;
        private static string _bucketSubdirectory = String.Empty;
        private string _ImagesbucketName = "upload.hld.erp.images";//this is my Amazon Bucket name
        private string _ImagesbucketNameCompressed = "upload.hld.erp.images.thumbnail";//this is my compressed Amazon Bucket name
        UploadFilesToS3DataAccess UploadFilesToS3 = null;
        OrderRelationDataAccess orderRelationDataAccess = null;
        EncDecChannel _EncDecChannel = null;
        string ApiURL = null;
        private readonly ILogger logger;
        SellerCloudOrderDataAccess _sellerCloudOrderDataAccess = null;
        BestBuyOrderDataAccess _bestBuyOrderDataAccess = null;
        BestBuyProductDataAccess _bestBuyProductDataAccess = null;
        GetChannelCredViewModel _getChannelCredViewModel = null;
        ProductDataAccess _ProductDataAccess = null;
        ProductWarehouseQtyDataAccess ProductWHQtyDataAccess;
        SellerCloudOrderDataAccess cloudOrderDataAccess;
        // Specify your bucket region (an example region is shown).
        ServiceReference1.AuthHeader authHeader = null;
        private AmazonS3Client _s3Client = new AmazonS3Client(RegionEndpoint.USEast2);

        public S3FileReadingJob(IConnectionString connectionString, ILogger<S3FileReadingJob> _logger)
        {
            _connectionString = connectionString;
            _s3Client = new AmazonS3Client(RegionEndpoint.USEast2);
            _sellerCloudOrderDataAccess = new SellerCloudOrderDataAccess(_connectionString);
            _bestBuyOrderDataAccess = new BestBuyOrderDataAccess(_connectionString);
            _bestBuyProductDataAccess = new BestBuyProductDataAccess(_connectionString);
            UploadFilesToS3 = new UploadFilesToS3DataAccess(_connectionString);
            orderRelationDataAccess = new OrderRelationDataAccess(_connectionString);

            _ProductDataAccess = new ProductDataAccess(_connectionString);
            ProductWHQtyDataAccess = new ProductWarehouseQtyDataAccess(_connectionString);
            cloudOrderDataAccess = new SellerCloudOrderDataAccess(_connectionString);
            ApiURL = "https://lp.api.sellercloud.com/rest/api";
            _EncDecChannel = new EncDecChannel(_connectionString);
            logger = _logger;
        }
        public async Task Execute(IJobExecutionContext context)

        {
            List<GetFileJobsToRunViewModel> getFileJobs = new List<GetFileJobsToRunViewModel>();
            getFileJobs = UploadFilesToS3.GetFileJobsToRun();

            foreach (var item in getFileJobs)
            {
                bool status = UploadFilesToS3.UpdateFileJobsASRunning(item.Job_Id);
                if (status == true)
                {
                    RunSpecificJob(item).Wait();
                }
                UploadFilesToS3.UpdateFileJobsASNotRunning(item.Job_Id);


            }


            await Task.CompletedTask;
        }


        public async Task RunSpecificJob(GetFileJobsToRunViewModel jobdetails)
        {
            try
            {
                if (jobdetails.Job_Type.Equals("skuasin_am_mxpr_dr_qty_comments"))
                {


                    string responseBody = "";
                    try
                    {
                        GetObjectRequest request = new GetObjectRequest
                        {
                            BucketName = jobdetails.File_Bucket,
                            Key = jobdetails.File_Name
                        };
                        using (GetObjectResponse response = await _s3Client.GetObjectAsync(request))
                        using (Stream responseStream = response.ResponseStream)
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            // using (MemoryStream stream = new MemoryStream(bin))
                            using (ExcelPackage excelPackage = new ExcelPackage(responseStream))
                            {
                                List<UpdateAsinSkuDropShipDataJobViewModel> SkuAsinmodel = new List<UpdateAsinSkuDropShipDataJobViewModel>();

                                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0];
                                var rowCount = worksheet.Dimension.Rows;

                                string SKU = worksheet.Cells[1, 1].Value.ToString().Trim();
                                string ASIN = worksheet.Cells[1, 2].Value.ToString().Trim();
                                string AmzPrice = worksheet.Cells[1, 3].Value.ToString().Trim();
                                string MaxPrice = worksheet.Cells[1, 4].Value.ToString().Trim();
                                string AvgCost = worksheet.Cells[1, 5].Value.ToString().Trim();
                                string DropShip = worksheet.Cells[1, 6].Value.ToString().Trim();
                                string DropShipQty = worksheet.Cells[1, 7].Value.ToString().Trim();
                                string DropShipComments = worksheet.Cells[1, 8].Value.ToString().Trim();


                                if (ASIN == "ASIN" && SKU == "SKU" && MaxPrice == "MAX_PRICE" &&
                                    AmzPrice == "AMZ_PRICE" && DropShip == "DropShip" &&
                                    DropShipQty == "DropShipQty" && DropShipComments == "DropShipComments" && AvgCost == "AvgCostCAD")
                                {
                                    for (int row = 2; row <= rowCount; row++)
                                    {
                                        if (worksheet.Cells[row, 1].Value != null && worksheet.Cells[row, 1].Value.ToString() != String.Empty)
                                        {

                                            SkuAsinmodel.Add(new UpdateAsinSkuDropShipDataJobViewModel
                                            {

                                                SKU = worksheet.Cells[row, 1].Value == null ? "" : worksheet.Cells[row, 1].Value.ToString().Trim(),
                                                ASIN = worksheet.Cells[row, 2].Value == null ? "" : worksheet.Cells[row, 2].Value.ToString().Trim(),
                                                AmzPrice = worksheet.Cells[row, 3].Value == null ? "" : worksheet.Cells[row, 3].Value.ToString().Trim(),
                                                MAXPrice = worksheet.Cells[row, 4].Value == null ? "" : worksheet.Cells[row, 4].Value.ToString().Trim(),
                                                AvgCost = worksheet.Cells[row, 5].Value == null ? "" : worksheet.Cells[row, 5].Value.ToString().Trim(),
                                                DropShip = worksheet.Cells[row, 6].Value == null ? "" : worksheet.Cells[row, 6].Value.ToString().Trim(),
                                                DropShipQty = worksheet.Cells[row, 7].Value == null ? "" : worksheet.Cells[row, 7].Value.ToString().Trim(),
                                                DropShipComments = worksheet.Cells[row, 8].Value == null ? "" : worksheet.Cells[row, 8].Value.ToString().Trim(),

                                                RowNumber = row.ToString(),
                                            });
                                        }
                                    }

                                    bool st = UploadFilesToS3.InsertS3FileDataOfSkuAsinDropship(jobdetails.Job_Id, SkuAsinmodel);
                                    // bool st = true;
                                    if (st == true)
                                    {
                                        UploadFilesToS3.UpdateFileJobsASCompleted(jobdetails.Job_Id);
                                    }

                                }
                            }

                            responseBody = reader.ReadToEnd(); // Now you process the response body.
                        }
                    }
                    catch (AmazonS3Exception e)
                    {
                        Console.WriteLine("Error encountered ***. Message:'{0}' when writing an object", e.Message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
                    }




                }

                if (jobdetails.Job_Type.Equals("ImportChildOrderSC"))
                {
                    try
                    {
                        logger.LogInformation("ImportChildOrderSC Job Started At =>" + DateTime.Now.ToString());
                        int Success = 0;
                        int error = 0;
                        List<GetChildORderToimportJobViewModel> viewModels = new List<GetChildORderToimportJobViewModel>();
                        AuthenticateSCRestViewModel authenticate = new AuthenticateSCRestViewModel();
                        // get Order of Related Jobs
                        logger.LogInformation("ImportChildOrderSC Job GetChildOrderToImport");
                        viewModels = orderRelationDataAccess.GetChildOrderToImport(jobdetails.Job_Id);
                        logger.LogInformation("ImportChildOrderSC Job list of ChildOrderToImport=>" + JsonConvert.SerializeObject(viewModels));
                        _getChannelCredViewModel = new GetChannelCredViewModel();
                        logger.LogInformation("ImportChildOrderSC Job Get Credentials");
                        _getChannelCredViewModel = _EncDecChannel.DecryptedData("sellercloud");
                        // Get Taken Fro SC
                        authenticate = _EncDecChannel.AuthenticateSCForIMportOrder(_getChannelCredViewModel, ApiURL);
                        foreach (var itemorder in viewModels)
                        {
                            logger.LogInformation("ImportChildOrderSC Job Order to import =>" + JsonConvert.SerializeObject(itemorder));
                            OrderRelationViewModel orderRelationViewModel = new OrderRelationViewModel();
                            //Get relation from SC 
                            orderRelationViewModel = GetOrdersFromSCRest(itemorder.SC_ChildID, authenticate.access_token);

                            logger.LogInformation("ImportChildOrderSC Job GetOrdersFromSCRest =>" + JsonConvert.SerializeObject(orderRelationViewModel));

                            if (orderRelationViewModel != null)
                            {
                                bool status = await GetChildOrdersFromSellerCloud(orderRelationViewModel, _getChannelCredViewModel);
                                // Set Jobs AS Logs
                                logger.LogInformation("ImportChildOrderSC Job Set Jobs AS Logs");
                                if (status == true)
                                {
                                    Success++;
                                    UploadFilesToS3.InsertJobLog(Success, error, "", 0, "", jobdetails.Job_Id);
                                }
                                else
                                {
                                    logger.LogInformation("ImportChildOrderSC Job Set Error In Order ID => " + jobdetails.Job_Id.ToString() + "Order" + JsonConvert.SerializeObject(itemorder));
                                    error++;
                                    UploadFilesToS3.InsertJobLog(Success, error, itemorder.SC_ChildID.ToString(), 0, "Error In Order", jobdetails.Job_Id);
                                }
                            }
                            else
                            {
                                logger.LogInformation("ImportChildOrderSC Job Set Error In Order ID => " + jobdetails.Job_Id.ToString() + "Order" + JsonConvert.SerializeObject(itemorder));
                                error++;
                                UploadFilesToS3.InsertJobLog(Success, error, itemorder.SC_ChildID.ToString(), 0, "No Related Order Found", jobdetails.Job_Id);

                            }


                        }
                        logger.LogInformation("ImportChildOrderSC Job => Job Completed ");
                        UploadFilesToS3.UpdateFileJobsASCompleted(jobdetails.Job_Id);
                        orderRelationDataAccess.UpdateOrderOrderRelationAsImported(jobdetails.Job_Id);
                    }
                    catch(Exception exp)
                    {
                        logger.LogInformation("ImportChildOrderSC Job Set Error In Order ID => " + jobdetails.Job_Id.ToString() + "Error => "+exp.Message );
                    }
                }

                if (jobdetails.Job_Type.Equals("DS_QTY_COMMENTS"))
                {

                    string responseBody = "";
                    try
                    {
                        GetObjectRequest request = new GetObjectRequest
                        {
                            BucketName = jobdetails.File_Bucket,
                            Key = jobdetails.File_Name
                        };
                        using (GetObjectResponse response = await _s3Client.GetObjectAsync(request))
                        using (Stream responseStream = response.ResponseStream)
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            // using (MemoryStream stream = new MemoryStream(bin))
                            using (ExcelPackage excelPackage = new ExcelPackage(responseStream))
                            {
                                List<UpdateQtyCommentsByJobViewModel> SkuDSQtymodel = new List<UpdateQtyCommentsByJobViewModel>();

                                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0];
                                var rowCount = worksheet.Dimension.Rows;

                                string SKU = worksheet.Cells[1, 1].Value.ToString().Trim();
                                string DropShip = worksheet.Cells[1, 2].Value.ToString().Trim();
                                string DropShipQty = worksheet.Cells[1, 3].Value.ToString().Trim();
                                string DropShipComments = worksheet.Cells[1, 4].Value.ToString().Trim();


                                if (SKU == "SKU" && DropShip == "DropShip" &&
                                    DropShipQty == "DropShipQty" && DropShipComments == "DropShipComments")
                                {
                                    for (int row = 2; row <= rowCount; row++)
                                    {
                                        if (worksheet.Cells[row, 1].Value != null && worksheet.Cells[row, 1].Value.ToString() != String.Empty)
                                        {

                                            SkuDSQtymodel.Add(new UpdateQtyCommentsByJobViewModel
                                            {

                                                ShopSKU_OfferSKU = worksheet.Cells[row, 1].Value == null ? "" : worksheet.Cells[row, 1].Value.ToString().Trim(),

                                                dropship_status = worksheet.Cells[row, 2].Value == null ? "" : worksheet.Cells[row, 2].Value.ToString().Trim(),
                                                dropship_Qty = worksheet.Cells[row, 3].Value == null ? "" : worksheet.Cells[row, 3].Value.ToString().Trim(),
                                                DropshipComments = worksheet.Cells[row, 4].Value == null ? "" : worksheet.Cells[row, 4].Value.ToString().Trim(),

                                                RowNumber = row.ToString(),

                                            });
                                        }


                                    }

                                    bool st = UploadFilesToS3.InsertS3BestBuySKUQtyComments(jobdetails.Job_Id, SkuDSQtymodel);
                                    // bool st = true;
                                    if (st == true)
                                    {
                                        UploadFilesToS3.UpdateFileJobsASCompleted(jobdetails.Job_Id);
                                    }

                                }
                            }

                            responseBody = reader.ReadToEnd(); // Now you process the response body.
                        }
                    }
                    catch (AmazonS3Exception e)
                    {
                        Console.WriteLine("Error encountered ***. Message:'{0}' when writing an object", e.Message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
                    }


                }

                if (jobdetails.Job_Type.Equals("Import missing sku from seller-cloud"))
                {
                    string responseBody = "";
                    try
                    {
                        GetObjectRequest request = new GetObjectRequest
                        {
                            BucketName = jobdetails.File_Bucket,
                            Key = jobdetails.File_Name
                        };
                        using (GetObjectResponse response = await _s3Client.GetObjectAsync(request))
                        using (Stream responseStream = response.ResponseStream)
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            // using (MemoryStream stream = new MemoryStream(bin))
                            using (ExcelPackage excelPackage = new ExcelPackage(responseStream))
                            {
                                List<ImportMissingSkuViewModel> SkuMissingmodel = new List<ImportMissingSkuViewModel>();

                                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0];
                                var rowCount = worksheet.Dimension.Rows;

                                string SKU = worksheet.Cells[1, 1].Value.ToString().Trim();


                                if (SKU == "SKU")
                                {
                                    for (int row = 2; row <= rowCount; row++)
                                    {
                                        if (worksheet.Cells[row, 1].Value != null && worksheet.Cells[row, 1].Value.ToString() != String.Empty)
                                        {

                                            SkuMissingmodel.Add(new ImportMissingSkuViewModel
                                            {

                                                Product_Sku = worksheet.Cells[row, 1].Value == null ? "" : worksheet.Cells[row, 1].Value.ToString().Trim(),


                                                RowNumber = row.ToString(),
                                            });
                                        }


                                    }
                                    // do here
                                    //  bool st = UploadFilesToS3.InsertS3BestBuyMIssingSKUFromSellerCloud(jobdetails.Job_Id, SkuMissingmodel);
                                    int Success = 0;
                                    int Fail = 0;

                                    foreach (var missingSku in SkuMissingmodel)
                                    {
                                        if (!String.IsNullOrEmpty(missingSku.Product_Sku))
                                        {


                                            int productID = _ProductDataAccess.GetProductIdBySKU(missingSku.Product_Sku);
                                            if (productID > 0)
                                            {
                                                Fail++;
                                                UploadFilesToS3.InsertJobLog(Success, Fail, missingSku.Product_Sku, Convert.ToInt32(missingSku.RowNumber), "SKU already exist.", jobdetails.Job_Id);
                                            }
                                            else
                                            {
                                                var product = _ProductDataAccess.GetProductInfoFromSellerCloudForMIssingSku(missingSku.Product_Sku);
                                                ProductInsertUpdateViewModel productInsertUpdate = new ProductInsertUpdateViewModel();
                                               // productInsertUpdate = await GetProductInfoFromSellerCloudForMIssingSku(missingSku.Product_Sku);
                                                if (product == null)
                                                {
                                                    
                                                    Fail++;
                                                    UploadFilesToS3.InsertJobLog(Success, Fail, missingSku.Product_Sku, Convert.ToInt32(missingSku.RowNumber), "Some error occurred.", jobdetails.Job_Id);
                                                }
                                                else
                                                {
                                                    product.SKU = missingSku.Product_Sku;
                                                    _ProductDataAccess.SaveProductnew(product);
                                                    ProductWHQtyDataAccess.GetWarahouseQty(product.SKU);

                                                    var imageURL = product.ImageUrl;
                                                    if (imageURL != null && imageURL != "")
                                                    {
                                                        Image img = DownloadImageFromUrl(imageURL);
                                                        // save seller cloud order images to prorduct_images table
                                                        ImagesSaveToDatabaseWithURLViewMOdel databaseImagesURL = new ImagesSaveToDatabaseWithURLViewMOdel();
                                                        try
                                                        {
                                                            if (img != null)
                                                            {

                                                                string fileName = Guid.NewGuid().ToString() + "-" + product.SKU.Trim() + Path.GetExtension(imageURL);
                                                                databaseImagesURL.product_Sku = product.SKU.Trim();
                                                                databaseImagesURL.FileName = fileName;
                                                                databaseImagesURL.ImageURL = imageURL;
                                                                databaseImagesURL.isImageExistInSC = true;
                                                                if (cloudOrderDataAccess.SaveProductImagesFromSellerCloudOrders(databaseImagesURL))
                                                                {
                                                                    await uploadToS3(GetStream(img, ImageFormat.Jpeg), fileName);

                                                                    await uploadCompressedToS3(GetStreamOfReducedImage(img), fileName);
                                                                }
                                                                //ProductapiAccess.UpdateSCImageStatusInProductTable(ApiURL, token, sku.Trim(), true);
                                                                //_successCounter++;
                                                            }

                                                        }
                                                        catch (Exception ex)
                                                        {
                                                        }
                                                    }

                                                    Success++;
                                                    UploadFilesToS3.InsertJobLog(Success, Fail, "", 0, "", jobdetails.Job_Id);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Fail++;
                                            UploadFilesToS3.InsertJobLog(Success, Fail, missingSku.Product_Sku, Convert.ToInt32(missingSku.RowNumber), " Contain invalid data in file.", jobdetails.Job_Id);
                                        }
                                    }

                                    //foreach (var missingSku in SkuMissingmodel)
                                    //{
                                    //    if (!String.IsNullOrEmpty(missingSku.Product_Sku))

                                    //    {

                                    //        int productID = _ProductDataAccess.GetProductIdBySKU(missingSku.Product_Sku);
                                    //        if (productID > 0)
                                    //        {
                                    //            Fail++;
                                    //            UploadFilesToS3.InsertJobLog(Success, Fail, missingSku.Product_Sku, Convert.ToInt32(missingSku.RowNumber), "SKU already exist.", jobdetails.Job_Id);


                                    //        }
                                    //        else
                                    //        {
                                    //            ProductInsertUpdateViewModel productInsertUpdate = new ProductInsertUpdateViewModel();
                                    //            productInsertUpdate = await GetProductInfoFromSellerCloudForMIssingSku(missingSku.Product_Sku);
                                    //            if (productInsertUpdate.ProductSKU == null)
                                    //            {
                                    //                Fail++;
                                    //                UploadFilesToS3.InsertJobLog(Success, Fail, missingSku.Product_Sku, Convert.ToInt32(missingSku.RowNumber), "Some error occurred.", jobdetails.Job_Id);
                                    //            }
                                    //            else
                                    //            {
                                    //                _ProductDataAccess.SaveProduct(productInsertUpdate);
                                    //                Success++;
                                    //                UploadFilesToS3.InsertJobLog(Success, Fail, "", 0, "", jobdetails.Job_Id);
                                    //            }



                                    //        }


                                    //    }


                                    //    else
                                    //    {

                                    //        Fail++;
                                    //        UploadFilesToS3.InsertJobLog(Success, Fail, missingSku.Product_Sku, Convert.ToInt32(missingSku.RowNumber), " Contain invalid data in file.", jobdetails.Job_Id);

                                    //    }

                                    //}
                                    // bool st = true;

                                    UploadFilesToS3.UpdateFileJobsASCompleted(jobdetails.Job_Id);


                                }
                            }

                            responseBody = reader.ReadToEnd(); // Now you process the response body.
                        }
                    }
                    catch (AmazonS3Exception e)
                    {
                        Console.WriteLine("Error encountered ***. Message:'{0}' when writing an object", e.Message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
                    }

                }

                if (jobdetails.Job_Type.Equals("ApprovedPriceUpdationJob"))
                {

                    string responseBody = "";
                    try
                    {
                        GetObjectRequest request = new GetObjectRequest
                        {
                            BucketName = jobdetails.File_Bucket,
                            Key = jobdetails.File_Name
                        };
                        using (GetObjectResponse response = await _s3Client.GetObjectAsync(request))
                        using (Stream responseStream = response.ResponseStream)
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            // using (MemoryStream stream = new MemoryStream(bin))
                            using (ExcelPackage excelPackage = new ExcelPackage(responseStream))
                            {
                                List<SaveApprovedPricesViewModel> SkuPricemodel = new List<SaveApprovedPricesViewModel>();

                                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0];
                                var rowCount = worksheet.Dimension.Rows;

                                string Vendor = worksheet.Cells[1, 1].Value.ToString().Trim();
                                string SKU = worksheet.Cells[1, 2].Value.ToString().Trim();
                                string ApprovedUnitPrice = worksheet.Cells[1, 3].Value.ToString().Trim();
                                string Currency = worksheet.Cells[1, 4].Value.ToString().Trim();


                                if (Vendor == "Vendor" && SKU == "SKU" &&
                                    ApprovedUnitPrice.Replace("\n", String.Empty).Replace(" ", "") == "ApprovedUnitPrice" && Currency == "Currency")
                                {
                                    for (int row = 2; row <= rowCount; row++)
                                    {
                                        if (worksheet.Cells[row, 1].Value != null && worksheet.Cells[row, 1].Value.ToString() != String.Empty)
                                        {

                                            SkuPricemodel.Add(new SaveApprovedPricesViewModel
                                            {

                                                Vendor = worksheet.Cells[row, 1].Value == null ? "" : worksheet.Cells[row, 1].Value.ToString().Trim(),

                                                SKU = worksheet.Cells[row, 2].Value == null ? "" : worksheet.Cells[row, 2].Value.ToString().Trim(),
                                                ApprovedPrice = worksheet.Cells[row, 3].Value == null ? "" : worksheet.Cells[row, 3].Value.ToString().Trim(),
                                                Currency = worksheet.Cells[row, 4].Value == null ? "" : worksheet.Cells[row, 4].Value.ToString().Trim(),

                                                RowNumber = row.ToString(),

                                            });
                                        }


                                    }

                                    bool st = UploadFilesToS3.InsertS3FileDataOfPOProduct(jobdetails.Job_Id, SkuPricemodel);
                                    // bool st = true;
                                    if (st == true)
                                    {
                                        UploadFilesToS3.UpdateFileJobsASCompleted(jobdetails.Job_Id);
                                    }

                                }
                            }

                            responseBody = reader.ReadToEnd(); // Now you process the response body.
                        }
                    }
                    catch (AmazonS3Exception e)
                    {
                        Console.WriteLine("Error encountered ***. Message:'{0}' when writing an object", e.Message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
                    }


                }

                if (jobdetails.Job_Type.Equals("InventoryContinueDiscontinue"))
                {
                    string responseBody = "";
                    try
                    {
                        GetObjectRequest request = new GetObjectRequest
                        {
                            BucketName = jobdetails.File_Bucket,
                            Key = jobdetails.File_Name
                        };
                        using (GetObjectResponse response = await _s3Client.GetObjectAsync(request))
                        using (Stream responseStream = response.ResponseStream)
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            // using (MemoryStream stream = new MemoryStream(bin))
                            using (ExcelPackage excelPackage = new ExcelPackage(responseStream))
                            {
                                List<ProductContinueDisContinueViewModel> SKUlist = new List<ProductContinueDisContinueViewModel>();

                                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0];
                                var rowCount = worksheet.Dimension.Rows;

                                string SKU = worksheet.Cells[1, 1].Value.ToString().Trim();
                                string Continue = worksheet.Cells[1, 2].Value.ToString().Trim();

                                if (Continue == "Continue" && SKU == "SKU")
                                {
                                    for (int row = 2; row <= rowCount; row++)
                                    {
                                        if (worksheet.Cells[row, 1].Value != null && worksheet.Cells[row, 1].Value.ToString() != String.Empty)
                                        {

                                            ProductContinueDisContinueViewModel obj = new ProductContinueDisContinueViewModel();
                                            obj.SKU = worksheet.Cells[row, 1].Value == null ? "" : worksheet.Cells[row, 1].Value.ToString().Trim();
                                            string conti = worksheet.Cells[row, 2].Value == null ? "" : worksheet.Cells[row, 2].Value.ToString().Trim();
                                            if (conti.ToUpper() == "Enable".ToUpper())
                                            {
                                                obj.Continue = true;
                                            }
                                            if (conti.ToUpper() == "Disable".ToUpper())
                                            {
                                                obj.Continue = false;
                                            }
                                            obj.RowNumber = row.ToString();
                                            SKUlist.Add(obj);
                                        }


                                    }
                                    //bool st = true;
                                    bool st = UploadFilesToS3.InsertS3FileDataOfInventory(jobdetails.Job_Id, SKUlist);

                                    if (st == true)
                                    {
                                        UploadFilesToS3.UpdateFileJobsASCompleted(jobdetails.Job_Id);
                                    }

                                }
                            }

                            responseBody = reader.ReadToEnd(); // Now you process the response body.
                        }
                    }
                    catch (AmazonS3Exception e)
                    {
                        Console.WriteLine("Error encountered ***. Message:'{0}' when writing an object", e.Message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }


        }


        public OrderRelationViewModel GetOrdersFromSCRest(int OrderID, string Token)
        {
            OrderRelationViewModel orderRelationViewModel = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ApiURL + "/Orders/" + OrderID);
                request.Method = "GET";
                request.Accept = "application/json;";
                request.ContentType = "application/json";
                request.Headers["Authorization"] = "Bearer " + Token;

                string strResponse = "";
                using (WebResponse webResponse = request.GetResponse())
                {
                    using (StreamReader stream = new StreamReader(webResponse.GetResponseStream()))
                    {
                        strResponse = stream.ReadToEnd();
                    }
                }
                var X = JObject.Parse(strResponse);
                if (X != null)
                {
                    
                    var RelatedOrders = X["RelatedOrders"];
                    var count = RelatedOrders.Count();
                    if (RelatedOrders != null && RelatedOrders.Count()>0)
                    {
                        orderRelationViewModel = new OrderRelationViewModel();
                        string relation = RelatedOrders[0].Value<string>("RelationshipType").ToString();
                        if (relation == "1" || relation == "4")
                        {
                            orderRelationViewModel.BB_OrderID = X["OrderDetails"].Value<string>("OrderSourceOrderId").ToString();
                            orderRelationViewModel.SC_ChildID = OrderID;
                            orderRelationViewModel.SC_ParentID = Convert.ToInt32(RelatedOrders[0].Value<string>("RelatedOrderID"));

                        }
                    }
                }


            }
            catch (WebException ex)
            {

            }
            return orderRelationViewModel;
        }

        public async Task<bool> GetChildOrdersFromSellerCloud(OrderRelationViewModel orderRelationView, GetChannelCredViewModel _getChannelCredViewModel)
        {
            List<string> bbOrderIds = new List<string>();
            bool status = false;

            try
            {


                authHeader = new AuthHeader();
                authHeader.ValidateDeviceID = false;
                authHeader.UserName = _getChannelCredViewModel.UserName;
                authHeader.Password = _getChannelCredViewModel.Key;
                ServiceReference1.SCServiceSoapClient sCServiceSoap =
                       new ServiceReference1.SCServiceSoapClient(ServiceReference1.SCServiceSoapClient.EndpointConfiguration.SCServiceSoap12);

                //ServiceReference1.UpdateOrderDropShipStatusRequest request = new UpdateOrderDropShipStatusRequest(authHeader, null, 2345, DropShipStatusType1.Requested);

                TimeSpan time1 = TimeSpan.FromHours(1); // my attempt to add 2 hours
                TimeSpan ts = DateTime.Now.TimeOfDay;
                var newTs = ts.Add(time1);
                sCServiceSoap.InnerChannel.OperationTimeout = newTs;
                ServiceOptions serviceOptions = new ServiceOptions();

                serviceOptions.AlwaysRecalculateWeight = false;
                serviceOptions.BulkDeleteShadows = false;
                serviceOptions.BulkWipeRelationships = false;
                serviceOptions.DontIncludePORMAImages = false;
                serviceOptions.FetchUserDefinedColumnsForOrder = false;
                serviceOptions.IncludeClientUserDocuments = false;
                serviceOptions.IncludePODetails = false;
                serviceOptions.SaveOrderPackageDimensions = false;
                serviceOptions.SkipBundleItemQtyUpdating = false;
                serviceOptions.UseCache = true;
                serviceOptions.DontNeedCompanyProfile = false;
                serviceOptions.FetchUserDefinedColumnsForProducts = false;
                serviceOptions.IncludeShippingSuggestions = false;
                serviceOptions.SkipCWAShippingRules = false;
                serviceOptions.AllowAnyProductShippingMethods = false;

                List<string> keys = new List<string>();
                List<string> values = new List<string>();

                SerializableDictionaryOfStringString filters = new SerializableDictionaryOfStringString();
                filters.Keys = keys.ToArray();
                filters.Values = values.ToArray();

                List<SellerCloudOrder_CustomerViewModel> _mainOrderDetailCustomerList = new List<SellerCloudOrder_CustomerViewModel>();
                List<int> listOrderID = new List<int>();
                listOrderID.Add(orderRelationView.SC_ChildID);
                var ordersDetail = await sCServiceSoap.Orders_GetDatasAsync(authHeader, null, listOrderID.ToArray());
                //No result//7/9/2020

                List<ImagesClass> imagesList = new List<ImagesClass>();
                List<BestBuyOrdersImportMainViewModel> listBestBuyOrders = new List<BestBuyOrdersImportMainViewModel>();
                //Prepare complete order and order detail object
                foreach (var item in ordersDetail.Orders_GetDatasResult)
                {
                    SellerCloudOrderViewModel sellerCloudOrder = new SellerCloudOrderViewModel();
                    SellerCloudCustomerDetail sellerCloudCustomer = new SellerCloudCustomerDetail();
                    SellerCloudOrder_CustomerViewModel order_orderDetail_customer = new SellerCloudOrder_CustomerViewModel();
                    List<SellerCloudOrderDetailViewModel> sellerCloudOrderDetailList = new List<SellerCloudOrderDetailViewModel>();


                    // bestbuy Order
                    BestBuyOrderImportViewModel OrderViewModel = new BestBuyOrderImportViewModel();
                    List<BestBuyOrderDetailImportViewModel> ListorderDetailViewModel = new List<BestBuyOrderDetailImportViewModel>();
                    BestBuyCustomerDetailImportViewModel customerDetailOrderViewModel = new BestBuyCustomerDetailImportViewModel();
                    BestBuyOrdersImportMainViewModel mainModel = new BestBuyOrdersImportMainViewModel();


                    OrderViewModel.order_id = item.Order.OrderSourceOrderId;
                    OrderViewModel.commercial_id = item.Order.OrderSourceOrderId.Replace("-A", "");
                    OrderViewModel.customer_id = "";
                    OrderViewModel.can_cancel = false;
                    OrderViewModel.order_state = "Shipping";
                    OrderViewModel.acceptance_decision_date = item.Order.LastUpdated;
                    OrderViewModel.created_date = item.Order.LastUpdated;
                    OrderViewModel.total_price = Convert.ToDouble(item.Order.SubTotal);// verify
                    OrderViewModel.total_commission = 0;// verify
                    OrderViewModel.sellerCloudID = item.Order.ID; //seller cloud order id
                    OrderViewModel.shipping_price = "";

                    mainModel.OrderViewModel = OrderViewModel;

                    SerializableDictionaryOfStringString stringString = item.GalleryImagesURL;
                    //List<string> tempKeys = stringString.Keys.ToList();
                    //List<string> tempValues = stringString.Values.ToList();

                    sellerCloudOrder.totalCount = item.Order.ItemCount;
                    sellerCloudOrder.dropShipStatus = System.Enum.GetName(typeof(DropShipStatusType2), item.Order.DropShipStatus);
                    sellerCloudOrder.currencyRateFromUSD = item.Order.CurrencyRateFromUSD;
                    sellerCloudOrder.lastUpdate = item.Order.LastUpdated;
                    sellerCloudOrder.timeOfOrder = item.Order.TimeOfOrder;
                    sellerCloudOrder.taxTotal = item.Order.TaxTotal;

                    //sellerCloudOrder.shippingStatus = System.Enum.GetName(typeof(OrderShippingStatus2), item.Order.ShippingStatus);  
                    sellerCloudOrder.shippingStatus = System.Enum.GetName(typeof(OrderShippingStatus2), item.Order.ShippingStatus);

                    sellerCloudOrder.shippingWeightTotalOz = item.Order.ShippingWeightTotalOz;
                    sellerCloudOrder.orderCurrencyCode = System.Enum.GetName(typeof(CurrencyCodeType2), item.Order.OrderCurrencyCode);
                    sellerCloudOrder.orderSourceOrderId = item.Order.OrderSourceOrderId;
                    sellerCloudOrder.paymentDate = item.Order.PaymentDate;//Order Date in our case
                    sellerCloudOrder.sellerCloudID = item.Order.ID; //seller cloud order id
                    sellerCloudOrder.ClientID = item.Order.ClientId;


                    Address address = item.Order.BillingAddress;

                    sellerCloudCustomer.countryName = address.CountryName;
                    sellerCloudCustomer.firstName = address.FirstName;
                    sellerCloudCustomer.lastName = address.LastName;
                    sellerCloudCustomer.phoneNumber = address.PhoneNumber;
                    sellerCloudCustomer.postalCode = address.PostalCode;
                    sellerCloudCustomer.stateCode = address.StateCode;
                    sellerCloudCustomer.stateName = address.StateName;
                    sellerCloudCustomer.streetLine1 = address.StreetLine1;
                    sellerCloudCustomer.streetLine2 = address.StreetLine2;
                    sellerCloudCustomer.city = address.City;
                    // add best by 
                    customerDetailOrderViewModel.firstname = address.FirstName;
                    customerDetailOrderViewModel.lastname = address.LastName;
                    customerDetailOrderViewModel.state = address.StateName;
                    customerDetailOrderViewModel.street_1 = address.StreetLine1;
                    customerDetailOrderViewModel.street_2 = address.StreetLine2;
                    customerDetailOrderViewModel.zip_code = address.PostalCode;
                    customerDetailOrderViewModel.phone = address.PhoneNumber;
                    customerDetailOrderViewModel.phone_secondary = "";
                    customerDetailOrderViewModel.city = address.City;
                    customerDetailOrderViewModel.country = address.CountryName;

                    mainModel.customerDetailOrderViewModel = customerDetailOrderViewModel;

                    foreach (var itemDetail in item.Order.Items)
                    {
                        SellerCloudOrderDetailViewModel sellerCloudOrderDetail = new SellerCloudOrderDetailViewModel();
                        sellerCloudOrderDetail.DropShippedOn = itemDetail.DropShippedOn;
                        sellerCloudOrderDetail.DropShippedStatus = System.Enum.GetName(typeof(DropShipStatusType), itemDetail.DropShippedStatus);
                        sellerCloudOrderDetail.OrderId = itemDetail.OrderID;
                        sellerCloudOrderDetail.MinQTY = itemDetail.MinimumQty;
                        sellerCloudOrderDetail.SKU = itemDetail.ProductID;
                        string statuscode = "5";
                        switch (System.Enum.GetName(typeof(OrderItemStatusCode), itemDetail.StatusCode))
                        {
                            case "InProcess":
                                statuscode = "5";
                                break;
                            case "Canceled":
                                statuscode = "11";
                                break;
                            case "Completed":
                                statuscode = "6";
                                break;
                            case "ShoppingCart":
                                statuscode = "4";
                                break;
                            case "ProblemOrder":
                                statuscode = "7";
                                break;
                            case "OnHold":
                                statuscode = "8";
                                break;
                            case "Quote":
                                statuscode = "9";
                                break;
                            case "Void":
                                statuscode = "10";
                                break;
                            case "InProcess or Completed":
                                statuscode = "1";
                                break;
                            case "InProcess or Hold":
                                statuscode = "2";
                                break;


                        }

                        sellerCloudOrderDetail.StatusCode = statuscode;
                        sellerCloudOrderDetail.Qty = itemDetail.Qty;
                        sellerCloudOrderDetail.ProductTitle = itemDetail.DisplayName;
                        sellerCloudOrderDetail.AdjustedSitePrice = itemDetail.AdjustedSitePrice;
                        sellerCloudOrderDetail.AverageCost = itemDetail.AverageCost;
                        sellerCloudOrderDetail.PricePerCase = itemDetail.PricePerCase;
                        sellerCloudOrderDetail.unitPrice = itemDetail.UnitPrice;
                        sellerCloudOrderDetail.UPC = itemDetail.UPC;

                        sellerCloudOrderDetailList.Add(sellerCloudOrderDetail);

                        // best buy
                        BestBuyOrderDetailImportViewModel orderDetailViewModel = new BestBuyOrderDetailImportViewModel();
                        orderDetailViewModel.order_line_id = itemDetail.eBayTransactionId;//eBayTransactionId
                        orderDetailViewModel.offer_sku = itemDetail.ProductID;
                        orderDetailViewModel.quantity = itemDetail.Qty.ToString();
                        orderDetailViewModel.total_priceOrerLine = Convert.ToDouble(itemDetail.PricePerCase);
                        orderDetailViewModel.total_commissionOrderLine = 0;
                        orderDetailViewModel.order_line_state = "";
                        orderDetailViewModel.received_date = item.Order.LastUpdated;
                        orderDetailViewModel.shipped_date = item.Order.LastUpdated;
                        orderDetailViewModel.product_title = itemDetail.DisplayName;
                        orderDetailViewModel.GST = 0;
                        orderDetailViewModel.PST = 0;
                        ListorderDetailViewModel.Add(orderDetailViewModel);
                    }
                    mainModel.orderDetailViewModel = ListorderDetailViewModel;

                    listBestBuyOrders.Add(mainModel);

                    //assign object to main object
                    order_orderDetail_customer.Customer = sellerCloudCustomer;
                    order_orderDetail_customer.Order = sellerCloudOrder;
                    order_orderDetail_customer.orderDetail = sellerCloudOrderDetailList;

                    //main object list
                    _mainOrderDetailCustomerList.Add(order_orderDetail_customer);
                    bbOrderIds.Add(sellerCloudOrder.orderSourceOrderId);

                }

                // add data into seller cloud tables like order ,order detail ,customer detail 
                List<ImagesSaveToDatabaseWithURLViewMOdel> listImagesUrl = new List<ImagesSaveToDatabaseWithURLViewMOdel>();

                if (_mainOrderDetailCustomerList.Count > 0)
                {
                    _sellerCloudOrderDataAccess.SaveOrderAndCustomerDetail(_mainOrderDetailCustomerList);

                }


                if (listBestBuyOrders.Count > 0)
                {
                    // Get single order against order id to send email
                    IEnumerable<BestBuyOrderDetailImportViewModel> result = listBestBuyOrders.SelectMany(e => e.orderDetailViewModel);


                    // sum quantity for specifu sku to update quantity on bb
                    var finalResult = result.GroupBy(e => e.offer_sku, (x, y) => new
                    {
                        totalQty = y.Sum(r => int.Parse(r.quantity)),
                        offersku = x,
                        date = y.Max(e => e.received_date)
                    }).ToList();

                    foreach (var item in finalResult)
                    {
                        BestBuyDropShipQtyMovementViewModel model = new BestBuyDropShipQtyMovementViewModel();
                        model.ProductSku = item.offersku;
                        model.OrderQuantity = item.totalQty.ToString();
                        model.OrderDate = DateTime.Now;
                        _bestBuyProductDataAccess.SaveBestBuyOrderDropShipMovement(model);
                    }
                    // save Order in bestBuy table order and orderslines

                    _bestBuyOrderDataAccess.SaveBestBuyOrders(listBestBuyOrders);


                    orderRelationDataAccess.InsertOrderRelation(orderRelationView);

                    status = true;

                }


                _sellerCloudOrderDataAccess.InsertDataFromSellerCloudTableToBestBuyTable();



            }
            catch (Exception ex)
            {
                status = false;
            }

            return status;
        }

        public async Task<ProductInsertUpdateViewModel> GetProductInfoFromSellerCloudForMIssingSku(string productSku)
        {

            _getChannelCredViewModel = new GetChannelCredViewModel();
            _getChannelCredViewModel = _EncDecChannel.DecryptedData("sellercloud");
            authHeader = new AuthHeader();
            authHeader.ValidateDeviceID = false;
            authHeader.UserName = _getChannelCredViewModel.UserName;
            authHeader.Password = _getChannelCredViewModel.Key;

            ServiceReference1.SCServiceSoapClient sCServiceSoap =
                      new ServiceReference1.SCServiceSoapClient(ServiceReference1.SCServiceSoapClient.EndpointConfiguration.SCServiceSoap12);
            var request = await sCServiceSoap.GetProductInfoAsync(authHeader, null, productSku);
            ProductInfo productInfo = request.GetProductInfoResult;
            ProductInsertUpdateViewModel model = new ProductInsertUpdateViewModel();
            model.ProductTitle = productInfo.ProductName;
            model.Upc = productInfo.UPC;
            model.ProductSKU = productInfo.ID;
            return model;
        }
        public System.Drawing.Image DownloadImageFromUrl(string imageUrl)
        {
            System.Drawing.Image image = null;

            try
            {
                Uri ImageUri = new Uri(imageUrl, UriKind.Absolute);
                System.Net.HttpWebRequest webRequest = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(ImageUri);
                webRequest.AllowWriteStreamBuffering = true;
                webRequest.Timeout = 30000;

                System.Net.WebResponse webResponse = webRequest.GetResponse();

                System.IO.Stream stream = webResponse.GetResponseStream();

                image = System.Drawing.Image.FromStream(stream);

                webResponse.Close();
            }
            catch (Exception ex)
            {
                return null;
            }

            return image;
        }

        public async Task<bool> uploadToS3(System.IO.Stream stream, string keyName)
        {
            bool status = false;
            try
            {
                TransferUtility fileTransferUtility = new TransferUtility(new AmazonS3Client(Amazon.RegionEndpoint.USEast2));

                string bucketName;
                if (_bucketSubdirectory == "" || _bucketSubdirectory == null)
                {
                    bucketName = _ImagesbucketName; //no subdirectory just bucket name  
                }
                else
                {   // subdirectory and bucket name  
                    bucketName = _ImagesbucketName + @"/" + _bucketSubdirectory;
                }
                // 1. Upload a file, file name is used as the object key name.
                await fileTransferUtility.UploadAsync(stream, bucketName, keyName);
                status = true;
                return status;
            }
            catch (AmazonS3Exception s3Exception)
            {
                Console.WriteLine(s3Exception.Message,
                                  s3Exception.InnerException);
            }
            return status;
        }

        public async Task<bool> uploadCompressedToS3(System.IO.Stream stream, string keyName)
        {
            bool status = false;
            try
            {
                TransferUtility fileTransferUtility = new TransferUtility(new AmazonS3Client(Amazon.RegionEndpoint.USEast2));

                string bucketName;
                if (_bucketSubdirectory == "" || _bucketSubdirectory == null)
                {
                    bucketName = _ImagesbucketNameCompressed; //no subdirectory just bucket name  
                }
                else
                {   // subdirectory and bucket name  
                    bucketName = _ImagesbucketNameCompressed + @"/" + _bucketSubdirectory;
                }
                // 1. Upload a file, file name is used as the object key name.
                await fileTransferUtility.UploadAsync(stream, bucketName, keyName);
                status = true;
                return status;
            }
            catch (AmazonS3Exception s3Exception)
            {
                Console.WriteLine(s3Exception.Message,
                                  s3Exception.InnerException);
            }
            return status;
        }

        public Stream GetStream(Image img, ImageFormat format)
        {
            var ms = new MemoryStream();
            img.Save(ms, format);
            return ms;
        }

        public Stream GetStreamOfReducedImage(Image inputPath)
        {
            MemoryStream memoryStream = new MemoryStream();
            try
            {
                var settings = new MagickReadSettings { Format = MagickFormat.Raw };
                byte[] vs = ImageToByteArray(inputPath);
                int size = 100;
                int quality = 75;
                using (var image = new MagickImage(vs))
                {
                    image.Resize(size, size);
                    image.Strip();
                    image.Quality = quality;
                    image.Write(memoryStream, MagickFormat.Jpeg);
                }
            }
            catch (Exception ex)
            {
                memoryStream = null;
            }
            finally
            {

            }
            return memoryStream;
        }

        public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }
    }
}
