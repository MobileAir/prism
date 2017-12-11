using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prism.Models
{
    public class Cart
    {
        public int CartID { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public String RecieptNumber { get; set; }

        [Display(Name = "No. of Items")]
        [Required]
        public decimal NumberOfItems { get; set; }

        [Display(Name = "Total")]
        [Required]
        public decimal TotalValue { get; set; }

        [Required]
        public bool IsPOS { get; set; }
        
        [Required]
        public bool IsRolledBack { get; set; }

        public DateTime? RolledBackDate { get; set; }

        [Required]
        public decimal AmountPaid { get; set; }

        [Required]
        public decimal Balance { get; set; }

        public virtual POSDetail PosDetail { get; set; }

        [Required]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Required]
        public virtual Customer Customer { get; set; }

        public virtual ICollection<CartItem> CartItems { get; set; }
        

        public Cart()
        {
            IsRolledBack = false; //set the default value to false 
              
        }
    }
}