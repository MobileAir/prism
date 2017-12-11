using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Prism.Models
{
    public class StockIn
    {
        public int StockInID { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string InvoiceNumber { get; set; }
        
        public virtual ICollection<StockInItem> StockInItems { get; set; }

        [Required]
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}