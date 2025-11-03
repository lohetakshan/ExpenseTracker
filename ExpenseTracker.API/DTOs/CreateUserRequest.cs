using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.API.DTOs
{
    public class CreateUserRequest
    {
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string Name { get; set; } = string.Empty;


        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

    }
}
