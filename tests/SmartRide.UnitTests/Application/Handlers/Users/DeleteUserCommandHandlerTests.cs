using Moq;
using SmartRide.Application.Commands.Users;
using SmartRide.Application.DTOs.Users;
using SmartRide.Application.Handlers.Users;
using SmartRide.Common.Exceptions;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.UnitTests.Application.Handlers.Users;

public class DeleteUserCommandHandlerTests
{
    private readonly Mock<IRepository<User>> _mockUserRepository;
    private readonly DeleteUserCommandHandler _handler;

    public DeleteUserCommandHandlerTests()
    {
        _mockUserRepository = new Mock<IRepository<User>>();
        _handler = new DeleteUserCommandHandler(_mockUserRepository.Object);
    }

    [Fact]
    public async Task Handle_Should_Delete_User_And_Return_Response()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            FirstName = "UserToBeDeleted",
            Email = "",
            Phone = "",
        };
        _mockUserRepository.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(user);
        _mockUserRepository.Setup(r => r.DeleteAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(user);

        var command = new DeleteUserCommand { UserId = userId };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.Equal(userId, result.UserId);
        _mockUserRepository.Verify(r => r.DeleteAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_When_User_Not_Found()
    {
        // Arrange
        var command = new DeleteUserCommand { UserId = Guid.NewGuid() };
        _mockUserRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(null as User);

        // Act & Assert
        await Assert.ThrowsAsync<BaseException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
