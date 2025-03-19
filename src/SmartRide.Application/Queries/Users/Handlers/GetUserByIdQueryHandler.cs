using AutoMapper;
using MediatR;
using SmartRide.Application.DTOs.Users;
using SmartRide.Domain.Entities;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Application.Queries.Users.Handlers;

public class GetUserByIdQueryHandler(IRepository<User> userRepository, IMapper mapper)
    : IRequestHandler<GetUserByIdQuery, GetUserByIdResponseDTO>
{
    private readonly IRepository<User> _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<GetUserByIdResponseDTO> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
        return _mapper.Map<GetUserByIdResponseDTO>(user);
    }
}
