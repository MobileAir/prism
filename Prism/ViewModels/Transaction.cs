using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prism.ViewModels
{
    public class Transaction
    {
        public DateTime Date { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public bool IsCredit { get; set; }
        public string Remark { get; set; }
        public int Id { get; set; }
    }
}