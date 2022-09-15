using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AlchimonAng.Models;
using Microsoft.IdentityModel.Tokens;

namespace AlchimonAng.Services
{
    public class JwtBuilderService
    {
        public JwtBuilderService()
        {
        }

        public async Task<string> BuildToken(Player newPlayer)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, newPlayer.Nik),
                new Claim(ClaimTypes.Role, newPlayer.role),
                new Claim(ClaimTypes.Country, newPlayer.Id)
            };
            var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(150)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
#if DEBUG
            Console.WriteLine(encodedJwt);
#endif
            return encodedJwt;

        }
    }
}

