using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace Prism.Models
{
    public class Company
    {
        public int CompanyID { get; set; }

        [Required(ErrorMessage = "A name is required.")]
        [Index( "xi_Name",IsUnique = true) ]
        [MaxLength(25)]
        [Display(Name = "Company Name")]
        public String Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}