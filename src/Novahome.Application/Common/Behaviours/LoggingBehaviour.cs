namespace Novahome.Application.Common.Behaviours;

/// <summary>
///   This behaviour represents a MediatR pipeline behaviour that can be registered as a part of the MediatR pipeline
///   handling process.
///   This means that this behaviour will fire at every request and can decide if the request handling should continue or
///   stop.
///   In this case, we're only logging the incoming request and passing on to the next handler in the pipeline.
/// </summary>
/// <typeparam name="TRequest">The generic request object</typeparam>
/// <typeparam name="TResponse">The generic response object</typeparam>
[ExcludeFromCodeCoverage]
public class LoggingBehaviour<TRequest, TResponse>(
  ILogger<TRequest> logger,
  ICurrentUserIdService currentUserIdService
) : IPipelineBehavior<TRequest, TResponse>
  where TRequest : notnull
{
  public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
  {
    var userId = await currentUserIdService.GetCurrentUserId();

    if (userId is not null)
      // log the request as an authenticated one
      logger.LogInformation("Incoming request {Name} from {UserId}", typeof(TRequest).Name, userId);
    else
      // log the request as an anonymous one
      logger.LogInformation("Incoming request {Name} (anonymous)", typeof(TRequest).Name);

    // no other processing required, the next handler can fire
    return await next();
  }
}
