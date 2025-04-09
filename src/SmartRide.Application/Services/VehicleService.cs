using MediatR;
using SmartRide.Application.Commands;
using SmartRide.Application.Commands.Vehicles;
using SmartRide.Application.DTOs;
using SmartRide.Application.DTOs.Vehicles;
using SmartRide.Application.Factories;
using SmartRide.Application.Interfaces;
using SmartRide.Application.Queries.Vehicles;
using SmartRide.Common.Responses;

namespace SmartRide.Application.Services;

public class VehicleService(IMediator mediator) : IVehicleService
{
    private readonly IMediator _mediator = mediator;

    public async Task<ListResponseDTO<ListVehicleResponseDTO>> ListVehiclesAsync(ListVehicleRequestDTO request)
    {
        try
        {
            var query = MediatRFactory.CreateQuery<ListVehicleQuery>(request);
            var result = await _mediator.Send(query);
            return new ListResponseDTO<ListVehicleResponseDTO>
            {
                Data = result,
                Count = result.Count
            };
        }
        catch (Exception ex)
        {
            return new ListResponseDTO<ListVehicleResponseDTO>
            {
                Info = new ResponseInfo { Code = "LIST_VEHICLES_ERROR", Message = ex.Message }
            };
        }
    }

    public async Task<ResponseDTO<GetVehicleResponseDTO>> GetVehicleByIdAsync(GetVehicleByIdRequestDTO request)
    {
        try
        {
            var query = MediatRFactory.CreateQuery<GetVehicleByIdQuery>(request);
            var result = await _mediator.Send(query);
            return new ResponseDTO<GetVehicleResponseDTO> { Data = result };
        }
        catch (Exception ex)
        {
            return new ResponseDTO<GetVehicleResponseDTO>
            {
                Info = new ResponseInfo { Code = "GET_VEHICLE_BY_ID_ERROR", Message = ex.Message }
            };
        }
    }

    public async Task<ResponseDTO<GetVehicleResponseDTO>> GetVehicleByVinAsync(GetVehicleByVinRequestDTO request)
    {
        try
        {
            var query = MediatRFactory.CreateQuery<GetVehicleByVinQuery>(request);
            var result = await _mediator.Send(query);
            return new ResponseDTO<GetVehicleResponseDTO> { Data = result };
        }
        catch (Exception ex)
        {
            return new ResponseDTO<GetVehicleResponseDTO>
            {
                Info = new ResponseInfo { Code = "GET_VEHICLE_BY_VIN_ERROR", Message = ex.Message }
            };
        }
    }

    public async Task<ResponseDTO<GetVehicleResponseDTO>> GetVehicleByPlateAsync(GetVehicleByPlateRequestDTO request)
    {
        try
        {
            var query = MediatRFactory.CreateQuery<GetVehicleByPlateQuery>(request);
            var result = await _mediator.Send(query);
            return new ResponseDTO<GetVehicleResponseDTO> { Data = result };
        }
        catch (Exception ex)
        {
            return new ResponseDTO<GetVehicleResponseDTO>
            {
                Info = new ResponseInfo { Code = "GET_VEHICLE_BY_PLATE_ERROR", Message = ex.Message }
            };
        }
    }

    public async Task<ResponseDTO<CreateVehicleResponseDTO>> CreateVehicleAsync(CreateVehicleRequestDTO request)
    {
        try
        {
            var command = MediatRFactory.CreateCommand<CreateVehicleCommand>(request);
            var result = await _mediator.Send(command);
            await _mediator.Send(new SaveChangesCommand());
            return new ResponseDTO<CreateVehicleResponseDTO> { Data = result };
        }
        catch (Exception ex)
        {
            return new ResponseDTO<CreateVehicleResponseDTO>
            {
                Info = new ResponseInfo { Code = "CREATE_VEHICLE_ERROR", Message = ex.Message }
            };
        }
    }

    public async Task<ResponseDTO<UpdateVehicleResponseDTO>> UpdateVehicleAsync(UpdateVehicleRequestDTO request)
    {
        try
        {
            var command = MediatRFactory.CreateCommand<UpdateVehicleCommand>(request);
            var result = await _mediator.Send(command);
            await _mediator.Send(new SaveChangesCommand());
            return new ResponseDTO<UpdateVehicleResponseDTO> { Data = result };
        }
        catch (Exception ex)
        {
            return new ResponseDTO<UpdateVehicleResponseDTO>
            {
                Info = new ResponseInfo { Code = "UPDATE_VEHICLE_ERROR", Message = ex.Message }
            };
        }
    }

    public async Task<ResponseDTO<DeleteVehicleResponseDTO>> DeleteVehicleAsync(DeleteVehicleRequestDTO request)
    {
        try
        {
            var command = MediatRFactory.CreateCommand<DeleteVehicleCommand>(request);
            var result = await _mediator.Send(command);
            await _mediator.Send(new SaveChangesCommand());
            return new ResponseDTO<DeleteVehicleResponseDTO> { Data = result };
        }
        catch (Exception ex)
        {
            return new ResponseDTO<DeleteVehicleResponseDTO>
            {
                Info = new ResponseInfo { Code = "DELETE_VEHICLE_ERROR", Message = ex.Message }
            };
        }
    }
}
