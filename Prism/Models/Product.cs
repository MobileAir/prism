using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prism.Models
{
    public enum Gender
    {
        M, F, Both
    }

    public enum Weather
    {
        Summer, Rain, Harmattan, All
    }

    public enum TargetAge
    {
        Kids, Adult, All
    }
    public class Product
    {
        
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Required")]
        [Index ("Name", IsUnique = true)]
        [MaxLength (100)]
        [Display(Name = "Product Name")]
        public String Name { get; set; }

        [Display(Name="Target Gender")]
        public Gender TargetGender { get; set; }

        [Display(Name = "Target Weather")]
        public Weather TargetWeather { get; set; }

        [Required (ErrorMessage = "Required")]
        [Display(Name = "Cost Price")]
        public decimal CostPrice { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Selling Price")]
        public decimal SellingPrice { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Supply Price")]
        public decimal SupplyPrice { get; set; }

        [Display(Name = "Target Age")]
        public TargetAge TargetAge { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        public DateTime LastModified { get; set; }


        [Required(ErrorMessage = "Required")]
        [Display (Name = "Department")]
        public int DepartmentID { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Company")]
        public int CompanyID{ get; set; }
        
        public virtual Company Company{ get; set; }
        public virtual Department Department { get; set; }
        public virtual ICollection<PriceHistory> PriceHistory { get; set; }
        public virtual ICollection<ProductVariant> ProductVariant { get; set; } 


    }
}