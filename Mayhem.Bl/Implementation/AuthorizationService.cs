using Mayhem.Configuration;
using Mayhem.Bl.Interfaces;
using Mayhem.Dal.Dto.Responses;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Mayhem.Bl.Implementation
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly MayhemConfiguration mayhemConfiguration;

        public AuthorizationService(MayhemConfiguration mayhemConfiguration)
        {
            this.mayhemConfiguration = mayhemConfiguration;
        }

        public async Task<JWTTokenResponse> GetAuthorizationToken()
        {
            SymmetricSecurityKey secretKey = new(Encoding.UTF8.GetBytes(mayhemConfiguration.JWTSecret));
            SigningCredentials signinCredentials = new(secretKey, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken tokeOptions = new(
                issuer: mayhemConfiguration.JWTValidIssuer,
                audience: mayhemConfiguration.JWTValidAudience,
                claims: new List<Claim>(),
                expires: DateTime.UtcNow.AddMinutes(mayhemConfiguration.JWTDurationInMinutes),
                signingCredentials: signinCredentials
            );
            string tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return new JWTTokenResponse { Token = tokenString };
        }
    }
}
