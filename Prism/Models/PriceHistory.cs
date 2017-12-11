using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Prism.Models
{
    public class PriceHistory
    {
        public int PriceHistoryID { get; set; }

        [ForeignKey("Product")]
        public int ProductID { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DateInserted { get; set; }

        public decimal CostPrice { get; set; }

        public decimal SellingPrice { get; set; }
        public decimal SupplyPrice { get; set; }
        
        public virtual Product Product { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}