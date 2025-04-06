using MediatR;
using SmartRide.Application.Commands;

namespace SmartRide.Infrastructure.Persistence;

public class SaveChangesCommandHandler(SmartRideDbContext dbContext) : IRequestHandler<SaveChangesCommand>
{
    private readonly SmartRideDbContext _dbContext = dbContext;

    public async Task Handle(SaveChangesCommand request, CancellationToken cancellationToken)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
