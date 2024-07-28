namespace Novahome.WebApi.Services;

/// <summary>
///   This service is used to obtain the current and impersonated user id of the current request.
///   The authenticated user id is accessed via the claim in the access token.
/// </summary>
public class CurrentUserIdService(IHttpContextAccessor httpContextAccessor) : ICurrentUserIdService
{
  /// <inheritdoc />
  public Task<Guid?> GetCurrentUserId()
  {
    var userIdClaim = httpContextAccessor.HttpContext?.User.FindFirstValue("sub");

    if (userIdClaim is null || !Guid.TryParse(userIdClaim, out Guid userId)) return Task.FromResult<Guid?>(null);

    return Task.FromResult<Guid?>(userId);
  }
}
