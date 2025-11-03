using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.API.DTOs
{
    public class DeleteCategoryRequest
    {
        [Required]
        public Guid DeletedByUserId { get; set; }
    }
}
