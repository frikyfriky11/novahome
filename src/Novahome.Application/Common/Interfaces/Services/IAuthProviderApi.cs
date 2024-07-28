namespace Novahome.Application.Common.Interfaces.Services;

/// <summary>
///   Allows interacting with the authentication provider
/// </summary>
public interface IAuthProviderApi
{
  /// <summary>
  ///   Get a single user by its unique id
  /// </summary>
  /// <param name="id">The id of the user</param>
  /// <param name="ct">A cancellation token</param>
  /// <returns>The user</returns>
  Task<AuthProviderUser> GetUser(Guid id, CancellationToken ct);
}
