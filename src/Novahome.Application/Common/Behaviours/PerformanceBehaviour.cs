namespace Novahome.Application.Common.Behaviours;

/// <summary>
///   This behaviour represents a MediatR pipeline behaviour that can be registered as a part of the MediatR pipeline
///   handling process.
///   This means that this behaviour will fire at every request and can decide if the request handling should continue or
///   stop.
///   It tracks how long a request takes, and if it exceeds a certain threshold, it logs a warning.
/// </summary>
/// <typeparam name="TRequest">The generic request object</typeparam>
/// <typeparam name="TResponse">The generic response object</typeparam>
[ExcludeFromCodeCoverage]
public class PerformanceBehaviour<TRequest, TResponse>(
  ILogger<TRequest> logger,
  ICurrentUserIdService currentUserIdService
) : IPipelineBehavior<TRequest, TResponse>
  where TRequest : notnull
{
  private readonly Stopwatch _timer = new();

  public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
  {
    // start the timer
    _timer.Start();

    // execute the request
    TResponse response = await next();

    // stop the timer
    _timer.Stop();

    var elapsedMilliseconds = _timer.ElapsedMilliseconds;

    // if the request is under 500 ms, we're good
    if (elapsedMilliseconds <= 500) return response;

    // if the request exceeded the threshold, log a warning
    var userId = await currentUserIdService.GetCurrentUserId();

    if (userId is not null)
      // log the request as an authenticated long-running one
      logger.LogInformation("Incoming request {Name} from {UserId} took {ElapsedMilliseconds} milliseconds",
        typeof(TRequest).Name, userId, elapsedMilliseconds);
    else
      // log the request as an anonymous long-running one
      logger.LogInformation("Incoming request {Name} (anonymous) took {ElapsedMilliseconds} milliseconds",
        typeof(TRequest).Name, elapsedMilliseconds);

    return response;
  }
}
