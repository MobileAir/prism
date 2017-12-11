using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prism.Models
{
    public class ProductVariant
    {
        public int ProductVariantID { get; set; }

        [MaxLength(100)]
        public string Variant { get; set; }

        [Required(ErrorMessage = "Required")]
        [Index("UPC", IsUnique = true)]
        [MaxLength(20)]
        [Display(Name = "UPC")]
        public string UPC { get; set; }

        [Required]
        public DateTime DateAdded { get; set; }

        [Required]
        public DateTime LastModified { get; set; }

        [Display(Name = "Position")]
        public string PositionID { get; set; }

        public int ProductID { get; set; }

        public virtual Product Product { get; set; }

        public virtual StockBalance StockBalance { get; set; }
        public virtual ICollection<StockInItem> StockIns { get; set; }

        public ProductVariant() //Constructor to set variant to empty string to avoid null values if the product has no variant
        {
            //TODO: Change default name to empty string
            Variant = "default variant name";
        }

        public string FullName
        {
            get { return Product.Name + " " + Variant; }
        }

    }
}