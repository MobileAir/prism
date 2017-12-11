using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prism.Models
{
    public class SupplyCustomer
    {
        public int SupplyCustomerID { get; set; }

        [Required(ErrorMessage = "Enter Customer Name")]
        [Display(Name = "Customer Name")]
        [MaxLength(50)]
        [Index("Name", IsUnique = true)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter Phone Number")]
        [Display(Name = "Phone Number")]
        [MaxLength(50)]
        public string PhoneNumber { get; set; }

        public virtual ICollection<SupplyCart> SupplyCarts { get; set; }
        public virtual ICollection<SupplyPayment> SupplyPayments { get; set; }
    }
}