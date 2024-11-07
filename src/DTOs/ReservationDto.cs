using RoomReservationBackend.Models;

namespace RoomReservationBackend.DTOs
{
    public class ReservationDto
    {
        public int ReservationId { get; set; }
        public int RoomId { get; set; }
        public int UserId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Purpose { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public ReservationStatus Status { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
    }

    public class CreateReservationDto
    {
        public int RoomId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Purpose { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }

    public class UpdateReservationDto
    {
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Purpose { get; set; }
        public string? Notes { get; set; }
        public ReservationStatus? Status { get; set; }
    }
}