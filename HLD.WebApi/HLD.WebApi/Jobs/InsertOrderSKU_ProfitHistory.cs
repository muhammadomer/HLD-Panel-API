using DataAccess.DataAccess;
using DataAccess.Helper;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.ViewModels;

namespace HLD.WebApi.Jobs
{
    [DisallowConcurrentExecution]
    public class InsertOrderSKU_ProfitHistory : IJob
    {
        private readonly IConnectionString connectionString;
        HLDHistoryDataAccess hLDHistoryDataAccess = null;
        public InsertOrderSKU_ProfitHistory(IConnectionString connectionString)
        {

            this.connectionString = connectionString;
            hLDHistoryDataAccess = new HLDHistoryDataAccess(this.connectionString);
        }
        public async Task Execute(IJobExecutionContext context)
        {

            try
            {
                List<SC_BB_OrderIDsViewModel> list = hLDHistoryDataAccess.GetSCOrderID_SKU_Profit_Calculation_History();
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        List<Order_SKU_Profit_History> model = hLDHistoryDataAccess.GetSCOrderDetail_SKU_Profit_Calculation_History(item);

                        if (model != null)
                        {
                            foreach (var historyModel in model)
                            {
                                hLDHistoryDataAccess.SaveOrder_SKU_ProfitHistory(historyModel);
                            }
                        }

                    }
                }
            }
            catch (Exception exception)
            {
            }
            await Task.CompletedTask;
        }
    }

}
