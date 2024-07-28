namespace Novahome.Application.Common.Interfaces.Services;

/// <summary>
///   Gets the current time. Useful for testing because it makes tests predictable by not relying on the system clock.
/// </summary>
public interface IDateTime
{
  /// <summary>
  ///   The current date time offset
  /// </summary>
  DateTimeOffset Now { get; }
}
