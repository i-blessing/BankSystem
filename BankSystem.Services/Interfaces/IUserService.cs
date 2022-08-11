using BankSystem.Business.Dto;

namespace BankSystem.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> GetUserForTokenAsync(string token);
        Task<LoginResponse> LoginAsync(LoginRequest request);
    }
}