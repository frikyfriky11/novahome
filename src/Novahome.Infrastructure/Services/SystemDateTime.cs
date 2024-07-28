namespace Novahome.Infrastructure.Services;

/// <summary>
///   An implementation of <see cref="IDateTime" /> that uses the system clock of the server to return the current datetime
/// </summary>
[ExcludeFromCodeCoverage]
public class SystemDateTime : IDateTime
{
  /// <inheritdoc />
  public DateTimeOffset Now => DateTimeOffset.Now;
}
