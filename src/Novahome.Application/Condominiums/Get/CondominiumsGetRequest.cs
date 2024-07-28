namespace Novahome.Application.Condominiums.Get;

[PublicAPI]
public sealed record CondominiumsGetRequest(
  Guid Id
) : IRequest<CondominiumsGetResponse>;
