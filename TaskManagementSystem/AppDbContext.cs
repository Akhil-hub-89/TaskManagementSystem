using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Model;

namespace TaskManagementSystem
{
    public class AppDbContext : DbContext
    {
        public DbSet<TaskModel> Tasks { get; set; }
        public DbSet<ColumnModel> Columns { get; set; }
        public DbSet<UserModel> Users { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
    }
}
