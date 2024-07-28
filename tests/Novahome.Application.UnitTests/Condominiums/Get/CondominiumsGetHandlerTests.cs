using Novahome.Application.Condominiums.Get;

namespace Novahome.Application.UnitTests.Condominiums.Get;

public class CondominiumsGetHandlerTests
{
  private AppDbContext _dbContext = null!;
  private CondominiumsGetHandler _sut = null!;

  [SetUp]
  public void SetUp()
  {
    _dbContext = AppDbContextHelpers.GetInMemoryDbContext();

    _sut = new CondominiumsGetHandler(_dbContext);
  }

  [TearDown]
  public void TearDown()
  {
    _dbContext.Dispose();
  }

  [Test]
  public async Task Handle_ShouldGetExistingCondominium_WhenCalled()
  {
    // arrange
    EntityEntry<Condominium> condominium = await _dbContext.Condominiums.AddAsync(new Condominium
    {
      Name = "name",
      FiscalCode = "fiscalCode",
    });
    await _dbContext.SaveChangesAsync(CancellationToken.None);

    CondominiumsGetRequest request = new(condominium.Entity.Id);

    // act
    CondominiumsGetResponse result = await _sut.Handle(request, CancellationToken.None);

    // assert
    result.Should().NotBeNull();
    result.Id.Should().Be(condominium.Entity.Id);
    result.Name.Should().Be(condominium.Entity.Name);
    result.FiscalCode.Should().Be(condominium.Entity.FiscalCode);
  }

  [Test]
  public async Task Handle_ShouldThrowNotFoundException_WhenCondominiumIsNotFound()
  {
    // arrange
    CondominiumsGetRequest request = new(Guid.NewGuid());

    // act
    Func<Task> act = async () => await _sut.Handle(request, CancellationToken.None);

    // assert
    await act.Should().ThrowAsync<NotFoundException>();
  }
}
