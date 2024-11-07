using System;
using System.ComponentModel.DataAnnotations;

namespace RoomReservationBackend.Models
{
    public class Room
    {
        public int RoomId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public int Capacity { get; set; }

        [Required]
        [StringLength(200)]
        public string Location { get; set; } = string.Empty;

        public string? Description { get; set; }
        
        public bool IsActive { get; set; } = true;

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}