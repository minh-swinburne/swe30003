using AutoMapper;
using MediatR;
using SmartRide.Application.DTOs.Users;
using SmartRide.Domain.Entities;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Application.Commands.Handlers;

public class CreateUserCommandHandler(IRepository<User> userRepository, IMapper mapper) : IRequestHandler<CreateUserCommand, CreateUserResponseDTO>
{
    private readonly IRepository<User> _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<CreateUserResponseDTO> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<User>(request);
        //user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
        //await _userRepository.AddAsync(user, cancellationToken);
        return _mapper.Map<CreateUserResponseDTO>(user);
    }
}
