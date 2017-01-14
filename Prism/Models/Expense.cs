using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Prism.Models
{
    public class Expense
    {
        public int ExpenseID { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string Purpose { get; set; }

        [Required]
        public string Recipient { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}