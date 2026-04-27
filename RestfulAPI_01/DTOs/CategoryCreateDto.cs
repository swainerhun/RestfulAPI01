using System.ComponentModel.DataAnnotations;

namespace RestfulAPI_01.DTOs
{
    public class CategoryCreateDto
    {
        [Required(ErrorMessage = "A név megadása kötelező.")]
        [MaxLength(50)]
        public string? Name { get; set; }
    }
}
