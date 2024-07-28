using Novahome.Application.Condominiums.Delete;

namespace Novahome.Application.UnitTests.Condominiums.Delete;

public class CondominiumsDeleteHandlerTests
{
  private AppDbContext _dbContext = null!;
  private CondominiumsDeleteHandler _sut = null!;

  [SetUp]
  public void SetUp()
  {
    _dbContext = AppDbContextHelpers.GetInMemoryDbContext();

    _sut = new CondominiumsDeleteHandler(_dbContext);
  }

  [TearDown]
  public void TearDown()
  {
    _dbContext.Dispose();
  }

  [Test]
  public async Task Handle_ShouldDeleteExistingCondominium_WhenCalled()
  {
    // arrange
    EntityEntry<Condominium> condominium = await _dbContext.Condominiums.AddAsync(new Condominium
    {
      Name = "name",
      FiscalCode = "fiscalCode",
    });
    await _dbContext.SaveChangesAsync(CancellationToken.None);

    CondominiumsDeleteRequest request = new(condominium.Entity.Id);

    // act
    await _sut.Handle(request, CancellationToken.None);

    // assert
    Condominium? dbCondominium = await _dbContext.Condominiums.FirstOrDefaultAsync(c => c.Id == condominium.Entity.Id);
    dbCondominium.Should().BeNull();
  }

  [Test]
  public async Task Handle_ShouldThrowNotFoundException_WhenCondominiumIsNotFound()
  {
    // arrange
    CondominiumsDeleteRequest request = new(Guid.NewGuid());

    // act
    Func<Task> act = async () => await _sut.Handle(request, CancellationToken.None);

    // assert
    await act.Should().ThrowAsync<NotFoundException>();
  }
}
