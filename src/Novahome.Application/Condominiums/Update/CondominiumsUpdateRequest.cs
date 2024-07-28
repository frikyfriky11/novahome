namespace Novahome.Application.Condominiums.Update;

[PublicAPI]
public sealed record CondominiumsUpdateRequest(
  string Name,
  string FiscalCode
) : IRequest
{
  public Guid Id { get; set; }
}
