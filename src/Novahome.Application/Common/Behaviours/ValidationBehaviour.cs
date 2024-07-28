using ValidationException = Novahome.Application.Common.Exceptions.ValidationException;

namespace Novahome.Application.Common.Behaviours;

/// <summary>
///   This behaviour represents a MediatR pipeline behaviour that can be registered as a part of the MediatR pipeline
///   handling process.
///   This means that this behaviour will fire at every request and can decide if the request handling should continue or
///   stop.
///   It validates incoming requests that have an associated <see cref="IValidator" /> implementation and throws an
///   exception if any of the validation rules fails.
/// </summary>
/// <typeparam name="TRequest">The generic request object</typeparam>
/// <typeparam name="TResponse">The generic response object</typeparam>
[ExcludeFromCodeCoverage]
public class ValidationBehaviour<TRequest, TResponse>(
  IEnumerable<IValidator<TRequest>> validators,
  ILogger<TRequest> logger
) : IPipelineBehavior<TRequest, TResponse>
  where TRequest : notnull
{
  public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
  {
    // get the name of the request object
    var requestName = typeof(TRequest).Name;

    // if there are no validators, we can skip this behaviour
    if (!validators.Any())
    {
      logger.LogDebug("No validators configured for request {Name}", requestName);
      return await next();
    }

    logger.LogDebug("Running {Count} validators for request {Name}", validators.Count(), requestName);

    // create a new validation context for the current request
    ValidationContext<TRequest> context = new(request);

    // fire all the validators asynchronously
    ValidationResult[] validationResults = await Task.WhenAll(
      validators.Select(v =>
        v.ValidateAsync(context, ct)));

    // flatten the validation results
    List<ValidationFailure> failures = validationResults
      .Where(r => r.Errors.Count != 0)
      .SelectMany(r => r.Errors)
      .ToList();

    if (failures.Count != 0)
    {
      // at least one validation rule failed, raise an exception
      logger.LogInformation("Validation failed for request {Name}: {@Result}", requestName, failures);
      throw new ValidationException(failures);
    }

    // no errors occured, continue processing the pipeline
    logger.LogInformation("Validation succeeded for request {Name}", requestName);
    return await next();
  }
}
