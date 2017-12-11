using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Prism.Models
{
    public class StockBalance
    {
        [Key]
        [ForeignKey("ProductVariant")]
        public int ProductVariantID { get; set; }
        public decimal Quantity { get; set; }

        public virtual ProductVariant ProductVariant { get; set; }
        
    }
}