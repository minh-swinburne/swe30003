using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SmartRide.Application.DTOs.Users;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Application.Commands.Users.Handlers;

public class CreateUserCommandHandler(IRepository<User> userRepository, IPasswordHasher<User> passwordHasher, IMapper mapper)
    : BaseCommandHandler<CreateUserCommand, CreateUserResponseDTO>
{
    private readonly IRepository<User> _userRepository = userRepository;
    private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;
    private readonly IMapper _mapper = mapper;

    public override async Task<CreateUserResponseDTO> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        // Map the command to the User entity
        var user = _mapper.Map<User>(command);

        // Hash the password
        user.Password = _passwordHasher.HashPassword(user, command.Password);

        // Add user to the database
        var createdUser = await _userRepository.CreateAsync(user, cancellationToken);

        // Map the created user back to the response DTO
        return _mapper.Map<CreateUserResponseDTO>(createdUser);
    }
}
