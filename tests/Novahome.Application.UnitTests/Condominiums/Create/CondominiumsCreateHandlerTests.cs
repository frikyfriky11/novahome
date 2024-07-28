using Novahome.Application.Condominiums.Create;

namespace Novahome.Application.UnitTests.Condominiums.Create;

public class CondominiumsCreateHandlerTests
{
  private AppDbContext _dbContext = null!;
  private CondominiumsCreateHandler _sut = null!;

  [SetUp]
  public void SetUp()
  {
    _dbContext = AppDbContextHelpers.GetInMemoryDbContext();

    _sut = new CondominiumsCreateHandler(_dbContext);
  }

  [TearDown]
  public void TearDown()
  {
    _dbContext.Dispose();
  }

  [Test]
  public async Task Handle_ShouldCreateNewCondominium_WhenCalled()
  {
    // arrange
    CondominiumsCreateRequest request = new(
      "name",
      "fiscalCode"
    );

    // act
    CondominiumsCreateResponse result = await _sut.Handle(request, CancellationToken.None);

    // assert
    Condominium? dbCondominium = await _dbContext.Condominiums.FirstOrDefaultAsync(c => c.Id == result.Id);
    dbCondominium.Should().NotBeNull();
    dbCondominium.Name.Should().Be(request.Name);
    dbCondominium.FiscalCode.Should().Be(request.FiscalCode);
  }
}
