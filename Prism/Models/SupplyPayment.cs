using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prism.Models
{
    public class SupplyPayment
    {
        public int SupplyPaymentID { get; set; }
        [Display(Name = "Supply Customer")]
        public int SupplyCustomerID { get; set; }

        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Remark { get; set; }
        [Required]
        public virtual ApplicationUser ApplicationUser { get; set; }
        [Required]
        public virtual SupplyCustomer SupplyCustomer { get; set; }
    }
}