using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartRide.Application.DTOs.Users;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Application.Queries.Users.Handlers;

public class GetUserByEmailQueryHandler(IRepository<User> userRepository, IMapper mapper)
    : BaseQueryHandler<GetUserByEmailQuery, GetUserResponseDTO>
{
    private readonly IRepository<User> _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;

    public override async Task<GetUserResponseDTO> Handle(GetUserByEmailQuery query, CancellationToken cancellationToken)
    {
        var user = await _userRepository.Query(cancellationToken)
            .FirstOrDefaultAsync(u => u.Email == query.Email, cancellationToken);

        return _mapper.Map<GetUserResponseDTO>(user);
    }
}
