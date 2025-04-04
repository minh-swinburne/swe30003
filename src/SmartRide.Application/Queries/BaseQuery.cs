using MediatR;

namespace SmartRide.Application.Queries;

public interface IQuery
{ }

public abstract class BaseQuery<TResponse> : IRequest<TResponse>, IQuery
    where TResponse : class
{ }
