using MediatR;
using SmartRide.Application.Commands;

namespace SmartRide.Application.Handlers;

public abstract class BaseCommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : BaseCommand<TResponse>
    where TResponse : class
{
    public abstract Task<TResponse> Handle(TCommand request, CancellationToken cancellationToken);
}
