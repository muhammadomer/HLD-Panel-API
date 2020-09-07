using DataAccess.DataAccess;
using DataAccess.Helper;
using DataAccess.ViewModels;
using HLD.WebApi.Enum;
using Quartz;
using ServiceReference1;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HLD.WebApi.Jobs
{
    [DisallowConcurrentExecution]
    public class BestBuyDropshipNoneWarehouseQtyFromSC_UpdateDifference_job:IJob
    {
        IConnectionString _connectionString = null;
        ServiceReference1.AuthHeader authHeader = null;
        ProductWarehouseQtyDataAccess productWarehouseQtyDataAccess = null;
        ProductDataAccess productDataAccess = null;
        public BestBuyDropshipNoneWarehouseQtyFromSC_UpdateDifference_job(IConnectionString connectionString)
        {
            _connectionString = connectionString;
            authHeader = new AuthHeader();
            authHeader.ValidateDeviceID = false;
            authHeader.UserName = "xpress.shop77@gmail.com";
            authHeader.Password = "U0tMIMrpeS*2qoIe9X%b";
            _connectionString = connectionString;
            productWarehouseQtyDataAccess = new ProductWarehouseQtyDataAccess(_connectionString);
            productDataAccess = new ProductDataAccess(_connectionString);

        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                ServiceReference1.SCServiceSoapClient sCServiceSoap =
                           new ServiceReference1.SCServiceSoapClient(ServiceReference1.SCServiceSoapClient.EndpointConfiguration.SCServiceSoap12);

                List<string> skuList = productDataAccess.GetProductDetailForWarehouseQtyUpdate();
                foreach (var sku in skuList)
                {
                    GetProductInventoryForALLWarehousesResponseType[] result = null;
                    try
                    {
                        var request = await sCServiceSoap.GetProductInventoryForALLWarehousesAsync(authHeader, null, sku);
                        result = request.GetProductInventoryForALLWarehousesResult;
                        ProductWarehouseQtyViewModel model = result.Where(e => e.WarehouseName == "HLD-CA1")
                            .Select(e => new ProductWarehouseQtyViewModel
                            {
                                WarehouseName = e.WarehouseName,
                                AvailableQty = e.QtyAvailable,
                                WarehouseID = 5
                            }).FirstOrDefault();

                        ProductWarehouseQtyForDS_None_SKU_ViewModel viewModel = new ProductWarehouseQtyForDS_None_SKU_ViewModel();
                        viewModel.update_datetime = DateTime.Now;
                        viewModel.product_sku = sku;
                        if (model.AvailableQty <= 0)
                        {
                            viewModel.warehouse_qty = 0;
                        }
                        else
                        {
                            viewModel.warehouse_qty = model.AvailableQty;
                        }
                        viewModel.warehouse_id = model.WarehouseID;
                        productWarehouseQtyDataAccess.SaveWarehouseQtyForDropshipNoneSKU(viewModel);
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + " :");
            }

            await Task.CompletedTask;
        }


    }
}
