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
    public class BestBuyDropshipNoneWarehouseQtyFromSC_insert_Job : IJob
    {
        IConnectionString _connectionString = null;
        ServiceReference1.AuthHeader authHeader = null;
        ProductWarehouseQtyDataAccess productWarehouseQtyDataAccess = null;
        ProductDataAccess productDataAccess = null;
        EncDecChannel _EncDecChannel = null;
        GetChannelCredViewModel _getChannelCredViewModel = null;
        public BestBuyDropshipNoneWarehouseQtyFromSC_insert_Job(IConnectionString connectionString)
        {
            _connectionString = connectionString;
           
            productWarehouseQtyDataAccess = new ProductWarehouseQtyDataAccess(_connectionString);
            productDataAccess = new ProductDataAccess(_connectionString);
            _EncDecChannel = new EncDecChannel(_connectionString);
        }
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                _getChannelCredViewModel = new GetChannelCredViewModel();
                _getChannelCredViewModel = _EncDecChannel.DecryptedData("sellercloud");
                authHeader = new AuthHeader();
                authHeader.ValidateDeviceID = false;
                authHeader.UserName = _getChannelCredViewModel.UserName;
                authHeader.Password = _getChannelCredViewModel.Key;
                ServiceReference1.SCServiceSoapClient sCServiceSoap =
                           new ServiceReference1.SCServiceSoapClient(ServiceReference1.SCServiceSoapClient.EndpointConfiguration.SCServiceSoap12);

                List<string> skuList = productDataAccess.GetProductDetailForWarehouseQtyUpdate();
                foreach (var sku in skuList)
                {
                    GetProductInventoryForALLWarehousesResponseType[] result = null;
                    try
                    {
                        try
                        {
                            var request = await sCServiceSoap.GetProductInventoryForALLWarehousesAsync(authHeader, null, sku);
                            result = request.GetProductInventoryForALLWarehousesResult;
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                        ProductWarehouseQtyViewModel model = result.Where(e => e.WarehouseName == "HLD-CA1")
                            .Select(e => new ProductWarehouseQtyViewModel
                            {
                                WarehouseName = e.WarehouseName,
                                AvailableQty = e.QtyAvailable,
                                WarehouseID = 5
                            }).FirstOrDefault();

                        ProductWarehouseQtyForDS_None_SKU_ViewModel dropshipNonDetail = productWarehouseQtyDataAccess.GetDropShipNoneWarehouseQtyBySKU(sku);
                        ProductWarehouseQtyForDS_None_SKU_ViewModel viewModel = new ProductWarehouseQtyForDS_None_SKU_ViewModel();
                        viewModel.insert_datetime = DateTime.Now;
                        viewModel.product_sku = sku;
                        viewModel.warehouse_id = model.WarehouseID;
                        if (model.AvailableQty < 0)
                        {
                            viewModel.warehouse_qty = 0;
                        }
                        else
                        {
                            viewModel.warehouse_qty = model.AvailableQty;
                        }
                        if (dropshipNonDetail != null)
                        {
                            if (viewModel.warehouse_qty != dropshipNonDetail.warehouse_qty)
                            {
                                BestBuyDropShipQtyMovementViewModel modelForInsert = new BestBuyDropShipQtyMovementViewModel();
                                modelForInsert.DropShipQuantity = viewModel.warehouse_qty;
                                modelForInsert.OrderDate = DateTime.Now;
                                modelForInsert.ProductSku = sku;
                                modelForInsert.DropShipStatus = false;

                                productWarehouseQtyDataAccess.SaveWarehouseQtyForDropshipNoneSKU(viewModel);
                                productWarehouseQtyDataAccess.SaveBestBuyQtyMovementForDropshipNone_SKU(modelForInsert);

                            }
                        }
                        else
                        {
                            productWarehouseQtyDataAccess.SaveWarehouseQtyForDropshipNoneSKU(viewModel);
                        }
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
