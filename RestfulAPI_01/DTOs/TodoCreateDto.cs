using System.ComponentModel.DataAnnotations;

namespace RestfulAPI_01.DTOs
{
    public class TodoCreateDto
    {
        [Required(ErrorMessage = "A cím megadása kötelező.")]
        [MinLength(3, ErrorMessage = "A címnek legalább 3 karakter hosszúnak kell lennie.")]
        [MaxLength(100, ErrorMessage = "A cím nem lehet hosszabb 100 karakternél.")]

        public string Title { get; set; } = string.Empty;
    }
}
