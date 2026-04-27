using Microsoft.EntityFrameworkCore;

namespace RestfulAPI_01
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options)
            : base(options) { }

        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
