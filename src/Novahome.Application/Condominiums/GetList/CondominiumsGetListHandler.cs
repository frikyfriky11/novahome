namespace Novahome.Application.Condominiums.GetList;

[UsedImplicitly]
public class CondominiumsGetListHandler(
  IAppDbContext dbContext
) : IRequestHandler<CondominiumsGetListRequest, List<CondominiumsGetListResponse>>
{
  public async Task<List<CondominiumsGetListResponse>> Handle(CondominiumsGetListRequest request, CancellationToken ct)
  {
    return await dbContext
      .Condominiums
      .AsNoTracking()
      .Where(c => request.Name == null || c.Name.Contains(request.Name))
      .Where(c => request.FiscalCode == null || c.FiscalCode.Contains(request.FiscalCode))
      .Select(c => new CondominiumsGetListResponse(
        c.Id,
        c.Name,
        c.FiscalCode
      ))
      .ToListAsync(ct);
  }
}
