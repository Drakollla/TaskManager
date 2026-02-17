using System.ComponentModel.DataAnnotations;

namespace Shared.DTO
{
    public record UserForAuthenticationDto(
        [Required]
        string UserName,

        [Required]
        string Password
    );
}