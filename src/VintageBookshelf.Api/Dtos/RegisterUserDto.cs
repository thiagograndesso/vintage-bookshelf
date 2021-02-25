using System.ComponentModel.DataAnnotations;

namespace VintageBookshelf.Api.Dtos
{
    public class RegisterUserDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }
        
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}