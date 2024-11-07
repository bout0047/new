using RoomReservationBackend.DTOs;
using RoomReservationBackend.Models;

namespace RoomReservationBackend.Services
{
    public interface IReservationService
    {
        Task<IEnumerable<ReservationDto>> GetReservationsAsync(int userId, DateTime? startDate, DateTime? endDate, int? roomId);
        Task<ReservationDto?> GetReservationByIdAsync(int id, int userId);
        Task<ReservationDto> CreateReservationAsync(int userId, CreateReservationDto createReservationDto);
        Task<ReservationDto?> UpdateReservationAsync(int id, int userId, UpdateReservationDto updateReservationDto);
        Task<bool> DeleteReservationAsync(int id, int userId);
        Task<ReservationDto?> UpdateReservationStatusAsync(int id, ReservationStatus status);
    }
}