using RoomReservationBackend.DTOs;
using RoomReservationBackend.Models;
using RoomReservationBackend.Repositories;

namespace RoomReservationBackend.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;

        public RoomService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<IEnumerable<RoomDto>> GetAllRoomsAsync()
        {
            var rooms = await _roomRepository.GetAllRoomsAsync();
            return rooms.Select(MapToDto);
        }

        public async Task<RoomDto?> GetRoomByIdAsync(int id)
        {
            var room = await _roomRepository.GetRoomByIdAsync(id);
            return room != null ? MapToDto(room) : null;
        }

        public async Task<RoomDto> CreateRoomAsync(CreateRoomDto createRoomDto)
        {
            var room = new Room
            {
                Name = createRoomDto.Name,
                Capacity = createRoomDto.Capacity,
                Location = createRoomDto.Location,
                Description = createRoomDto.Description,
                IsActive = true
            };

            await _roomRepository.CreateRoomAsync(room);
            return MapToDto(room);
        }

        public async Task<RoomDto?> UpdateRoomAsync(int id, UpdateRoomDto updateRoomDto)
        {
            var room = await _roomRepository.GetRoomByIdAsync(id);
            if (room == null)
            {
                return null;
            }

            if (updateRoomDto.Name != null)
                room.Name = updateRoomDto.Name;
            
            if (updateRoomDto.Capacity.HasValue)
                room.Capacity = updateRoomDto.Capacity.Value;
            
            if (updateRoomDto.Location != null)
                room.Location = updateRoomDto.Location;
            
            if (updateRoomDto.Description != null)
                room.Description = updateRoomDto.Description;
            
            if (updateRoomDto.IsActive.HasValue)
                room.IsActive = updateRoomDto.IsActive.Value;

            await _roomRepository.UpdateRoomAsync(room);
            return MapToDto(room);
        }

        public async Task<bool> DeleteRoomAsync(int id)
        {
            return await _roomRepository.DeleteRoomAsync(id);
        }

        private static RoomDto MapToDto(Room room)
        {
            return new RoomDto
            {
                RoomId = room.RoomId,
                Name = room.Name,
                Capacity = room.Capacity,
                Location = room.Location,
                Description = room.Description,
                IsActive = room.IsActive
            };
        }
    }
}