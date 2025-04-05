using AutoMapper;
using Moq;
using SmartRide.Application.Commands.Users;
using SmartRide.Application.Commands.Users.Handlers;
using SmartRide.Application.DTOs.Users;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.UnitTests.Application.Handlers.Users;

public class CreateUserCommandHandlerTests
{
    private readonly Mock<IRepository<User>> _mockUserRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly CreateUserCommandHandler _handler;

    public CreateUserCommandHandlerTests()
    {
        _mockUserRepository = new Mock<IRepository<User>>();
        _mockMapper = new Mock<IMapper>();
        _handler = new CreateUserCommandHandler(_mockUserRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_Should_Create_User_And_Return_Response()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "johndoe@example.com",
            Phone = "0123456789",
            Password = "secure-password"
        };

        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = command.FirstName,
            LastName = command.LastName,
            Email = command.Email,
            Phone = command.Phone
        };

        var response = new CreateUserResponseDTO
        {
            UserId = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Phone = user.Phone
        };

        _mockMapper.Setup(m => m.Map<User>(command)).Returns(user);
        _mockMapper.Setup(m => m.Map<CreateUserResponseDTO>(user)).Returns(response);
        _mockUserRepository.Setup(r => r.CreateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(user));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(response.UserId, result.UserId);
        Assert.Equal(response.FirstName, result.FirstName);
        Assert.Equal(response.Email, result.Email);
        _mockUserRepository.Verify(r => r.CreateAsync(It.Is<User>(u => u.Email == command.Email), It.IsAny<CancellationToken>()), Times.Once);
    }
}
