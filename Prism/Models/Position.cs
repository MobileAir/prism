using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace Prism.Models
{
    public enum ShelfTypes
    {
        C, //Center Unit
        W //Wall Unit
    }

    public enum Face
    {
        F, //Front Face
        B //Back Face
    }
    

    public class Position
    {
        [DisplayName("Shelf Type")]
        public ShelfTypes ShelfType { get; set; }

        [Range(0, 100, ErrorMessage = "Can only be between 0 .. 100")]
        [Required(ErrorMessage = "Required")]
        [DisplayName("Serial Number")]
        public int SerialNumber { get; set; }

        public Face Side { get; set; }

        [Range(0, 6, ErrorMessage = "Can only be between 0 .. 6")]
        [Required(ErrorMessage = "Required")]
        public int Level { get; set; }



        [Index("Code", IsUnique = true)]
        [Required(ErrorMessage = "Invalid")]
        [MaxLength(25)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [DisplayName("Position")]
        public string PositionID { get; set; }

        //public  string Code { get; set; }

        public virtual ICollection<Product> Products { get; set; }


        //Calculated Properties
        public string FullName
        {
            get { return ShelfType.ToString() + SerialNumber + Side.ToString() + Level; }
        }

    }
}