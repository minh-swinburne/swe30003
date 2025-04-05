using AutoMapper;
using Microsoft.AspNetCore.Identity;
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
    private readonly Mock<IPasswordHasher<User>> _mockPasswordHasher;
    private readonly Mock<IMapper> _mockMapper;
    private readonly CreateUserCommandHandler _handler;

    public CreateUserCommandHandlerTests()
    {
        _mockUserRepository = new Mock<IRepository<User>>();
        _mockPasswordHasher = new Mock<IPasswordHasher<User>>();
        _mockMapper = new Mock<IMapper>();
        _handler = new CreateUserCommandHandler(_mockUserRepository.Object, _mockPasswordHasher.Object, _mockMapper.Object);
    }

    private void SetupMocks(Guid userId)
    {
        _mockUserRepository.Setup(m => m.CreateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User user, CancellationToken ct) => user);
        _mockPasswordHasher.Setup(m => m.HashPassword(It.IsAny<User>(), It.IsAny<string>()))
            .Returns("hashed-password");
        _mockMapper.Setup(m => m.Map<User>(It.IsAny<CreateUserCommand>()))
            .Returns((CreateUserCommand command) => new User
            {
                Id = userId,
                FirstName = command.FirstName,
                LastName = command.LastName,
                Email = command.Email,
                Phone = command.Phone,
                Password = "hashed-password"
            });
        _mockMapper.Setup(m => m.Map<CreateUserResponseDTO>(It.IsAny<User>()))
            .Returns((User user) => new CreateUserResponseDTO
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone
            });
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
        
        SetupMocks(user.Id);

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
