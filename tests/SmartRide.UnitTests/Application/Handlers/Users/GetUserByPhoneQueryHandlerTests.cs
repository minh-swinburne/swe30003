using AutoMapper;
using MockQueryable;
using Moq;
using SmartRide.Application.DTOs.Users;
using SmartRide.Application.Queries.Users;
using SmartRide.Application.Queries.Users.Handlers;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.UnitTests.Application.Handlers.Users;

public class GetUserByPhoneQueryHandlerTests
{
    private readonly Mock<IRepository<User>> _mockUserRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetUserByPhoneQueryHandler _handler;

    public GetUserByPhoneQueryHandlerTests()
    {
        _mockUserRepository = new Mock<IRepository<User>>();
        _mockMapper = new Mock<IMapper>();
        _handler = new GetUserByPhoneQueryHandler(_mockUserRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_User_When_Phone_Is_Found()
    {
        // Arrange
        var phone = "0123456789";
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            Email = "johndoe@example.com",
            Phone = phone
        };
        var responseDto = new GetUserResponseDTO
        {
            UserId = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Phone = user.Phone,
            Roles = []
        };

        _mockUserRepository.Setup(r => r.Query(It.IsAny<CancellationToken>()))
            .Returns(new List<User> { user }.BuildMock());
        _mockMapper.Setup(m => m.Map<GetUserResponseDTO>(user)).Returns(responseDto);

        var query = new GetUserByPhoneQuery { Phone = phone };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(responseDto.Phone, result.Phone);
        _mockUserRepository.Verify(r => r.Query(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Return_Null_When_Phone_Is_Not_Found()
    {
        // Arrange
        var phone = "0000000000";
        _mockUserRepository.Setup(r => r.Query(It.IsAny<CancellationToken>()))
            .Returns(new List<User>().BuildMock());

        var query = new GetUserByPhoneQuery { Phone = phone };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Null(result);
        _mockUserRepository.Verify(r => r.Query(It.IsAny<CancellationToken>()), Times.Once);
    }
}
