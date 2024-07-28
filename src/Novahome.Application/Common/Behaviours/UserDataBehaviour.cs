namespace Novahome.Application.Common.Behaviours;

/// <summary>
///   This behaviour represents a MediatR pipeline behaviour that can be registered as a part of the MediatR pipeline
///   handling process.
///   This means that this behaviour will fire at every request and can decide if the request handling should continue or
///   stop.
///   In this case, we're checking if the authenticated user is already known and if it's already stored in the memory
///   cache.
/// </summary>
/// <typeparam name="TRequest">The generic request object</typeparam>
/// <typeparam name="TResponse">The generic response object</typeparam>
public class UserDataBehaviour<TRequest, TResponse>(
  ILogger<TRequest> logger,
  ICurrentUserIdService currentUserIdService,
  IAppDbContext dbContext,
  IAuthProviderApi authProviderApi
) : IPipelineBehavior<TRequest, TResponse>
  where TRequest : notnull
{
  public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
  {
    logger.LogDebug("Getting current user id");
    var userId = await currentUserIdService.GetCurrentUserId();

    if (userId is null)
    {
      logger.LogDebug("User id is null, skipping UserDataBehaviour");
      return await next();
    }

    logger.LogDebug("Fetching the user {UserId} from the Identity Provider", userId);
    AuthProviderUser authUser = await authProviderApi.GetUser(userId.Value, ct);

    logger.LogDebug("Fetching the user {UserId} from our app db", userId);
    User? dbUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId, ct);

    if (dbUser is not null)
    {
      logger.LogDebug("User {UserId} found in the app db, updating it with the latest data", userId);
      dbUser.EmailAddress = authUser.Email;
      dbUser.GivenName = authUser.FirstName;
      dbUser.FamilyName = authUser.LastName;
    }
    else
    {
      logger.LogDebug("User {UserId} not found in the app db, creating it with the latest data", userId);
      await dbContext.Users.AddAsync(new User
      {
        Id = userId.Value,
        EmailAddress = authUser.Email,
        GivenName = authUser.FirstName,
        FamilyName = authUser.LastName
      }, ct);
    }

    await dbContext.SaveChangesAsync(ct);

    logger.LogTrace("UserDataBehaviour completed, continuing the pipeline execution");
    return await next();
  }
}
