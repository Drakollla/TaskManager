using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Models;
using TaskManager.Repository.Configuration;

namespace Repository
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new WorkTaskConfiguration());
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<WorkTask> WorkTasks { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }
    }
}