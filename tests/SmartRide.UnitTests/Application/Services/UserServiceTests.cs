using Moq;
using AutoMapper;
using MediatR;
using SmartRide.Application.DTOs.Auth;
using SmartRide.Application.DTOs.Users;
using SmartRide.Application.Services;
using SmartRide.Common.Responses;
using System.Security.Claims;
using SmartRide.Application.DTOs.Lookup;
using SmartRide.Application.DTOs;
using SmartRide.Application.Queries.Users;
using SmartRide.Domain.Interfaces;
using SmartRide.Common.Responses.Errors;

namespace SmartRide.UnitTests.Application.Services;

public class UserServiceTests
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IJwtService> _mockJwtService;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _mockMediator = new Mock<IMediator>();
        _mockMapper = new Mock<IMapper>();
        _mockJwtService = new Mock<IJwtService>();
        _userService = new UserService(_mockMediator.Object, _mockMapper.Object, _mockJwtService.Object);
    }

    [Fact]
    public async Task GetCurrentUserAsync_Should_Return_User_When_Token_Is_Valid()
    {
        // Arrange
        var user = new GetUserResponseDTO
        {
            UserId = Guid.NewGuid(),
            Email = "test@example.com",
            Phone = "+12 345 678 901",
            FirstName = "John",
            LastName = "Doe",
            Roles = [new RoleDTO { Name = "Passenger" }]
        };
        var token = "valid-token";
        var payload = new Dictionary<string, object>
        {
            { "sub", user.UserId.ToString() },
            { "email", user.Email },
            { "phone", user.Phone },
            { "firstName", user.FirstName },
            { "lastName", user.LastName },
            { "roles", string.Join(",", user.Roles.Select(r => r.Name)) }
        };

        _mockJwtService
            .Setup(a => a.DecodeToken(It.IsAny<string>()))
            .Returns(payload);

        _mockMediator
            .Setup(m => m.Send(It.IsAny<GetUserByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        var result = await _userService.GetCurrentUserAsync(new GetCurrentUserRequestDTO { AccessToken = token });

        // Assert
        Assert.NotNull(result.Data);
        Assert.Equal(user.UserId, result.Data.UserId);

        _mockJwtService.Verify(a => a.DecodeToken(It.IsAny<string>()), Times.Once);
        _mockMediator.Verify(m => m.Send(It.IsAny<GetUserByIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetCurrentUserAsync_Should_Return_Error_When_Token_Is_Invalid()
    {
        // Arrange
        var token = "invalid-token";

        _mockJwtService
            .Setup(a => a.DecodeToken(It.IsAny<string>()))
            .Returns([]);

        // Act
        var result = await _userService.GetCurrentUserAsync(new GetCurrentUserRequestDTO { AccessToken = token });

        // Assert
        Assert.Null(result.Data);
        Assert.NotNull(result.Info);
        Assert.Equal(AuthErrors.TOKEN_EMPTY, result.Info);

        _mockJwtService.Verify(a => a.DecodeToken(It.IsAny<string>()), Times.Once);
        _mockMediator.Verify(m => m.Send(It.IsAny<GetUserByIdQuery>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    // [Fact]
    // public async Task RegisterAsync_Should_Create_User_And_Login()
    // {
    //     // Arrange
    //     var request = new CreateUserRequestDTO { Email = "test@example.com", Password = "password123" };
    //     var createdUser = new CreateUserResponseDTO { UserId = Guid.NewGuid(), Email = request.Email };
    //     var user = new GetUserResponseDTO { UserId = createdUser.UserId, Email = request.Email, Password = request.Password };
    //     var token = "generated-token";

    //     _mockMediator
    //         .Setup(m => m.Send(It.IsAny<CreateUserCommand>(), It.IsAny<CancellationToken>()))
    //         .ReturnsAsync(createdUser);

    //     _mockMediator
    //         .Setup(m => m.Send(It.IsAny<GetUserByIdQuery>(), It.IsAny<CancellationToken>()))
    //         .ReturnsAsync(user);

    //     _mockAuthService
    //         .Setup(a => a.LoginAsync(It.IsAny<LoginRequestDTO>()))
    //         .ReturnsAsync(new ResponseDTO<AuthResponseDTO> { Data = new AuthResponseDTO { AccessToken = token } });

    //     // Act
    //     var result = await _userService.RegisterAsync(request);

    //     // Assert
    //     Assert.NotNull(result.Data);
    //     Assert.Equal(token, result.Data.AccessToken);
    //     _mockMediator.Verify(m => m.Send(It.IsAny<CreateUserCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    //     _mockMediator.Verify(m => m.Send(It.IsAny<GetUserByIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    //     _mockAuthService.Verify(a => a.LoginAsync(It.IsAny<LoginRequestDTO>()), Times.Once);
    // }
}
