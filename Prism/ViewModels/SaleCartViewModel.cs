using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace Prism.ViewModels
{
    public class SalesCartViewModel
    {
        public string TellerName { get; set; }
        public string CustomerName { get; set; }
        public string CartItems { get; set; } //expecting JSON string
        public decimal Total { get; set; }

    }
}