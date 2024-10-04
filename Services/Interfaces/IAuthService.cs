using BB.APIs.BeardBoss.Monolithic.DTOs.Auth;

namespace BB.APIs.BeardBoss.Monolithic.Services.Interfaces
{
    public interface IAuthService
    {
        Task<JwtTokenResult> RegisterUserAsync(RegisterUserDto registerUserDto);
        Task<JwtTokenResult> LoginUserAsync(LoginUserDto loginUserDto);
    }
}
