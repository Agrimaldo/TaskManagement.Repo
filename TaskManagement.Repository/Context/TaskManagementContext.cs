
using Microsoft.EntityFrameworkCore;
using domain = TaskManagement.Domain.Entities;
using TaskManagement.Repository.Mapping;

namespace TaskManagement.Repository.Context
{
    public class TaskManagementContext : DbContext
    {
        public TaskManagementContext(DbContextOptions<TaskManagementContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TaskMap());

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<domain.Task>? Tasks { get; set; }
    }
}
