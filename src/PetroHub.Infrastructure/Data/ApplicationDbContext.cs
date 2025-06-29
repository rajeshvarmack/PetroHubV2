using Microsoft.EntityFrameworkCore;
using PetroHub.Domain.Common;

namespace PetroHub.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all configurations from the current assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // Global query filters for soft delete
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
            {
                var method = typeof(ApplicationDbContext)
                    .GetMethod(nameof(SetSoftDeleteFilter))!
                    .MakeGenericMethod(entityType.ClrType);
                method.Invoke(this, new object[] { modelBuilder });
            }
        }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditableEntities();
        return await base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        UpdateAuditableEntities();
        return base.SaveChanges();
    }    private void UpdateAuditableEntities()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is IAuditable && 
                       (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            var auditable = (IAuditable)entry.Entity;
            var now = DateTime.UtcNow;

            if (entry.State == EntityState.Added)
            {
                auditable.CreatedAt = now;
                // TODO: Set CreatedBy from current user context
            }

            if (entry.State == EntityState.Modified)
            {
                auditable.UpdatedAt = now;
                // TODO: Set UpdatedBy from current user context
            }
        }
    }

    private void SetSoftDeleteFilter<T>(ModelBuilder modelBuilder) where T : class, ISoftDelete
    {
        modelBuilder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);    }
}
