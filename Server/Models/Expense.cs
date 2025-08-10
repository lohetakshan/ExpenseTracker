using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTrackerNew.Models
{
    public class Expense
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")] 
        public decimal Amount { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string Category { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; }
    }
}