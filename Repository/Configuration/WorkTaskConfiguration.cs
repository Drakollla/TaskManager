using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Models;

namespace TaskManager.Repository.Configuration
{
    public class WorkTaskConfiguration : IEntityTypeConfiguration<WorkTask>
    {
        public void Configure(EntityTypeBuilder<WorkTask> builder)
        {
            builder.Property(t => t.Title).IsRequired().HasMaxLength(100);

            builder.HasMany(t => t.Tags)
                .WithMany(tag => tag.Tasks)
                .UsingEntity(j => j.ToTable("TaskTags"));
        }
    }
}