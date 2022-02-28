using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortfolioManagementSystem.Models
{
    public class StockProfitViewModel
    {
        public int TotalUnits { get; set; }
        public double TotalInvestment { get; set; }
        public double SoldAmount { get; set; }
        public double CurrentAmount { get; set; }
        public double Profit { get; set; }
    }
}
