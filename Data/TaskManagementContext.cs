using Microsoft.EntityFrameworkCore;
using TaskManagementApp.Models;

namespace TaskManagementApp.Data
{
    public class TaskManagementContext : DbContext
    {
        public TaskManagementContext(DbContextOptions<TaskManagementContext> options)
            : base(options)
        {
        }

        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<BoardColumn> BoardColumns { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed initial board columns (example)
            modelBuilder.Entity<BoardColumn>().HasData(
                new BoardColumn { Id = 1, Name = "ToDo" },
                new BoardColumn { Id = 2, Name = "InProgress" },
                new BoardColumn { Id = 3, Name = "Done" }
            );
        }
    }
}
