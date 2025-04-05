using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services.Contract;

namespace Talabat.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> userManager)
        {
            //Private claims (user-defined)
            var authClaims = new List<Claim>
            {
              new Claim(ClaimTypes.GivenName, user.UserName),
              new Claim(ClaimTypes.Email, user.Email)
            };

            //Roles
            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
                authClaims.Add(new Claim(ClaimTypes.Role, role));

            //Secret Key
            var authkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

            var token = new JwtSecurityToken(
                  audience: _configuration["JWT:ValidAudience"],
                  issuer: _configuration["JWT:ValidIssuer"],
                  expires: DateTime.UtcNow.AddDays(double.Parse(_configuration["JWT:DurationInDays"])),
                  claims: authClaims,
                  signingCredentials: new SigningCredentials(authkey, SecurityAlgorithms.HmacSha256Signature)

            );

            //Create token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
