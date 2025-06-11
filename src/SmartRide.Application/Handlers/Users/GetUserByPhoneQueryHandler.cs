using AutoMapper;
using SmartRide.Application.DTOs.Users;
using SmartRide.Application.Queries.Users;
using SmartRide.Domain.Entities.Base;
using SmartRide.Domain.Interfaces;

namespace SmartRide.Application.Handlers.Users;

public class GetUserByPhoneQueryHandler(IRepository<User> userRepository, IMapper mapper)
    : BaseQueryHandler<GetUserByPhoneQuery, GetUserResponseDTO>
{
    private readonly IRepository<User> _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;

    public override async Task<GetUserResponseDTO> Handle(GetUserByPhoneQuery query, CancellationToken cancellationToken)
    {
        var user = await _userRepository
            .Query(filter: u => u.Phone == query.Phone, includes: [u => u.Roles], cancellationToken);

        return _mapper.Map<GetUserResponseDTO>(user);
    }
}
