using Microsoft.AspNetCore.Identity;
using Shared.DTO;

namespace Domain.Contracts
{
    public interface IAuthenticationManager
    {
        Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration);
        Task<TokenDto?> ValidateAndCreateToken(UserForAuthenticationDto userForAuth);
        Task<TokenDto> RefreshToken(TokenDto tokenDto);
    }
}