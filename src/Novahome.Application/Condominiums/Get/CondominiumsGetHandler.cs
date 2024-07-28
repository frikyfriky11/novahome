namespace Novahome.Application.Condominiums.Get;

[UsedImplicitly]
public class CondominiumsGetHandler(
  IAppDbContext dbContext
) : IRequestHandler<CondominiumsGetRequest, CondominiumsGetResponse>
{
  public async Task<CondominiumsGetResponse> Handle(CondominiumsGetRequest request, CancellationToken ct)
  {
    CondominiumsGetResponse? result = await dbContext
      .Condominiums
      .AsNoTracking()
      .Where(c => c.Id == request.Id)
      .Select(c => new CondominiumsGetResponse(
        c.Id,
        c.Name,
        c.FiscalCode
      ))
      .FirstOrDefaultAsync(ct);

    if (result is null) throw new NotFoundException();

    return result;
  }
}
