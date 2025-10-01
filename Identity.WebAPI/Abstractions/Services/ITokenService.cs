using Identity.WebAPI.Models.DTOs.Output;
using Identity.WebAPI.Models.Entities;

namespace Identity.WebAPI.Abstractions.Services;

public interface ITokenService
{
    Task<TokenResponse> GenerateTokensAsync(ApplicationUser user);
    Task<TokenResponse> RefreshTokenAsync(string accessToken, string refreshToken);
}
