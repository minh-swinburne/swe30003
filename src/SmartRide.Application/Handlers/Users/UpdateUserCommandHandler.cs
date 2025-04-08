using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SmartRide.Application.Commands.Users;
using SmartRide.Application.DTOs.Users;
using SmartRide.Common.Exceptions;
using SmartRide.Common.Responses.Errors;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Application.Handlers.Users;

public class UpdateUserCommandHandler(IRepository<User> userRepository, IPasswordHasher<User> passwordHasher, IMapper mapper)
    : BaseCommandHandler<UpdateUserCommand, UpdateUserResponseDTO>
{
    private readonly IRepository<User> _userRepository = userRepository;
    private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;
    private readonly IMapper _mapper = mapper;

    public override async Task<UpdateUserResponseDTO> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(command.UserId, cancellationToken: cancellationToken)
            ?? throw new BaseException(UserErrors.Module, UserErrors.ID_NOT_FOUND.FormatMessage(("UserId", command.UserId)));

        _mapper.Map(command, user);

        if (!string.IsNullOrWhiteSpace(command.Password))
        {
            user.Password = _passwordHasher.HashPassword(user, command.Password);
        }

        var updatedUser = await _userRepository.UpdateAsync(user, cancellationToken);

        return _mapper.Map<UpdateUserResponseDTO>(updatedUser);
    }
}
