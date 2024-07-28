namespace Novahome.Application.Condominiums.Delete;

[UsedImplicitly]
public class CondominiumsDeleteHandler(
  IAppDbContext dbContext
) : IRequestHandler<CondominiumsDeleteRequest>
{
  public async Task Handle(CondominiumsDeleteRequest request, CancellationToken ct)
  {
    Condominium? entity = await dbContext
      .Condominiums
      .Where(c => c.Id == request.Id)
      .FirstOrDefaultAsync(ct);

    if (entity is null) throw new NotFoundException();

    dbContext.Condominiums.Remove(entity);

    await dbContext.SaveChangesAsync(ct);
  }
}
