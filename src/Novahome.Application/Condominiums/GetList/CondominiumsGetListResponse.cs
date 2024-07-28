namespace Novahome.Application.Condominiums.GetList;

[PublicAPI]
public sealed record CondominiumsGetListResponse(
  Guid Id,
  string Name,
  string FiscalCode
);
