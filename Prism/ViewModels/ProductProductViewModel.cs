using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Prism.Models;

namespace Prism.ViewModels
{
    public class ProductProductViewModel
    {
        public Product Product { get; set; }
        public List<ProductVariant> ProductVariant { get; set; }

        //    [Required(ErrorMessage = "Required")]
        //[Index("Name", IsUnique = true)]
        //[MaxLength(100)]
        //[Display(Name = "Product Name")]
        //public String Name { get; set; }

        //[Display(Name = "Target Gender")]
        //public Gender TargetGender { get; set; }

        //[Display(Name = "Target Weather")]
        //public Weather TargetWeather { get; set; }

        //[Required(ErrorMessage = "Required")]
        //[Display(Name = "Cost Price")]
        //public decimal CostPrice { get; set; }

        //[Required(ErrorMessage = "Required")]
        //[Display(Name = "Selling Price")]
        //public decimal SellingPrice { get; set; }

        //[Required(ErrorMessage = "Required")]
        //[Display(Name = "Department")]
        //public int DepartmentID { get; set; }

        //[Required(ErrorMessage = "Required")]
        //[Display(Name = "Company")]
        //public int CompanyID { get; set; }

        //[Display(Name = "Section")]
        //public int SectionID { get; set; }

        //[MaxLength(100)]
        //public String Variant { get; set; }

        //[Required(ErrorMessage = "Required")]
        //[Index("UPC", IsUnique = true)]
        //[MaxLength(20)]
        //[Display(Name = "UPC")]
        //public String UPC { get; set; }

        //[Display(Name = "Position")]
        //public string PositionID { get; set; }
    }
}