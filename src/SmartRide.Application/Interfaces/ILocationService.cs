using SmartRide.Application.DTOs;
using SmartRide.Application.DTOs.Locations;

namespace SmartRide.Application.Interfaces;

public interface ILocationService
{
    Task<ListResponseDTO<ListLocationResponseDTO>> ListLocationsAsync(ListLocationRequestDTO request);
    Task<ResponseDTO<GetLocationResponseDTO>> GetLocationByIdAsync(GetLocationByIdRequestDTO request);
    Task<ResponseDTO<CreateLocationResponseDTO>> CreateLocationAsync(CreateLocationRequestDTO request);
    Task<ResponseDTO<UpdateLocationResponseDTO>> UpdateLocationAsync(UpdateLocationRequestDTO request);
    Task<ResponseDTO<DeleteLocationResponseDTO>> DeleteLocationAsync(DeleteLocationRequestDTO request);
    Task<GetLocationResponseDTO> GetOrCreateLocationAsync(string? address, double? latitude, double? longitude);
}
