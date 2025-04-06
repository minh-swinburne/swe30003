using MediatR;
using SmartRide.Application.Commands;
using SmartRide.Application.Commands.Vehicles;
using SmartRide.Application.DTOs;
using SmartRide.Application.DTOs.Vehicles;
using SmartRide.Application.Factories;
using SmartRide.Application.Interfaces;
using SmartRide.Application.Queries.Vehicles;

namespace SmartRide.Application.Services;

public class VehicleService(IMediator mediator) : IVehicleService
{
    private readonly IMediator _mediator = mediator;

    public async Task<ListResponseDTO<ListVehicleResponseDTO>> GetAllVehiclesAsync(ListVehicleRequestDTO request)
    {
        var query = MediatRFactory.CreateQuery<ListVehicleQuery>(request);
        var result = await _mediator.Send(query);
        return new ListResponseDTO<ListVehicleResponseDTO>
        {
            Data = result,
            Count = result.Count
        };
    }

    public async Task<ResponseDTO<GetVehicleResponseDTO>> GetVehicleByIdAsync(GetVehicleByIdRequestDTO request)
    {
        var query = MediatRFactory.CreateQuery<GetVehicleByIdQuery>(request);
        var result = await _mediator.Send(query);
        return new ResponseDTO<GetVehicleResponseDTO> { Data = result };
    }

    public async Task<ResponseDTO<GetVehicleResponseDTO>> GetVehicleByVinAsync(GetVehicleByVinRequestDTO request)
    {
        var query = MediatRFactory.CreateQuery<GetVehicleByVinQuery>(request);
        var result = await _mediator.Send(query);
        return new ResponseDTO<GetVehicleResponseDTO> { Data = result };
    }

    public async Task<ResponseDTO<GetVehicleResponseDTO>> GetVehicleByPlateAsync(GetVehicleByPlateRequestDTO request)
    {
        var query = MediatRFactory.CreateQuery<GetVehicleByPlateQuery>(request);
        var result = await _mediator.Send(query);
        return new ResponseDTO<GetVehicleResponseDTO> { Data = result };
    }

    public async Task<ResponseDTO<CreateVehicleResponseDTO>> CreateVehicleAsync(CreateVehicleRequestDTO request)
    {
        var command = MediatRFactory.CreateCommand<CreateVehicleCommand>(request);
        var result = await _mediator.Send(command);
        await _mediator.Send(new SaveChangesCommand());
        return new ResponseDTO<CreateVehicleResponseDTO> { Data = result };
    }

    public async Task<ResponseDTO<UpdateVehicleResponseDTO>> UpdateVehicleAsync(UpdateVehicleRequestDTO request)
    {
        var command = MediatRFactory.CreateCommand<UpdateVehicleCommand>(request);
        var result = await _mediator.Send(command);
        await _mediator.Send(new SaveChangesCommand());
        return new ResponseDTO<UpdateVehicleResponseDTO> { Data = result };
    }

    public async Task<ResponseDTO<DeleteVehicleResponseDTO>> DeleteVehicleAsync(DeleteVehicleRequestDTO request)
    {
        var command = MediatRFactory.CreateCommand<DeleteVehicleCommand>(request);
        var result = await _mediator.Send(command);
        await _mediator.Send(new SaveChangesCommand());
        return new ResponseDTO<DeleteVehicleResponseDTO> { Data = result };
    }
}
