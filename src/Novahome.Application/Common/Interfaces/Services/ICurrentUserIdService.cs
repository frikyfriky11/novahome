namespace Novahome.Application.Common.Interfaces.Services;

/// <summary>
///   Allows getting the currently logged in user id
/// </summary>
public interface ICurrentUserIdService
{
  /// <summary>
  ///   Gets the currently authenticated user id
  /// </summary>
  /// <returns>The user id</returns>
  Task<Guid?> GetCurrentUserId();
}
