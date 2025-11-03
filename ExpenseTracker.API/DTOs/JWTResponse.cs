using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.API.DTOs
{
    public class JWTResponse
    {
        public string Token { get; set; }
        public string UserId { get; set; }
        public string Role { get; set; }
        public string UserName { get; set; }
        
    }
}
