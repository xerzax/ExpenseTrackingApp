using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Transaction
    {
        public int TransactionId { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int Amount { get; set; }

        public string? Note { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
