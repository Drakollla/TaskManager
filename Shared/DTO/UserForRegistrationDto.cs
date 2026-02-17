using System.ComponentModel.DataAnnotations;

namespace Shared.DTO
{
    public record UserForRegistrationDto(
        string FirstName,
        string LastName,
        
        [Required] 
        string UserName,
        
        [Required] 
        string Password,
        
        [Required]
        string Email,
        
        string? PhoneNumber
     );
}