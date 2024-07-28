using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Novahome.Application.Common.Behaviours;
using Novahome.Application.Common.Interfaces.Services;
using Novahome.Application.Common.Models;

namespace Novahome.Application.UnitTests.Common.Behaviours;

public class UserDataBehaviourTests
{
  private Mock<IAuthProviderApi> _authProviderApi = null!;
  private Mock<ICurrentUserIdService> _currentUserIdService = null!;
  private AppDbContext _dbContext = null!;
  private ILogger<int> _logger = null!;
  private Mock<RequestHandlerDelegate<int>> _requestHandlerDelegate = null!;
  private UserDataBehaviour<int, int> _sut = null!;

  [SetUp]
  public void SetUp()
  {
    _dbContext = AppDbContextHelpers.GetInMemoryDbContext();

    _logger = NullLoggerFactory.Instance.CreateLogger<int>();

    _currentUserIdService = new Mock<ICurrentUserIdService>();

    _authProviderApi = new Mock<IAuthProviderApi>();

    _requestHandlerDelegate = new Mock<RequestHandlerDelegate<int>>();
    _requestHandlerDelegate.Setup(m => m())
      .ReturnsAsync(999);

    _sut = new UserDataBehaviour<int, int>(_logger, _currentUserIdService.Object, _dbContext, _authProviderApi.Object);
  }

  [TearDown]
  public void TearDown()
  {
    _dbContext.Dispose();
  }

  [Test]
  public async Task Handle_ShouldSkipExecution_IfUserIdIsNotAuthenticated()
  {
    // arrange
    _currentUserIdService.Setup(m => m.GetCurrentUserId())
      .ReturnsAsync((Guid?)null);

    // act
    var result = await _sut.Handle(123, _requestHandlerDelegate.Object, CancellationToken.None);

    // assert
    result.Should().Be(999);
    _requestHandlerDelegate.Verify(m => m(), Times.Once);
    _authProviderApi.Verify(m => m.GetUser(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);

    var dbUser = await _dbContext.Users.CountAsync();
    dbUser.Should().Be(0);
  }

  [Test]
  public async Task Handle_ShouldUpdateDbUser_IfItAlreadyExists()
  {
    // arrange
    Guid userId = Guid.NewGuid();

    var oldUser = await _dbContext.Users.AddAsync(new User
    {
      Id = userId,
      GivenName = "Jane",
      FamilyName = "Smith",
      EmailAddress = "jane@example.com"
    });
    await _dbContext.SaveChangesAsync(CancellationToken.None);

    AuthProviderUser user = new(userId, "john", "john@example.com", "John", "Doe");

    _currentUserIdService.Setup(m => m.GetCurrentUserId())
      .ReturnsAsync(userId);
    _authProviderApi.Setup(m => m.GetUser(userId, It.IsAny<CancellationToken>()))
      .ReturnsAsync(user);

    // act
    var result = await _sut.Handle(123, _requestHandlerDelegate.Object, CancellationToken.None);

    // assert
    result.Should().Be(999);
    _requestHandlerDelegate.Verify(m => m(), Times.Once);

    User? dbUser = await _dbContext.Users.FirstOrDefaultAsync();
    dbUser.Should().NotBeNull();
    dbUser.Id.Should().Be(userId);
    dbUser.EmailAddress.Should().Be(user.Email);
    dbUser.GivenName.Should().Be(user.FirstName);
    dbUser.FamilyName.Should().Be(user.LastName);
  }

  [Test]
  public async Task Handle_ShouldCreateDbUser_IfItDoesNotExist()
  {
    // arrange
    Guid userId = Guid.NewGuid();

    AuthProviderUser user = new(userId, "john", "john@example.com", "John", "Doe");

    _currentUserIdService.Setup(m => m.GetCurrentUserId())
      .ReturnsAsync(userId);
    _authProviderApi.Setup(m => m.GetUser(userId, It.IsAny<CancellationToken>()))
      .ReturnsAsync(user);

    // act
    var result = await _sut.Handle(123, _requestHandlerDelegate.Object, CancellationToken.None);

    // assert
    result.Should().Be(999);
    _requestHandlerDelegate.Verify(m => m(), Times.Once);

    User? dbUser = await _dbContext.Users.FirstOrDefaultAsync();
    dbUser.Should().NotBeNull();
    dbUser.Id.Should().Be(userId);
    dbUser.EmailAddress.Should().Be(user.Email);
    dbUser.GivenName.Should().Be(user.FirstName);
    dbUser.FamilyName.Should().Be(user.LastName);
  }
}
