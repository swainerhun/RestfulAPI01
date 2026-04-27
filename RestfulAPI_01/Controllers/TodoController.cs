using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using RestfulAPI_01.DTOs;

namespace RestfulAPI_01.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TodoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetAll()
        {
            List<TodoItem>? items = await _context.TodoItems
                .Include(t => t.Category)
                .ToListAsync();

            return Ok(items.Select(item => new TodoReadDto
            {
                Id = item.Id,
                Title = item.Title,
                IsDone = item.IsDone,
                CategoryName = item.Category?.Name ?? "N/A"
            }));

            //return await _context.TodoItems.ToListAsync(); => Dto nélkül
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetById(int id)
        {
            TodoItem? item = await _context.TodoItems.FindAsync(id);
            if (item is null) 
                return NotFound(new ApiError
                {
                    StatusCode = 404,
                    Message = $"A {id} azonosítójú elem nem található."
                });

            return Ok(new TodoReadDto
            {
                Id = item.Id,
                Title = item.Title,
                IsDone = item.IsDone
            });
        }

        [HttpPost]
        public async Task<ActionResult<TodoReadDto>> Create(TodoCreateDto dto)
        {
            Category? category = await _context.Categories.FindAsync(dto.CategoryId);

            if (category is null)
            {
                return NotFound(new ApiError
                {
                    StatusCode = 404,
                    Message = $"A {dto.CategoryId} azonosítójú kategória nem található."
                });
            }        

            TodoItem? item = new TodoItem
            {
                Title = dto.Title,
                IsDone = false,
                CategoryId = dto.CategoryId
            };

            _context.TodoItems.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = item.Id }, new TodoReadDto
            {
                Id = item.Id,
                Title = item.Title,
                IsDone = item.IsDone,
                CategoryName = category.Name
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TodoItem updated)
        {
            TodoItem? item = await _context.TodoItems.FindAsync(id);
            if(item is null) return NotFound();

            item.Title = updated.Title;
            item.IsDone = updated.IsDone;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            TodoItem? item = await _context.TodoItems.FindAsync(id);
            if (item is null) 
                return NotFound(new ApiError
                {
                    StatusCode = 404,
                    Message = $"A {id} azonosítójú todo nem található."
                });

            _context.TodoItems.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
