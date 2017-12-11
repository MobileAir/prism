using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Linq;
using System.Web;
namespace Prism.Models
{
    public class Section
    {

        public int SectionID { get; set; }

        [Required(ErrorMessage = "Invalid")]
        [Index("xi_Name", IsUnique = true)]
        [MaxLength(25)]
        [Display(Name = "Section Name")]
        public string Name { get; set; }

        public virtual ICollection<Department> Departments { get; set; }
    }
}