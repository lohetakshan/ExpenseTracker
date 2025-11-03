using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Core.Entities
{
    public class User
    {
        public Guid UserId { get; set; }
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        [MinLength(8)]
        public string Passwordhash { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        [Required]
        public string Role { get; set; } = string.Empty;

        // Navigation: A User can have multiple expenses
        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    }
}
