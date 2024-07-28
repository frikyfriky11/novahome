namespace Novahome.Infrastructure.Persistence.Converters;

/// <summary>
///   Converts DateTimeOffset types to their UTC value, useful when using Postgres
/// </summary>
[UsedImplicitly]
public class DateTimeOffsetConverter() : ValueConverter<DateTimeOffset, DateTimeOffset>(
  d => d.ToUniversalTime(),
  d => d.ToUniversalTime()
);
