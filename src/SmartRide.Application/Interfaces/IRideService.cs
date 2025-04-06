using SmartRide.Application.DTOs;
using SmartRide.Application.DTOs.Rides;

namespace SmartRide.Application.Interfaces;

public interface IRideService
{
    Task<ListResponseDTO<ListRideResponseDTO>> GetAllRidesAsync(ListRideRequestDTO request);
    Task<ResponseDTO<GetRideResponseDTO>> GetRideByIdAsync(GetRideByIdRequestDTO request);
    Task<ResponseDTO<CreateRideResponseDTO>> CreateRideAsync(CreateRideRequestDTO request);
    Task<ResponseDTO<UpdateRideResponseDTO>> UpdateRideAsync(UpdateRideRequestDTO request);
    Task<ResponseDTO<DeleteRideResponseDTO>> DeleteRideAsync(DeleteRideRequestDTO request);
}
