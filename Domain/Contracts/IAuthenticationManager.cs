using Microsoft.AspNetCore.Identity;
using Shared.DTO;

namespace Domain.Contracts
{
    public interface IAuthenticationManager
    {
        Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration);
        Task<bool> ValidateUser(UserForAuthenticationDto userForAuth);
        Task<string> CreateToken();
    }
}