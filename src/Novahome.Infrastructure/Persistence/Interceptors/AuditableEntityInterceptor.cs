namespace Novahome.Infrastructure.Persistence.Interceptors;

/// <summary>
///   This interceptor intercepts all the SaveChanges calls to the DbContext and updates the
///   <see cref="BaseAuditableEntity" /> properties.
/// </summary>
[ExcludeFromCodeCoverage]
public class AuditableEntityInterceptor(
  ICurrentUserIdService currentUserIdService,
  IDateTime dateTime)
  : SaveChangesInterceptor
{
  public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
  {
    UpdateEntities(eventData.Context).GetAwaiter().GetResult();

    return base.SavingChanges(eventData, result);
  }

  public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
    DbContextEventData eventData,
    InterceptionResult<int> result,
    CancellationToken ct = default
  )
  {
    await UpdateEntities(eventData.Context);

    return await base.SavingChangesAsync(eventData, result, ct);
  }

  private async Task UpdateEntities(DbContext? context)
  {
    if (context is null) return;

    var userId = await currentUserIdService.GetCurrentUserId();

    // loop on every tracked entity that inherits from BaseAuditableEntity
    foreach (var entry in context.ChangeTracker.Entries<BaseAuditableEntity>())
    {
      if (entry.State == EntityState.Added && entry.Entity.CreatedById is null)
      {
        entry.Entity.CreatedById = userId;
        entry.Entity.CreatedOn = dateTime.Now;
      }

      if (entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
      {
        entry.Entity.LastModifiedById = userId;
        entry.Entity.LastModifiedOn = dateTime.Now;
      }
    }
  }
}
