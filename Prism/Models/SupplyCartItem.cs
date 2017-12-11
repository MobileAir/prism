using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Prism.Models
{
    public class SupplyCartItem
    {
        public int SupplyCartItemID { get; set; }

        [Required]
        public int SupplyCartID { get; set; }

        [Required]
        public int ProductVariantID { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }

        [Required]
        public decimal CostPrice { get; set; }

        [Required]
        public decimal Quantity { get; set; }

        [Required]
        public decimal PreviousStockBalance { get; set; }

        public virtual SupplyCart SupplyCart { get; set; }
        public virtual ProductVariant ProductVariant { get; set; }


        //calculated properties. Columns are not created in db
        public decimal TotalCost 
        {
            get { return Math.Round(CostPrice * Quantity, 2); }
        }

        public decimal TotalValue 
        {
            get { return Math.Round(UnitPrice*Quantity, 2); }
        }
        
    }
}