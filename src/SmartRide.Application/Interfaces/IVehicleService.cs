using SmartRide.Application.DTOs;
using SmartRide.Application.DTOs.Vehicles;

namespace SmartRide.Application.Interfaces;

public interface IVehicleService
{
    Task<ListResponseDTO<ListVehicleResponseDTO>> ListVehiclesAsync(ListVehicleRequestDTO request);
    Task<ResponseDTO<GetVehicleResponseDTO>> GetVehicleByIdAsync(GetVehicleByIdRequestDTO request);
    Task<ResponseDTO<GetVehicleResponseDTO>> GetVehicleByVinAsync(GetVehicleByVinRequestDTO request);
    Task<ResponseDTO<GetVehicleResponseDTO>> GetVehicleByPlateAsync(GetVehicleByPlateRequestDTO request);
    Task<ResponseDTO<CreateVehicleResponseDTO>> CreateVehicleAsync(CreateVehicleRequestDTO request);
    Task<ResponseDTO<UpdateVehicleResponseDTO>> UpdateVehicleAsync(UpdateVehicleRequestDTO request);
    Task<ResponseDTO<DeleteVehicleResponseDTO>> DeleteVehicleAsync(DeleteVehicleRequestDTO request);
}
