using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Prism.Models;

namespace Prism.ViewModels
{
    public class ProductVariantsViewModel
    {
        public int ProductID { get; set; }
        public List<ProductVariant> Variants { get; set; } 
    }
}