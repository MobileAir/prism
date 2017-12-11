using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Prism.Models
{
    public class StockInItem
    {
        public int StockInItemID { get; set; }
        
        [ForeignKey("StockIn")]
        public int StockInID { get; set; }

        [ForeignKey("ProductVariant")]
        public int ProductVariantID { get; set; }

        [Required(ErrorMessage = "Required")]
        public decimal Quantity { get; set; }

        [Required]
        public decimal PreviousQuantity { get; set; }

        [Required]
        public decimal NewBalance { get; set; }
        
        public string Remark { get; set; }

        
        public virtual ProductVariant ProductVariant { get; set; }
        public virtual StockIn StockIn { get; set; }

    }
}