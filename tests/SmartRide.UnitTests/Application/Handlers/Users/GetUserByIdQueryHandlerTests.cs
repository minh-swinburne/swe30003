using AutoMapper;
using Moq;
using SmartRide.Application.DTOs.Users;
using SmartRide.Application.Queries.Users;
using SmartRide.Application.Queries.Users.Handlers;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.UnitTests.Application.Handlers.Users;

public class GetUserByIdQueryHandlerTests
{
    private readonly Mock<IRepository<User>> _mockUserRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetUserByIdQueryHandler _handler;

    public GetUserByIdQueryHandlerTests()
    {
        _mockUserRepository = new Mock<IRepository<User>>();
        _mockMapper = new Mock<IMapper>();
        _handler = new GetUserByIdQueryHandler(_mockUserRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_User_When_User_Is_Found()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            FirstName = "John",
            LastName = "Doe",
            Email = "johndoe@example.com",
            Phone = "0123456789"
        };
        var responseDto = new GetUserResponseDTO
        {
            UserId = userId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Phone = user.Phone,
            Roles = []
        };

        _mockUserRepository.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(user);
        _mockMapper.Setup(m => m.Map<GetUserResponseDTO>(user)).Returns(responseDto);

        var query = new GetUserByIdQuery { UserId = userId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(responseDto.UserId, result.UserId);
        Assert.Equal(responseDto.FirstName, result.FirstName);
        Assert.Equal(responseDto.Email, result.Email);
        _mockUserRepository.Verify(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Return_Null_When_User_Is_Not_Found()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _mockUserRepository.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(null as User);

        var query = new GetUserByIdQuery { UserId = userId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Null(result);
        _mockUserRepository.Verify(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
    }
}
