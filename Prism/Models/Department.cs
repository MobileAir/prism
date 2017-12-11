using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Prism.Models
{
    public class Department
    {
        public int DepartmentID { get; set; }

        [Required(ErrorMessage = "Invalid")]
        [Index("xi_Name", IsUnique = true)]
        [MaxLength(25)]
        [Display(Name = "Department Name")]
        public string Name { get; set; }

        [Required]
        public int SectionID { get; set; }

        public virtual ICollection<Product> Products { get; set; }
        public virtual Section Section { get; set; }
    }
}