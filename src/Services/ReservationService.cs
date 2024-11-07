using RoomReservationBackend.DTOs;
using RoomReservationBackend.Models;
using RoomReservationBackend.Repositories;

namespace RoomReservationBackend.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IUserRepository _userRepository;

        public ReservationService(
            IReservationRepository reservationRepository,
            IRoomRepository roomRepository,
            IUserRepository userRepository)
        {
            _reservationRepository = reservationRepository;
            _roomRepository = roomRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<ReservationDto>> GetReservationsAsync(
            int userId,
            DateTime? startDate,
            DateTime? endDate,
            int? roomId)
        {
            var reservations = await _reservationRepository.GetReservationsAsync(userId, startDate, endDate, roomId);
            return reservations.Select(MapToDto);
        }

        public async Task<ReservationDto?> GetReservationByIdAsync(int id, int userId)
        {
            var reservation = await _reservationRepository.GetReservationByIdAsync(id);
            if (reservation == null || (reservation.UserId != userId && !await IsUserAdminOrManager(userId)))
            {
                return null;
            }

            return MapToDto(reservation);
        }

        public async Task<ReservationDto> CreateReservationAsync(int userId, CreateReservationDto createReservationDto)
        {
            var room = await _roomRepository.GetRoomByIdAsync(createReservationDto.RoomId);
            if (room == null)
            {
                throw new InvalidOperationException("Room not found.");
            }

            if (await HasConflictingReservation(createReservationDto.RoomId, createReservationDto.StartTime, createReservationDto.EndTime))
            {
                throw new InvalidOperationException("The room is already reserved for this time period.");
            }

            var reservation = new Reservation
            {
                RoomId = createReservationDto.RoomId,
                UserId = userId,
                StartTime = createReservationDto.StartTime,
                EndTime = createReservationDto.EndTime,
                Purpose = createReservationDto.Purpose,
                Notes = createReservationDto.Notes,
                Status = ReservationStatus.Pending
            };

            await _reservationRepository.CreateReservationAsync(reservation);
            return MapToDto(reservation);
        }

        public async Task<ReservationDto?> UpdateReservationAsync(
            int id,
            int userId,
            UpdateReservationDto updateReservationDto)
        {
            var reservation = await _reservationRepository.GetReservationByIdAsync(id);
            if (reservation == null || (reservation.UserId != userId && !await IsUserAdminOrManager(userId)))
            {
                return null;
            }

            if (updateReservationDto.StartTime.HasValue)
                reservation.StartTime = updateReservationDto.StartTime.Value;
            
            if (updateReservationDto.EndTime.HasValue)
                reservation.EndTime = updateReservationDto.EndTime.Value;
            
            if (updateReservationDto.Purpose != null)
                reservation.Purpose = updateReservationDto.Purpose;
            
            if (updateReservationDto.Notes != null)
                reservation.Notes = updateReservationDto.Notes;
            
            if (updateReservationDto.Status.HasValue)
                reservation.Status = updateReservationDto.Status.Value;

            if (await HasConflictingReservation(reservation.RoomId, reservation.StartTime, reservation.EndTime, id))
            {
                throw new InvalidOperationException("The room is already reserved for this time period.");
            }

            await _reservationRepository.UpdateReservationAsync(reservation);
            return MapToDto(reservation);
        }

        public async Task<bool> DeleteReservationAsync(int id, int userId)
        {
            var reservation = await _reservationRepository.GetReservationByIdAsync(id);
            if (reservation == null || (reservation.UserId != userId && !await IsUserAdminOrManager(userId)))
            {
                return false;
            }

            return await _reservationRepository.DeleteReservationAsync(id);
        }

        public async Task<ReservationDto?> UpdateReservationStatusAsync(int id, ReservationStatus status)
        {
            var reservation = await _reservationRepository.GetReservationByIdAsync(id);
            if (reservation == null)
            {
                return null;
            }

            reservation.Status = status;
            await _reservationRepository.UpdateReservationAsync(reservation);
            return MapToDto(reservation);
        }

        private async Task<bool> HasConflictingReservation(
            int roomId,
            DateTime startTime,
            DateTime endTime,
            int? excludeReservationId = null)
        {
            var conflictingReservations = await _reservationRepository.GetConflictingReservationsAsync(
                roomId, startTime, endTime, excludeReservationId);
            return conflictingReservations.Any();
        }

        private async Task<bool> IsUserAdminOrManager(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            return user != null && (user.Role == UserRole.Admin || user.Role == UserRole.Manager);
        }

        private static ReservationDto MapToDto(Reservation reservation)
        {
            return new ReservationDto
            {
                ReservationId = reservation.ReservationId,
                RoomId = reservation.RoomId,
                UserId = reservation.UserId,
                StartTime = reservation.StartTime,
                EndTime = reservation.EndTime,
                Purpose = reservation.Purpose,
                Notes = reservation.Notes,
                Status = reservation.Status,
                RoomName = reservation.Room?.Name ?? string.Empty,
                UserName = reservation.User?.Name ?? string.Empty
            };
        }
    }
}