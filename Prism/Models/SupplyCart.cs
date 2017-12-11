using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Prism.Models
{
    public class SupplyCart
    {
        public int SupplyCartID { get; set; }

        public DateTime Date { get; set; }
        
        [DisplayName("Reciept No.")]
        public string RecieptNumber { get; set; }

        [Editable(false)]
        [DisplayName("No. Of Items")]
        public decimal NumberOfItems { get; set; }

        public int SupplyCustomerID { get; set; }

        [Editable(false)]
        [DisplayName("Total Value")]
        public decimal TotalValue { get; set; }

        [Required]
        public virtual SupplyCustomer SupplyCustomer { get; set; }

        public virtual ICollection<SupplyCartItem> SupplyCartItems { get; set; }

        [Required]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}