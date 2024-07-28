namespace Novahome.Application.Condominiums.Delete;

[PublicAPI]
public sealed record CondominiumsDeleteRequest(
  Guid Id
) : IRequest;
