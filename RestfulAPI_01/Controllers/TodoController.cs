using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using RestfulAPI_01.DTOs;

namespace RestfulAPI_01.Controllers
{
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
            List<TodoItem>? items = await _context.TodoItems.ToListAsync();

            var result = items.Select(items => new TodoReadDto
            {
                Id = items.Id,
                Title = items.Title,
                IsDone = items.IsDone
            });

            return Ok(result);

            //return await _context.TodoItems.ToListAsync(); => Dto nélkül
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetById(int id)
        {
            var item = await _context.TodoItems.FindAsync(id);
            if (item is null) return NotFound();

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
            TodoItem? item = new TodoItem
            {
                Title = dto.Title,
                IsDone = false
            };

            _context.TodoItems.Add(item);
            await _context.SaveChangesAsync();

            TodoReadDto? result = new TodoReadDto
            {
                Id = item.Id,
                Title = item.Title,
                IsDone = item.IsDone
            };

            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
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
            if (item is null) return NotFound();

            _context.TodoItems.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
