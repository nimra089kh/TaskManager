using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManager.Models;


namespace TaskManager.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) :base(options)
        {
        }
        public DbSet<TaskItem> TaskItems { get; set; }
    }
}
