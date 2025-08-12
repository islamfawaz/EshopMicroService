using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Ordering.Infrastructure.Data.Interceptors
{
    public class AuditableEntityInterceptor : SaveChangesInterceptor
    {
        private readonly ILoggedInUserService _loggedInUserService;

        public AuditableEntityInterceptor(ILoggedInUserService loggedInUserService)
        {
            _loggedInUserService = loggedInUserService;
        }
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChanges(eventData, result);
        }
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public void UpdateEntities(DbContext ? dbContext)
        {
            if (dbContext is null) return;
            var userId=_loggedInUserService.UserId ??"System";
            
            foreach (var entry in dbContext.ChangeTracker.Entries<IEntity>())
            {
                if (entry.State==EntityState.Added)
                {
                    entry.Entity.CreatedBy =userId;
                    entry.Entity.CreatedAt=DateTime.UtcNow;
                }
                else if (entry.State==EntityState.Added ||entry.State==EntityState.Modified ||entry.HasChangedOwnedEntities())
                {
                    
                    entry.Entity.LastModifiedBy = userId;
                    entry.Entity.LastModifiedAt = DateTime.UtcNow;
                }
            }

        }
    }

   
    
}

public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry)
    {
        return entry.References.Any(r =>
            r.TargetEntry != null &&
            r.TargetEntry.Metadata.IsOwned() &&
            (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
    }
}

