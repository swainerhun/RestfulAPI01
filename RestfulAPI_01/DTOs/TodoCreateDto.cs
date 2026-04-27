using System.ComponentModel.DataAnnotations;

namespace RestfulAPI_01.DTOs
{
    public class TodoCreateDto
    {
        [Required(ErrorMessage = "A cím megadása kötelező.")]
        [MinLength(3)]
        [MaxLength(100)]
        public string? Title { get; set; }

        [Required(ErrorMessage = "A kategória megadása kötelező.")]
        public int CategoryId { get; set; }
    }
}
