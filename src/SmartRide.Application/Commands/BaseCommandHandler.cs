using MediatR;

namespace SmartRide.Application.Commands;

public abstract class BaseCommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : BaseCommand<TResponse>
    where TResponse : class
{
    public abstract Task<TResponse> Handle(TCommand request, CancellationToken cancellationToken);
}
