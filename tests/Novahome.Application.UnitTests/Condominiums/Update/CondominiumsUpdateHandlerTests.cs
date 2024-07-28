using Novahome.Application.Condominiums.Update;

namespace Novahome.Application.UnitTests.Condominiums.Update;

public class CondominiumsUpdateHandlerTests
{
  private AppDbContext _dbContext = null!;
  private CondominiumsUpdateHandler _sut = null!;

  [SetUp]
  public void SetUp()
  {
    _dbContext = AppDbContextHelpers.GetInMemoryDbContext();

    _sut = new CondominiumsUpdateHandler(_dbContext);
  }

  [TearDown]
  public void TearDown()
  {
    _dbContext.Dispose();
  }

  [Test]
  public async Task Handle_ShouldUpdateExistingCondominium_WhenCalled()
  {
    // arrange
    EntityEntry<Condominium> condominium = await _dbContext.Condominiums.AddAsync(new Condominium
    {
      Name = "name",
      FiscalCode = "fiscalCode",
    });
    await _dbContext.SaveChangesAsync(CancellationToken.None);

    CondominiumsUpdateRequest request = new("newName", "newFiscalCode")
    {
      Id = condominium.Entity.Id,
    };

    // act
    await _sut.Handle(request, CancellationToken.None);

    // assert
    Condominium? dbCondominium = await _dbContext.Condominiums.FirstOrDefaultAsync(c => c.Id == condominium.Entity.Id);
    dbCondominium.Should().NotBeNull();
    dbCondominium.Name.Should().Be(request.Name);
    dbCondominium.FiscalCode.Should().Be(request.FiscalCode);
  }

  [Test]
  public async Task Handle_ShouldThrowNotFoundException_WhenCondominiumIsNotFound()
  {
    // arrange
    CondominiumsUpdateRequest request = new("newName", "newFiscalCode")
    {
      Id = Guid.NewGuid(),
    };

    // act
    Func<Task> act = async () => await _sut.Handle(request, CancellationToken.None);

    // assert
    await act.Should().ThrowAsync<NotFoundException>();
  }
}
