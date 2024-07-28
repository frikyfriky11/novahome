namespace Novahome.Application.Common.Interfaces.Persistence;

/// <summary>
///   The main database structure of the app
/// </summary>
public interface IAppDbContext
{
  DbSet<Condominium> Condominiums { get; }

  DbSet<User> Users { get; }

  /// <summary>
  ///   Saves the changes asynchronously
  /// </summary>
  /// <param name="ct">The cancellation token to use</param>
  /// <returns>The number of state entries written to the database</returns>
  Task<int> SaveChangesAsync(CancellationToken ct);
}
