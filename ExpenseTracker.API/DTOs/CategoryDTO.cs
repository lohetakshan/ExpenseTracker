using ExpenseTracker.Core.Entities;

namespace ExpenseTracker.API.DTOs
{
    public class CategoryDTO
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public bool IsDefault { get; set; } = false;
        public Guid? CreatedByUserId { get; set; }
        public Guid UpdatedByUserId { get; set; }
    }
}
