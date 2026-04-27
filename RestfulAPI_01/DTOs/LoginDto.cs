using System.ComponentModel.DataAnnotations;

namespace RestfulAPI_01.DTOs
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
