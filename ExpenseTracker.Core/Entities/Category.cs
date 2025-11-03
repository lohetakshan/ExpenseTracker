using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Core.Entities
{
    public class Category
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public bool IsDefault { get; set; } = false;

        //Ownership: Track which user created the category
        public Guid? CreatedByUserId { get; set; } //Foreign key to User who created the category
        public Guid? UpdatedByUserId { get; set; } //Foreign key to User who updated the category
        public Guid? DeletedByUserId { get; set; } //Foreign key to User who deleted the category
        public User? CreatedByUser { get; set; } //Link to User who created the category

        // Navigation: A Category can have multiple expenses
        //public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
        public ICollection<Expense> Expenses { get; set; } = []; //new approach
    }
}
