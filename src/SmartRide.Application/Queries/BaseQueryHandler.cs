using MediatR;

namespace SmartRide.Application.Queries;

public abstract class BaseQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : BaseQuery<TResponse>
    where TResponse : class
{
    public abstract Task<TResponse> Handle(TQuery request, CancellationToken cancellationToken);
}
