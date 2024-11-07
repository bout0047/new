using System.Security.Cryptography;
using System.Text;
using RoomReservationBackend.DTOs;
using RoomReservationBackend.Models;
using RoomReservationBackend.Repositories;

namespace RoomReservationBackend.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto?> CreateUserAsync(CreateUserDto createUserDto)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(createUserDto.Email);
            if (existingUser != null)
            {
                return null;
            }

            var user = new User
            {
                Name = createUserDto.Name,
                Email = createUserDto.Email,
                Role = createUserDto.Role,
                PasswordHash = HashPassword(createUserDto.Password)
            };

            await _userRepository.CreateUserAsync(user);
            return MapToDto(user);
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            return user != null ? MapToDto(user) : null;
        }

        public async Task<UserDto?> AuthenticateAsync(LoginDto loginDto)
        {
            var user = await _userRepository.GetUserByEmailAsync(loginDto.Email);
            if (user != null && VerifyPassword(loginDto.Password, user.PasswordHash))
            {
                return MapToDto(user);
            }
            return null;
        }

        public async Task<UserDto?> UpdateUserAsync(int id, UpdateUserDto updateUserDto)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return null;
            }

            if (updateUserDto.Name != null)
                user.Name = updateUserDto.Name;
            
            if (updateUserDto.Email != null)
                user.Email = updateUserDto.Email;
            
            if (updateUserDto.Password != null)
                user.PasswordHash = HashPassword(updateUserDto.Password);
            
            if (updateUserDto.Role.HasValue)
                user.Role = updateUserDto.Role.Value;
            
            if (updateUserDto.IsActive.HasValue)
                user.IsActive = updateUserDto.IsActive.Value;

            await _userRepository.UpdateUserAsync(user);
            return MapToDto(user);
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerifyPassword(string inputPassword, string storedHash)
        {
            var hashedInput = HashPassword(inputPassword);
            return hashedInput == storedHash;
        }

        private static UserDto MapToDto(User user)
        {
            return new UserDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                IsActive = user.IsActive
            };
        }
    }
}