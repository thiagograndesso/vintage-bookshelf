using System.ComponentModel.DataAnnotations;

namespace VintageBookshelf.Api.Dtos
{
    public class LoginUserDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }
    }
}