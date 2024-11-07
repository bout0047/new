namespace RoomReservationBackend.DTOs
{
    public class RoomDto
    {
        public int RoomId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string Location { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateRoomDto
    {
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string Location { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class UpdateRoomDto
    {
        public string? Name { get; set; }
        public int? Capacity { get; set; }
        public string? Location { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
    }
}