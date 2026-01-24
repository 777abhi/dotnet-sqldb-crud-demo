using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Infrastructure.Data;

public class TaskContext(DbContextOptions<TaskContext> options) : DbContext(options)
{
    public DbSet<WorkTask> WorkTasks { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // WorkTask Configuration
        modelBuilder.Entity<WorkTask>(entity =>
        {
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Status)
                  .HasConversion<string>(); // Store enum as string

            // Shadow Properties
            entity.Property<DateTimeOffset>("CreatedDate");
            entity.Property<DateTimeOffset>("UpdatedDate");

            // Global Query Filter
            entity.HasQueryFilter(e => !e.IsArchived);
        });

        // Category Configuration
        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);

            // Shadow Properties
            entity.Property<DateTimeOffset>("CreatedDate");
            entity.Property<DateTimeOffset>("UpdatedDate");
        });
    }

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            // Only update if the entity type has these shadow properties
            // We know WorkTask and Category have them, but it's good practice to check if we had other entities.
            // For this scope, it's fine as we applied it to both.
            // A safer way is to check if the property exists.

            var now = DateTimeOffset.UtcNow;

            if (entry.Properties.Any(p => p.Metadata.Name == "CreatedDate") && entry.State == EntityState.Added)
            {
                entry.Property("CreatedDate").CurrentValue = now;
            }

            if (entry.Properties.Any(p => p.Metadata.Name == "UpdatedDate"))
            {
                entry.Property("UpdatedDate").CurrentValue = now;
            }
        }
    }
}
