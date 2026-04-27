using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestfulAPI_01.DTOs;

namespace RestfulAPI_01.Controllers
{
    //Tudom, hogy figyelsz Drelaky :3

    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryReadDto>>> GetAll()
        {
            List<Category>? categories = await _context.Categories.ToListAsync();

            return Ok(categories.Select(c => new CategoryReadDto
            {
                Id = c.Id,
                Name = c.Name
            }));
        }

        [HttpPost]
        public async Task<ActionResult<CategoryReadDto>> Create(CategoryCreateDto dto)
        {
            Category? category = new Category { Name = dto.Name };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return Ok(new CategoryReadDto
            {
                Id = category.Id,
                Name = category.Name
            });
        }
    }
}
