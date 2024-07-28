namespace Novahome.Application.Condominiums.Create;

[UsedImplicitly]
public class CondominiumsCreateHandler(
  IAppDbContext dbContext
) : IRequestHandler<CondominiumsCreateRequest, CondominiumsCreateResponse>
{
  public async Task<CondominiumsCreateResponse> Handle(CondominiumsCreateRequest request, CancellationToken ct)
  {
    Condominium entity = new()
    {
      Name = request.Name,
      FiscalCode = request.FiscalCode,
    };

    await dbContext.Condominiums.AddAsync(entity, ct);

    await dbContext.SaveChangesAsync(ct);

    return new CondominiumsCreateResponse(entity.Id);
  }
}
