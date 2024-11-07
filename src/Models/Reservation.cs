using System;
using System.ComponentModel.DataAnnotations;

namespace RoomReservationBackend.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; }

        [Required]
        public int RoomId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        [StringLength(200)]
        public string Purpose { get; set; } = string.Empty;

        public string? Notes { get; set; }

        public ReservationStatus Status { get; set; } = ReservationStatus.Pending;

        public Room? Room { get; set; }
        public User? User { get; set; }
    }

    public enum ReservationStatus
    {
        Pending,
        Confirmed,
        Cancelled
    }
}