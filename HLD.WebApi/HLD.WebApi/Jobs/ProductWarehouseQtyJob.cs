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
using WarehouseName = HLD.WebApi.Enum.WarehouseName;

namespace HLD.WebApi.Jobs
{
    [DisallowConcurrentExecution]
    public class ProductWarehouseQtyJob : IJob
    {
        IConnectionString _connectionString = null;
        ServiceReference1.AuthHeader authHeader = null;
        ProductWarehouseQtyDataAccess productWarehouseQtyDataAccess = null;
        ProductDataAccess productDataAccess = null;
        EncDecChannel _EncDecChannel = null;
        GetChannelCredViewModel _getChannelCredViewModel = null;
        public ProductWarehouseQtyJob(IConnectionString connectionString)
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

                List<string> skuList = productDataAccess.GetProductDetailForWarehouseQtyUpdate_ALLSKU();
                foreach (var sku in skuList)
                {
                    GetProductInventoryForALLWarehousesResponseType[] result = null;

                    try
                    {
                        var request = await sCServiceSoap.GetProductInventoryForALLWarehousesAsync(authHeader, null, sku);
                        result = request.GetProductInventoryForALLWarehousesResult;
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }

                    List<ProductWarehouseQtyViewModel> list = new List<ProductWarehouseQtyViewModel>();
                    foreach (var item in result)
                    {
                        ProductWarehouseQtyViewModel model = new ProductWarehouseQtyViewModel();

                        if (item.WarehouseName == "DropShip Canada")
                        {
                            model.WarehouseID = (int)WarehouseName.DropShipCanada;
                            model.AvailableQty = item.QtyAvailable;
                            
                            model.ProductSku = sku;
                            list.Add(model);
                        }

                        else if (item.WarehouseName == "DropShip USA")
                        {
                            model.WarehouseID = (int)WarehouseName.DropShipUSA;
                            model.AvailableQty = item.QtyAvailable;
                            model.ProductSku = sku;
                            list.Add(model);
                        }
                        else if (item.WarehouseName == "FBA Canada")
                        {
                            model.WarehouseID = (int)WarehouseName.FBACanada;
                            model.AvailableQty = item.QtyAvailable;
                            model.ProductSku = sku;
                            list.Add(model);
                        }
                        else if (item.WarehouseName == "FBA USA")
                        {
                            model.WarehouseID = (int)WarehouseName.FBAUSA;
                            model.AvailableQty = item.QtyAvailable;
                            model.ProductSku = sku;
                            list.Add(model);
                        }
                        else if (item.WarehouseName == "HLD-CA1")
                        {
                            model.WarehouseID = (int)WarehouseName.HLDCA1;
                            model.AvailableQty = item.QtyAvailable;
                            model.ProductSku = sku;
                            list.Add(model);
                        }
                        else if (item.WarehouseName == "HLD-CA2")
                        {
                            model.WarehouseID = (int)WarehouseName.HLDCA2;
                            model.AvailableQty = item.QtyAvailable;
                            model.ProductSku = sku;
                            list.Add(model);
                        }
                        else if (item.WarehouseName == "HLD-CN1")
                        {
                            model.WarehouseID = (int)WarehouseName.HLDCN1;
                            model.AvailableQty = item.QtyAvailable;
                            model.ProductSku = sku;
                            list.Add(model);
                        }
                        else if (item.WarehouseName == "HLD-Interim")
                        {
                            model.WarehouseID = (int)WarehouseName.HLDInterim;
                            model.AvailableQty = item.QtyAvailable;
                            model.ProductSku = sku;
                            list.Add(model);
                        }
                        else if (item.WarehouseName == "HLD-Tech1")
                        {
                            model.WarehouseID = (int)WarehouseName.HLDTech1;
                            model.AvailableQty = item.QtyAvailable;
                            model.ProductSku = sku;
                            list.Add(model);
                        }
                        else if (item.WarehouseName == "Interim FBA CA")
                        {
                            model.WarehouseID = (int)WarehouseName.InterimFBACA;
                            model.AvailableQty = item.QtyAvailable;
                            model.ProductSku = sku;
                            list.Add(model);
                        }
                        else if (item.WarehouseName == "Interim FBA USA")
                        {
                            model.WarehouseID = (int)WarehouseName.InterimFBAUSA;
                            model.AvailableQty = item.QtyAvailable;
                            model.ProductSku = sku;
                            list.Add(model);
                        }
                        else if (item.WarehouseName == "NY-14305")
                        {
                            model.WarehouseID = (int)WarehouseName.NY14305;
                            model.AvailableQty = item.QtyAvailable;
                            model.ProductSku = sku;
                            list.Add(model);
                        }
                        else if (item.WarehouseName == "Shipito")
                        {
                            model.WarehouseID = (int)WarehouseName.Shipito;
                            model.AvailableQty = item.QtyAvailable;
                            model.ProductSku = sku;
                            list.Add(model);
                        }
                    }
                    productWarehouseQtyDataAccess.SaveProductQty(list);
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
