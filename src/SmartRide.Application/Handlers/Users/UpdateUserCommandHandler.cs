using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SmartRide.Application.Commands;
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
        var user = await _userRepository.GetByIdAsync(command.UserId, cancellationToken)
            ?? throw new BaseException(UserErrors.Module, UserErrors.ID_NOT_FOUND.FormatMessage(("UserId", command.UserId)));

        // Update required fields only if they are not null and not empty
        if (!string.IsNullOrWhiteSpace(command.FirstName)) user.FirstName = command.FirstName;
        if (!string.IsNullOrWhiteSpace(command.Email)) user.Email = command.Email;
        if (!string.IsNullOrWhiteSpace(command.Phone)) user.Phone = command.Phone;

        // Update optional fields (allow clearing by setting to empty string)
        if (command.LastName != null) user.LastName = command.LastName;
        if (command.Picture != null) user.Picture = command.Picture;

        // Update password if provided
        if (!string.IsNullOrWhiteSpace(command.Password))
        {
            user.Password = _passwordHasher.HashPassword(user, command.Password);
        }

        var updatedUser = await _userRepository.UpdateAsync(user, cancellationToken);

        return _mapper.Map<UpdateUserResponseDTO>(updatedUser);
    }
}
