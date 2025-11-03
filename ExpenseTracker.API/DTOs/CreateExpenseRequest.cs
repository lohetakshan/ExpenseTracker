using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.API.DTOs
{
    public class CreateExpenseRequest
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }
        
        [Required]
        public DateTime Date { get; set; }
        
        public string? Notes { get; set; }
        
        public Guid? CategoryId { get; set; }
    }
}
