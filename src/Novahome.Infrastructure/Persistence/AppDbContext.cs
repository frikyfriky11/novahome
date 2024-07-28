namespace Novahome.Infrastructure.Persistence;

/// <summary>
///   The main application database context
/// </summary>
public class AppDbContext(
  DbContextOptions<AppDbContext> options,
  IEnumerable<ISaveChangesInterceptor>? saveChangesInterceptors = null
) : DbContext(options), IAppDbContext
{
  public DbSet<Condominium> Condominiums => Set<Condominium>();

  public DbSet<User> Users => Set<User>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    // apply all the configurations for every entity that implements IEntityTypeConfiguration<T>
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

    base.OnModelCreating(modelBuilder);
  }

  [ExcludeFromCodeCoverage]
  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    // add all the interceptors
    foreach (ISaveChangesInterceptor interceptor in saveChangesInterceptors ?? [])
      optionsBuilder.AddInterceptors(interceptor);

    // add nicer exception objects using https://github.com/Giorgi/EntityFramework.Exceptions
    optionsBuilder.UseExceptionProcessor();

    base.OnConfiguring(optionsBuilder);
  }

  [ExcludeFromCodeCoverage]
  protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
  {
    // DateTimeOffset do not play nice with Postgres, this converts them all to UTC times
    configurationBuilder
      .Properties<DateTimeOffset>()
      .HaveConversion<DateTimeOffsetConverter>();

    base.ConfigureConventions(configurationBuilder);
  }
}
