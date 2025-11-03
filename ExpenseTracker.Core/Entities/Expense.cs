using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Core.Entities
{
    public class Expense
    {
        public Guid ExpenseId { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string? Notes { get; set; }

        // Foreign key to User
        public Guid UserId { get; set; }
        //Link to User
        public User? User { get; set; }

        // Foreign key to Category
        public Guid? CategoryId { get; set; }
        //Link to Category
        public Category? Category { get; set; }
    }
}
