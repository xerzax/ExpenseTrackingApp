using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Category
    {

        public int CategoryId { get; set; }

        public string Title { get; set; }

       /* public string Icon { get; set; } = "";*/

        public string Type { get; set; } = "Expense";
        public ICollection<Transaction> Transactions { get; set; }
    }
}
