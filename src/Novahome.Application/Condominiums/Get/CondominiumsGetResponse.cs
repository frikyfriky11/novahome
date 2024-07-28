namespace Novahome.Application.Condominiums.Get;

[PublicAPI]
public sealed record CondominiumsGetResponse(
  Guid Id,
  string Name,
  string FiscalCode
);
