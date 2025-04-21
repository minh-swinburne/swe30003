using Moq;
using AutoMapper;
using MediatR;
using SmartRide.Application.DTOs.Auth;
using SmartRide.Application.DTOs.Users;
using SmartRide.Application.Interfaces;
using SmartRide.Application.Services;
using SmartRide.Common.Responses;
using System.Security.Claims;
using SmartRide.Application.DTOs.Lookup;
using SmartRide.Application.DTOs;
using SmartRide.Application.Queries.Users;

namespace SmartRide.UnitTests.Application.Services;

public class UserServiceTests
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IAuthService> _mockAuthService;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _mockMediator = new Mock<IMediator>();
        _mockMapper = new Mock<IMapper>();
        _mockAuthService = new Mock<IAuthService>();
        _userService = new UserService(_mockMediator.Object, _mockMapper.Object, _mockAuthService.Object);
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
        var claims = new List<Claim>
        {
            new("sub", user.UserId.ToString()),
            new("email", user.Email),
            new("phone", user.Phone),
            new("firstName", user.FirstName),
            new("lastName", user.LastName),
            new("roles", string.Join(",", user.Roles.Select(r => r.Name))),
            // new Claim("iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
        };

        _mockAuthService
            .Setup(a => a.ValidateToken(It.IsAny<ValidateTokenRequestDTO>()))
            .Returns(new ResponseDTO<List<Claim>> { Data = claims });

        _mockMapper
            .Setup(m => m.Map<GetCurrentUserRequestDTO, ValidateTokenRequestDTO>(It.IsAny<GetCurrentUserRequestDTO>()))
            .Returns(new ValidateTokenRequestDTO { AccessToken = token });

        _mockMediator
            .Setup(m => m.Send(It.IsAny<GetUserByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        var result = await _userService.GetCurrentUserAsync(new GetCurrentUserRequestDTO { AccessToken = token });

        // Assert
        Assert.NotNull(result.Data);
        Assert.Equal(user.UserId, result.Data.UserId);

        _mockAuthService.Verify(a => a.ValidateToken(It.IsAny<ValidateTokenRequestDTO>()), Times.Once);
        _mockMediator.Verify(m => m.Send(It.IsAny<GetUserByIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetCurrentUserAsync_Should_Return_Error_When_Token_Is_Invalid()
    {
        // Arrange
        var token = "invalid-token";

        _mockAuthService
            .Setup(a => a.ValidateToken(It.IsAny<ValidateTokenRequestDTO>()))
            .Returns(new ResponseDTO<List<Claim>>
            {
                Info = new ResponseInfo { Code = "INVALID_TOKEN", Message = "Token validation failed." }
            });

        _mockMapper
            .Setup(m => m.Map<GetCurrentUserRequestDTO, ValidateTokenRequestDTO>(It.IsAny<GetCurrentUserRequestDTO>()))
            .Returns(new ValidateTokenRequestDTO { AccessToken = token });

        // Act
        var result = await _userService.GetCurrentUserAsync(new GetCurrentUserRequestDTO { AccessToken = token });

        // Assert
        Assert.Null(result.Data);
        Assert.NotNull(result.Info);
        Assert.Equal("INVALID_TOKEN", result.Info?.Code);
        Assert.Equal("Token validation failed.", result.Info?.Message);

        _mockAuthService.Verify(a => a.ValidateToken(It.IsAny<ValidateTokenRequestDTO>()), Times.Once);
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
