
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DataAccess.ViewModels
{
    public class ProductInsertUpdateViewModel
    {
        public int ProductId { get; set; }       
        public string ProductSKU { get; set; }       
        public string ProductTitle { get; set; }        
        public int ConditionId { get; set; }
        public IEnumerable<SelectListItem> Condition { get; set; }                
        public String Color { get; set; }
        public string ColorAlias { get; set; }
        public int ColorId { get; set; }
        public int BrandId { get; set; }
        public String Brand { get; set; }       
        public string Upc { get; set; }         
        public string Category { get; set; }         
        public string Description { get; set; }        
        public decimal AvgCost { get; set; }        
        public decimal ShipmentWeight { get; set; }
        public string CategoryIds { get; set; }
        public int CategoryMain { get; set; }
        public int CategorySub1 { get; set; }
        public int CategorySub2 { get; set; }
        public int CategorySub3 { get; set; }
        public int CategorySub4 { get; set; }
        public decimal shipmentLenght { get; set; }
        public decimal shipmentWidth { get; set; }
        public decimal shipmentHeight { get; set; }
        public string StatusName { get; set; }

    }
}