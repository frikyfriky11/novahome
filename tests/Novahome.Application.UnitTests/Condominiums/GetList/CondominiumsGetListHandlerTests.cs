using Novahome.Application.Condominiums.GetList;

namespace Novahome.Application.UnitTests.Condominiums.GetList;

public class CondominiumsGetListHandlerTests
{
  private AppDbContext _dbContext = null!;
  private CondominiumsGetListHandler _sut = null!;

  [SetUp]
  public void SetUp()
  {
    _dbContext = AppDbContextHelpers.GetInMemoryDbContext();

    _sut = new CondominiumsGetListHandler(_dbContext);
  }

  [TearDown]
  public void TearDown()
  {
    _dbContext.Dispose();
  }

  private async Task SeedCondominiums()
  {
    await _dbContext.Condominiums.AddRangeAsync(new Condominium
    {
      Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
      Name = "name 1",
      FiscalCode = "fiscalCode1",
    }, new Condominium
    {
      Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
      Name = "name 2",
      FiscalCode = "fiscalCode2",
    }, new Condominium
    {
      Id = Guid.Parse("00000000-0000-0000-0000-000000000003"),
      Name = "name 3",
      FiscalCode = "fiscalCode3",
    });

    await _dbContext.SaveChangesAsync(CancellationToken.None);
  }

  [Test]
  public async Task Handle_ShouldGetExistingCondominiums_WhenCalled()
  {
    // arrange
    await SeedCondominiums();

    CondominiumsGetListRequest request = new();

    // act
    List<CondominiumsGetListResponse> result = await _sut.Handle(request, CancellationToken.None);

    // assert
    result.Should().HaveCount(3);
    result[0].Id.Should().Be(Guid.Parse("00000000-0000-0000-0000-000000000001"));
    result[0].Name.Should().Be("name 1");
    result[0].FiscalCode.Should().Be("fiscalCode1");
    result[1].Id.Should().Be(Guid.Parse("00000000-0000-0000-0000-000000000002"));
    result[1].Name.Should().Be("name 2");
    result[1].FiscalCode.Should().Be("fiscalCode2");
    result[2].Id.Should().Be(Guid.Parse("00000000-0000-0000-0000-000000000003"));
    result[2].Name.Should().Be("name 3");
    result[2].FiscalCode.Should().Be("fiscalCode3");
  }

  [Test]
  public async Task Handle_ShouldGetCondominiumsSubset_WhenFilteringByName()
  {
    // arrange
    await SeedCondominiums();

    CondominiumsGetListRequest request = new("2");

    // act
    List<CondominiumsGetListResponse> result = await _sut.Handle(request, CancellationToken.None);

    // assert
    result.Should().HaveCount(1);
    result[0].Id.Should().Be(Guid.Parse("00000000-0000-0000-0000-000000000002"));
    result[0].Name.Should().Be("name 2");
    result[0].FiscalCode.Should().Be("fiscalCode2");
  }

  [Test]
  public async Task Handle_ShouldGetCondominiumsSubset_WhenFilteringByFiscalCode()
  {
    // arrange
    await SeedCondominiums();

    CondominiumsGetListRequest request = new(FiscalCode: "Code2");

    // act
    List<CondominiumsGetListResponse> result = await _sut.Handle(request, CancellationToken.None);

    // assert
    result.Should().HaveCount(1);
    result[0].Id.Should().Be(Guid.Parse("00000000-0000-0000-0000-000000000002"));
    result[0].Name.Should().Be("name 2");
    result[0].FiscalCode.Should().Be("fiscalCode2");
  }
}
