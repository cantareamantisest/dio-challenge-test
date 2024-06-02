using Desafio.Dio.Identity.Models;

namespace Desafio.Dio.Identity.Services
{
    public interface IIdentityService
    {
        Task<RegisterResponse> Register(RegisterRequest request);

        Task<LoginResponse> Login(LoginRequest request);

        Task<LoginResponse> LoginWithoutPassword(string userId);
    }
}