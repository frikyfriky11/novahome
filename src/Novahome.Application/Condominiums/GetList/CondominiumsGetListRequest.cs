namespace Novahome.Application.Condominiums.GetList;

[PublicAPI]
public sealed record CondominiumsGetListRequest(
  string? Name = null,
  string? FiscalCode = null
) : IRequest<List<CondominiumsGetListResponse>>;
