using System.ComponentModel.DataAnnotations;

namespace RestfulAPI_01.DTOs
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [MinLength(6)]
        public string? Password { get; set; }
    }
}
