using MediatR;

namespace SmartRide.Application.Commands;

public interface ICommand
{ }

public abstract class BaseCommand<TResponse> : IRequest<TResponse>, ICommand
    where TResponse : class
{ }
