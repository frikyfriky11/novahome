namespace Novahome.Application.Condominiums.Create;

[PublicAPI]
public sealed record CondominiumsCreateRequest(
  string Name,
  string FiscalCode
) : IRequest<CondominiumsCreateResponse>;
