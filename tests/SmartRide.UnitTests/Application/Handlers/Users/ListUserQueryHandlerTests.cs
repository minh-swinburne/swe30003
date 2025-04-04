﻿using AutoMapper;
using Moq;
using SmartRide.Application.DTOs.Lookup;
using SmartRide.Application.DTOs.Users;
using SmartRide.Application.Queries.Users;
using SmartRide.Application.Queries.Users.Handlers;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;
using System.Linq.Expressions;

namespace SmartRide.UnitTests.Application.Handlers.Users;

public class ListUserQueryHandlerTests
{
    private readonly Mock<IRepository<User>> _mockUserRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly ListUserQueryHandler _handler;

    public ListUserQueryHandlerTests()
    {
        _mockUserRepository = new Mock<IRepository<User>>();
        _mockMapper = new Mock<IMapper>();
        _handler = new ListUserQueryHandler(_mockUserRepository.Object, _mockMapper.Object);
    }

    private List<User> CreateMockUsers()
    {
        return new List<User>
        {
            new() { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe", Email = "johndoe@example.com", Phone = "0123456789", Password = "secure-password-1" },
            new() { Id = Guid.NewGuid(), FirstName = "Jane", LastName = "Smith", Email = "jsmith123@example.xyz", Phone = "0987654321", Password = "secure-password-2", Picture = "https://example.com/images/avatar_1.png" }
        };
    }

    private List<ListUserResponseDTO> MapToResponseDTOs(List<User> users)
    {
        return users.Select(x => new ListUserResponseDTO
        {
            UserId = x.Id,
            FirstName = x.FirstName,
            LastName = x.LastName,
            Email = x.Email,
            Phone = x.Phone,
            Picture = x.Picture,
            Roles = x.Roles.Select(r => new RoleDTO
            {
                RoleId = (byte)r.Id,
                Name = r.Name,
                Description = r.Description
            }).ToList()
        }).ToList();
    }

    private void SetupMocks(List<User> users, List<ListUserResponseDTO> expected)
    {
        _mockUserRepository.Setup(x => x.GetWithFilterAsync(
            It.IsAny<Expression<Func<User, bool>>>(),
            It.IsAny<Expression<Func<User, ListUserResponseDTO>>>(),
            It.IsAny<Expression<Func<User, object>>>(),
            It.IsAny<bool>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(users);

        _mockMapper.Setup(x => x.Map<List<ListUserResponseDTO>>(It.IsAny<List<User>>())).Returns(expected);
    }

    [Fact]
    public async Task Handle_Should_Return_Users_With_Correct_Count_And_Ids()
    {
        // Arrange
        var query = new ListUserQuery();
        var users = CreateMockUsers();
        var expected = MapToResponseDTOs(users);
        SetupMocks(users, expected);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(users.First().Id, result.First().UserId);
        Assert.Equal(users.Last().Id, result.Last().UserId);
    }

    [Fact]
    public async Task Handle_Should_Return_Users_Without_Password()
    {
        // Arrange
        var query = new ListUserQuery();
        var users = CreateMockUsers();
        var expected = MapToResponseDTOs(users);
        SetupMocks(users, expected);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.DoesNotContain(result, x => x.GetType().GetProperty("Password") != null);
    }
}
