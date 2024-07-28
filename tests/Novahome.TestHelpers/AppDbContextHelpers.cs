namespace Novahome.TestHelpers;

public static class AppDbContextHelpers
{
  public static AppDbContext GetInMemoryDbContext()
  {
    DbContextOptions<AppDbContext> dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
      .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

    return new AppDbContext(dbContextOptions);
  }
}
