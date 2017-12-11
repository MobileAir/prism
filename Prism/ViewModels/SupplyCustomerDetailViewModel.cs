using Prism.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prism.ViewModels
{
    public class SupplyCustomerDetailViewModel
    {
        public SupplyCustomer SupplyCustomer { get;set; }
        public ICollection<Transaction> Transactions { get; set; }
    }
}