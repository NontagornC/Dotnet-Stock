using System.ComponentModel.DataAnnotations;

namespace dotnet_stock.DTOs.Account
{
    public class RegisterRequest
    {
        [Required]
        [EmailAddress]
        public string Username { get; set; } = null!;

        [Required]
        [MinLength(8)]
        public string Password { get; set; } = null!;

        public int RoleId { get; set; }
    }
}
