using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.API.DTOs
{
    public class CreateCategoryRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [MaxLength(250)]
        public string? Description { get; set; }
        public bool IsDefault { get; set; } = false;
        [Required]
        public Guid CreatedByUserId { get; set; }
        public Guid UpdatedByUserId { get; set; }
    }
}
