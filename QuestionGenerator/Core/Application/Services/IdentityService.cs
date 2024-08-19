using Microsoft.IdentityModel.Tokens;
using QuestionGenerator.Core.Application.Interfaces.Services;
using QuestionGenerator.Models.UserModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QuestionGenerator.Core.Application.Services
{
    public class IdentityService : IIdentityService
    {
        public string GenerateAccessToken(string key, string issuer, string audience, UserResponse user)
        {

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.FullName), // Full name
                new Claim(JwtRegisteredClaimNames.Email, user.Email), // Email
                new Claim("Profile Picture", user.ProfilePictureUrl ?? ""), // Profile picture URL
                new Claim("Role", user.RoleName), // Role
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // JWT ID
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()), // Issued at
                new Claim(JwtRegisteredClaimNames.Exp, DateTime.UtcNow.AddMinutes(15).ToString()), // Expiration
                new Claim(JwtRegisteredClaimNames.Aud, audience), // Audience
                new Claim(JwtRegisteredClaimNames.Iss, issuer) // Issuer
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new JwtSecurityToken(issuer, audience, claims,
                expires: DateTime.UtcNow.AddMinutes(15), signingCredentials: credentials);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
            return token;
        }

        public bool IsTokenValid(string key, string issuer, string audience, string token)
        {
            var mySecret = Encoding.UTF8.GetBytes(key);
            var mySecurityKey = new SymmetricSecurityKey(mySecret);

            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = mySecurityKey
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static int GetLoginId(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            // Retrieve the ID from the claims
            var idClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (idClaim != null)
            {
                string userId = idClaim.Value;

                // Use the userId as needed
                return int.Parse(userId);
            }
            else
            {
                // Handle the case when the ID claim is not present in the token
                return 0;
            }
        }

        public string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
