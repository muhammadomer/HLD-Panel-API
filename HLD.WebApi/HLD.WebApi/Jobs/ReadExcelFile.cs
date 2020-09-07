using DataAccess.DataAccess;
using DataAccess.Helper;
using DataAccess.ViewModels;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HLD.WebApi.Jobs
{
    [DisallowConcurrentExecution]
    public class ReadExcelFile : IJob
    {
        IConnectionString _connectionString = null;
        EncDecChannel _EncDecChannel = null;
        string ApiURL = null;
        private readonly ILogger logger;
        ProductDataAccess productData = null;
        GetChannelCredViewModel _getChannelCredViewModel = null;
        ChannelDecrytionDataAccess channelDecrytionDataAccess = null;
        string ftpUsername = "predict@homelivingdream.ca";
        string ftpPassword = "$9oTy3GY%!&WLoi";

        string ftp = "ftp://homelivingdream.ca";

        public ReadExcelFile(IConnectionString connectionString, ILogger<ReadExcelFile> _logger)
        {
            _connectionString = connectionString;


            ApiURL = "https://lp.api.sellercloud.com/rest/api";
            _EncDecChannel = new EncDecChannel(_connectionString);

            productData = new ProductDataAccess(_connectionString);
            channelDecrytionDataAccess = new ChannelDecrytionDataAccess(connectionString);
            this.logger = _logger;

        }


        public async Task Execute(IJobExecutionContext context)
        {
            int status = channelDecrytionDataAccess.CheckZincJobsStatus("ReadExcelFile");
            if (status == 1)
            {
                logger.LogInformation("ReadExcelFile Job Started At =>" + DateTime.Now.ToString());
                Downloadfile("Predict US ASIN FNSKU.xlsx");
                Downloadfile("Predict CA ASIN FNSKU.xlsx");
                logger.LogInformation("ReadExcelFile Job Stopped At =>" + DateTime.Now.ToString());
            }

            await Task.CompletedTask;
        }




        public void Downloadfile(string fileName)
        {
            logger.LogInformation("ReadExcelFile Job FileName=>" + fileName.ToString());
            try
            {

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create((new Uri(ftp + @"/" + fileName)));
                logger.LogInformation("ReadExcelFile Job => set request method to download file.");
                //set request method to download file. 
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                //request.UsePassive = false;
                request.UseBinary = true;
                request.KeepAlive = false;
                logger.LogInformation("ReadExcelFile Job => set up credentials");
                //set up credentials. 
                request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
                logger.LogInformation("ReadExcelFile Job => initialize Ftp response.");
                //initialize Ftp response.
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                logger.LogInformation("ReadExcelFile Job => open readers to read data from ftp");
                //open readers to read data from ftp 
                Stream responseStream = response.GetResponseStream();

                var memoryStream = new MemoryStream();
                responseStream.CopyTo(memoryStream);
                var fileBytes = memoryStream.ToArray();
                using (MemoryStream memStream = new MemoryStream(fileBytes))
                {
                    logger.LogInformation("ReadExcelFile Job => Converting to Excel Pakages");
                    ExcelPackage package = new ExcelPackage(memStream);
                    logger.LogInformation("ReadExcelFile Job => Generating WorkSheet");
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;
                    logger.LogInformation("ReadExcelFile Job => Reading columns headers");
                    string ShadowOf = worksheet.Cells[1, 1].Value.ToString().Trim();
                    string ProductID = worksheet.Cells[1, 2].Value.ToString().Trim();
                    string AmazonMerchantSKU = worksheet.Cells[1, 3].Value.ToString().Trim();
                    string AmazonFBASKU = worksheet.Cells[1, 4].Value.ToString().Trim();
                    string AmazonPrice = worksheet.Cells[1, 5].Value.ToString().Trim();
                    string ASIN = worksheet.Cells[1, 6].Value.ToString().Trim();
                    string UPC = worksheet.Cells[1, 7].Value.ToString().Trim();
                    if (ShadowOf == "ShadowOf" && ProductID == "ProductID" && AmazonMerchantSKU == "AmazonMerchantSKU" &&
                                    AmazonFBASKU == "AmazonFBASKU" && ASIN == "ASIN" &&
                                    UPC == "UPC" && AmazonPrice == "AmazonPrice")
                    {
                        for (int row = 2; row <= rowCount; row++)
                        {

                            try
                            {
                                if (worksheet.Cells[row, 1].Value != null && worksheet.Cells[row, 1].Value.ToString() != String.Empty)
                                {
                                    string ShadowOfRow = worksheet.Cells[row, 1].Value == null ? "" : worksheet.Cells[row, 1].Value.ToString().Trim();
                                    string ProductIDRow = worksheet.Cells[row, 2].Value == null ? "" : worksheet.Cells[row, 2].Value.ToString().Trim();
                                    string AmazonMerchantSKURow = worksheet.Cells[row, 3].Value == null ? "" : worksheet.Cells[row, 3].Value.ToString().Trim();
                                    string AmazonFBASKURow = worksheet.Cells[row, 4].Value == null ? "" : worksheet.Cells[row, 4].Value.ToString().Trim();
                                    decimal AmazonPriceRow = worksheet.Cells[row, 5].Value == null ? 0 : Convert.ToDecimal(worksheet.Cells[row, 5].Value);
                                    string ASINRow = worksheet.Cells[row, 6].Value == null ? "" : worksheet.Cells[row, 6].Value.ToString().Trim();
                                    //string UPCRow = worksheet.Cells[row, 7].Value == null ? "" : worksheet.Cells[row, 7].Value.ToString().Trim();
                                    productData.UpdateProductByExcel(ShadowOfRow, AmazonMerchantSKURow, AmazonFBASKURow, AmazonPriceRow, ASINRow);
                                    logger.LogInformation("ReadExcelFile Job Updating ShadowOF=>" + ShadowOfRow);
                                }
                                else
                                {
                                    string ShadowOfSKURow = worksheet.Cells[row, 1].Value == null ? "" : worksheet.Cells[row, 1].Value.ToString().Trim();
                                    logger.LogInformation("ReadExcelFile Job ShadowOF=>" + ShadowOfSKURow.ToString());
                                }
                            }
                            catch (Exception exp)
                            {
                                logger.LogInformation("ReadExcelFile Job ShadowOF=>" + exp.ToString());
                                continue;
                            }

                        }
                    }

                }
                response.Close();
            }
            catch (Exception ex)
            {
                logger.LogInformation("ReadExcelFile Job ShadowOF=>" + ex.ToString());
            }
        }
    }
}
