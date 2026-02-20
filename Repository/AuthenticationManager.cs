using Domain.Configuration;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Repository
{
    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtConfiguration _jwtConfiguration;
        private User? _user;

        public AuthenticationManager(UserManager<User> userManager,
             IOptions<JwtConfiguration> configuration)
        {
            _userManager = userManager;
            _jwtConfiguration = configuration.Value;
        }


        public async Task<TokenDto> CreateToken(bool populateExp)
        {
            var secretKey = Environment.GetEnvironmentVariable("SECRET");

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, _user.UserName),
                new Claim(ClaimTypes.NameIdentifier, _user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var token = new JwtSecurityToken(
                issuer: _jwtConfiguration.ValidIssuer,
                audience: _jwtConfiguration.ValidAudience,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtConfiguration.Expires)),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            var refreshToken = GenerateRefreshToken();

            _user.RefreshToken = refreshToken;

            if (populateExp)
                _user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

            await _userManager.UpdateAsync(_user);

            return new TokenDto(accessToken, refreshToken);
        }

        public async Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration)
        {
            var user = new User
            {
                UserName = userForRegistration.UserName,
                Email = userForRegistration.Email,
                FirstName = userForRegistration.FirstName,
                LastName = userForRegistration.LastName,
                PhoneNumber = userForRegistration.PhoneNumber
            };

            return await _userManager.CreateAsync(user, userForRegistration.Password);

        }

        public async Task<bool> ValidateUser(UserForAuthenticationDto userForAuth)
        {
            _user = await _userManager.FindByNameAsync(userForAuth.UserName);

            return (_user != null && await _userManager.CheckPasswordAsync(_user, userForAuth.Password));
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var secret = Environment.GetEnvironmentVariable("SECRET");

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                ValidateLifetime = false,
            };

            var tokenHendler = new JwtSecurityTokenHandler();
            var principal = tokenHendler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        public async Task<TokenDto> RefreshToken(TokenDto tokenDto)
        {
            var principal = GetPrincipalFromExpiredToken(tokenDto.AccessToken);
            var username = principal.Identity.Name;

            var user = await _userManager.FindByNameAsync(username);

            if (user == null || user.RefreshToken != tokenDto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                throw new RefreshTokenBadRequestException();
            }

            _user = user;

            return await CreateToken(populateExp: false);
        }
    }
}
