using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prism.DTO
{
    public class ProductDTO
    {
        public string UPC { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
    }
}