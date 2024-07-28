namespace Novahome.Application.Common.Exceptions;

/// <summary>
///   Represents an exception that can be raised when a specific entity cannot be found.
///   This will be handled by the ASP.NET Core pipeline returning an HTTP 404 result.
/// </summary>
[ExcludeFromCodeCoverage]
public class NotFoundException : Exception
{
  public NotFoundException()
  {
  }

  public NotFoundException(string name, object key)
    : base($"Entity \"{name}\" ({key}) was not found.")
  {
  }
}
