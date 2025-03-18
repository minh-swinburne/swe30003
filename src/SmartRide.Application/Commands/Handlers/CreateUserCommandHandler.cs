using AutoMapper;
using MediatR;
using SmartRide.Application.DTOs;
using SmartRide.Application.DTOs.Users;
using SmartRide.Domain.Entities;
using SmartRide.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRide.Application.Commands.Handlers;

public class CreateUserCommandHandler(IRepository<User> userRepository, IMapper mapper) : IRequestHandler<CreateUserCommand, ResponseDTO<CreateUserResponseDTO>>
{
    private readonly IRepository<User> _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ResponseDTO<CreateUserResponseDTO>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<User>(request);
        //user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
        //await _userRepository.AddAsync(user, cancellationToken);
        return new ResponseDTO<CreateUserResponseDTO>
        {
            Data = _mapper.Map<CreateUserResponseDTO>(user),
            Message = "User created successfully."
        };
    }
}
