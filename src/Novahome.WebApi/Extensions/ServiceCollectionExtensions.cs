namespace Novahome.WebApi.Extensions;

public static class ServiceCollectionExtensions
{
  /// <summary>
  ///   Adds the WebApi services to the Dependency Injection container.
  /// </summary>
  /// <param name="services">The service collection container</param>
  /// <param name="configuration">The configuration object</param>
  public static void AddWebApiServices(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddLogging(configure => configure.AddSeq(configuration.GetSection("Seq")));

    // add the implementation of the current user service
    // this is outside the Infrastructure layer because only the API knows if a user is authenticated or not
    services.AddScoped<ICurrentUserIdService, CurrentUserIdService>();

    // add some utility services
    services.AddHttpContextAccessor();

    // add the API controllers and configure the pipeline filters for the exception handling
    services.AddControllers(options => options.Filters.Add<ExceptionsFilter>());

    // see https://github.com/FluentValidation/FluentValidation/issues/1965
    services.AddFluentValidationClientsideAdapters();

    services.Configure<ApiBehaviorOptions>(options =>
      options.SuppressModelStateInvalidFilter = true);

    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
      .AddJwtBearer(options =>
      {
        options.Authority = $"{configuration.GetValue<string>("Auth:InternalRootUrl")}/realms/{configuration.GetValue<string>("Auth:Realm")}";
        options.TokenValidationParameters.ValidIssuers =
        [
          $"{configuration.GetValue<string>("Auth:InternalRootUrl")}/realms/{configuration.GetValue<string>("Auth:Realm")}",
          $"{configuration.GetValue<string>("Auth:ExternalRootUrl")}/realms/{configuration.GetValue<string>("Auth:Realm")}",
        ];
        options.Audience = "novahome";
        options.TokenValidationParameters.NameClaimType = "name";
        options.MapInboundClaims = false;
        options.RequireHttpsMetadata = configuration.GetValue<bool>("Auth:RequireHttpsMetadata");
      });
    services.AddAuthorization();

    // add the Swagger definitions as an OpenAPI document
    services.AddOpenApiDocument(options =>
    {
      options.Title = "Novahome API";

      // add the OAuth 2.0 flow for Keycloak
      options.AddSecurity("Bearer", [], new OpenApiSecurityScheme
      {
        Type = OpenApiSecuritySchemeType.OAuth2,
        Description = "Keycloak Authentication",
        Flow = OpenApiOAuth2Flow.Implicit,
        AuthorizationUrl = $"{configuration.GetValue<string>("Auth:ExternalRootUrl")}/realms/{configuration.GetValue<string>("Auth:Realm")}/protocol/openid-connect/auth",
        TokenUrl = $"{configuration.GetValue<string>("Auth:ExternalRootUrl")}/realms/{configuration.GetValue<string>("Auth:Realm")}/protocol/openid-connect/token",
      });

      options.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("Bearer"));
    });

    // configure CORS to allow request only from trusted hosts
    services.AddCors(options =>
      options.AddPolicy("CorsPolicy", policy =>
        {
          string[] origins = configuration.GetSection("Cors:Origins").Get<string[]>() ??
                             throw new InvalidOperationException("Missing CORS configuration");

          policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithOrigins(origins)
            .WithExposedHeaders("Content-Disposition");
        }
      )
    );

    // add OTEL metrics
    services.AddOpenTelemetry()
      .ConfigureResource(r => r.AddService("Novahome.WebApi"))
      .WithMetrics(metrics =>
      {
        metrics.AddAspNetCoreInstrumentation();
        metrics.AddPrometheusExporter();
      })
      .WithTracing(tracing =>
      {
        tracing.AddSource("Novahome.WebApi");
        tracing.AddAspNetCoreInstrumentation(options =>
        {
          // skip tracing of the health checks
          options.Filter = context => !context.Request.Path.Equals("/healthz");
        });
        tracing.AddHttpClientInstrumentation(options =>
        {
          // skip tracing of the logs sent to the log server itself
          options.FilterHttpRequestMessage = httpRequestMessage => httpRequestMessage.RequestUri?.PathAndQuery != "/api/events/raw";
        });
        tracing.AddNpgsql();
        tracing.AddConsoleExporter();
        tracing.AddOtlpExporter();
      });
  }

  /// <summary>
  ///   Uses the WebApi services from the Dependency Injection container
  /// </summary>
  /// <param name="app">The app builder</param>
  /// <param name="configuration">The configuration object</param>
  public static void UseWebApiServices(this IApplicationBuilder app, IConfiguration configuration)
  {
    // initialize the database context only if not running for NSwag generation
    if (!configuration.GetValue<bool>("RUNNING_NSWAG"))
    {
      using IServiceScope scope = app.ApplicationServices.CreateScope();
      AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

      dbContext.Database.Migrate();
    }

    // configure the OpenAPI generators and the Swagger GUI
    app.UseOpenApi();
    app.UseSwaggerUi(settings =>
    {
      settings.OAuth2Client = new OAuth2ClientSettings
      {
        ClientId = "novahome",
        AppName = "Novahome",
        Realm = "common",
      };
    });

    // add the health checks endpoint
    app.UseHealthChecks("/healthz");

    // add the attribute routing capability
    app.UseRouting();

    // use the CORS policy configured
    app.UseCors("CorsPolicy");

    // add the attribute authorization capability
    app.UseAuthentication();
    app.UseAuthorization();

    // map the controller endpoints
    app.UseEndpoints(endpoints =>
    {
      endpoints.MapControllers();
      endpoints.MapPrometheusScrapingEndpoint();
    });
  }
}
