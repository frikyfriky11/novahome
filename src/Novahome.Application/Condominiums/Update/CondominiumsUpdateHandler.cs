namespace Novahome.Application.Condominiums.Update;

[UsedImplicitly]
public class CondominiumsUpdateHandler(
  IAppDbContext dbContext
) : IRequestHandler<CondominiumsUpdateRequest>
{
  public async Task Handle(CondominiumsUpdateRequest request, CancellationToken ct)
  {
    Condominium? entity = await dbContext
      .Condominiums
      .Where(c => c.Id == request.Id)
      .FirstOrDefaultAsync(ct);

    if (entity is null) throw new NotFoundException();

    entity.Name = request.Name;
    entity.FiscalCode = request.FiscalCode;

    await dbContext.SaveChangesAsync(ct);
  }
}
