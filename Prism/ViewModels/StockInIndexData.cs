using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Prism.Models;

namespace Prism.ViewModels
{
    public class StockInIndexData
    {
        public IEnumerable<StockIn> StockIns { get; set; }
        public IEnumerable<StockInItem> StockInItems { get; set; } 
    }
}