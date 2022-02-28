using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortfolioManagementSystem.Models
{
    public class StockDetailViewModel
    {
        public int TransactionId { get; set; }

        public string TransactionType { get; set; }

        public int Quantity { get; set; }

        public double Amount { get; set; }

        public string TransactionDate { get; set; }

        public int StockId { get; set; }

        public string StockName { get; set; }
    }
}
