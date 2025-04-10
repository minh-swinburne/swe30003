using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using SmartRide.Application.Commands.Users;
using SmartRide.Application.DTOs.Users;
using SmartRide.Application.Handlers.Users;
using SmartRide.Common.Exceptions;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;
using System.Linq.Expressions;

namespace SmartRide.UnitTests.Application.Handlers.Users;

public class UpdateUserCommandHandlerTests
{
    private readonly Mock<IRepository<User>> _mockUserRepository;
    private readonly Mock<IPasswordHasher<User>> _mockPasswordHasher;
    private readonly Mock<IMapper> _mockMapper;
    private readonly UpdateUserCommandHandler _handler;

    public UpdateUserCommandHandlerTests()
    {
        _mockUserRepository = new Mock<IRepository<User>>();
        _mockPasswordHasher = new Mock<IPasswordHasher<User>>();
        _mockMapper = new Mock<IMapper>();
        _handler = new UpdateUserCommandHandler(_mockUserRepository.Object, _mockPasswordHasher.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_Should_Update_User_And_Return_Response()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new UpdateUserCommand
        {
            UserId = userId,
            FirstName = "UpdatedFirstName",
            Email = "updatedemail@example.com",
            Phone = "0123456789"
        };
        var user = new User
        {
            Id = userId,
            FirstName = "OldFirstName",
            Email = "oldemail@example.com",
            Phone = "9876543210"
        };
        _mockUserRepository.Setup(r => r.GetByIdAsync(userId, It.IsAny<List<Expression<Func<User, object>>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(user);
        _mockUserRepository.Setup(r => r.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>())).ReturnsAsync(user);
        _mockMapper.Setup(m => m.Map<UpdateUserResponseDTO>(It.IsAny<User>())).Returns(new UpdateUserResponseDTO
        {
            UserId = userId,
            FirstName = command.FirstName,
            Email = command.Email,
            Phone = command.Phone
        });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userId, result.UserId);
        _mockUserRepository.Verify(r => r.UpdateAsync(It.Is<User>(u => u.Id == command.UserId), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_When_User_Not_Found()
    {
        // Arrange
        var command = new UpdateUserCommand { UserId = Guid.NewGuid() };
        _mockUserRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<List<Expression<Func<User, object>>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(null as User);

        // Act & Assert
        await Assert.ThrowsAsync<BaseException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
