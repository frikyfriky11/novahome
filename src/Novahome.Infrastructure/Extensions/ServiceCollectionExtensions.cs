namespace Novahome.Infrastructure.Extensions;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
  /// <summary>
  ///   Add the infrastructure services to the Dependency Injection container
  /// </summary>
  /// <param name="services">The service collection container</param>
  /// <param name="configuration">The configuration object</param>
  public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
  {
    // add the db context interceptors
    services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();

    // add the db context
    services.AddDbContext<AppDbContext>(options =>
    {
      options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
        builder => builder.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));
    });

    services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());

    // add the database health checks
    services.AddHealthChecks().AddDbContextCheck<AppDbContext>();

    // add the service implementations
    services.AddTransient<IDateTime, SystemDateTime>();

    // add keycloak auth provider admin api
    services.AddHttpClient<IAuthProviderApi, KeycloakAuthProviderApi>();
  }
}
