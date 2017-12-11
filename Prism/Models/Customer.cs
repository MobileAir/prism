using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Prism.Models
{
    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }

        [Required]
        public string SerialCode { get; set; }

        [Required(ErrorMessage = "Enter Customer Name")]
        [Display(Name = "Customer Name")]
        [MaxLength(50)]
        [Index("Name", IsUnique = true)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter Phone Number")]
        [Display(Name = "Phone Number")]
        [MaxLength(50)]
        public string PhoneNumber { get; set; }

        public virtual ICollection<Cart> Carts { get; set; }
    }
}