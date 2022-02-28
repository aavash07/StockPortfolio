using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PortfolioManagementSystem.Models
{
    public class Stocks
    {
        [Key]
        public int StockId { get; set; }

        [Column("varchar(100)")]
        public string StockName { get; set; }
        public IEnumerable<StockDetails> stockDetails;
        
    }
}
