using RoomReservationBackend.DTOs;

namespace RoomReservationBackend.Services
{
    public interface IUserService
    {
        Task<UserDto?> CreateUserAsync(CreateUserDto createUserDto);
        Task<UserDto?> GetUserByIdAsync(int id);
        Task<UserDto?> AuthenticateAsync(LoginDto loginDto);
        Task<UserDto?> UpdateUserAsync(int id, UpdateUserDto updateUserDto);
    }
}