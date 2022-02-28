using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PortfolioManagementSystem.Models
{
    public class StockDetails
    {
        [Key]
        public int TransactionId { get; set; }

        [Column(TypeName = "nvarchar(5)")]
        public string TransactionType { get; set; }

        [Column(TypeName = "nvarchar(5)")]
        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public int Quantity { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public double Amount { get; set; }

        [Column(TypeName ="date")]
        public DateTime TransactionDate { get; set; }

        [ForeignKey("Stocks")]
        public int StockId { get; set; }
        public Stocks Stocks{ get; set; }
    }
}
