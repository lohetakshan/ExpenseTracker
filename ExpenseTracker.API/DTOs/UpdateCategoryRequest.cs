
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.API.DTOs
{
public class UpdateCategoryRequest
{
    [Required]
    public Guid CategoryId { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsDefault { get; set; } = false;

    [Required]
    public Guid CreatedByUserId { get; set; }
    public Guid UpdatedByUserId { get; set; }

    }
}
