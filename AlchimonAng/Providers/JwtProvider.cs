using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AlchimonAng.Models;
using Microsoft.IdentityModel.Tokens;


namespace AlchimonAng.Providers
{

    //в провайдер сделать ЮзерконтекстАксессор
    public class JwtProvider
    {
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
            expires: DateTime.UtcNow.AddMinutes(1),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
#if DEBUG
            Console.WriteLine(encodedJwt);
#endif
            return encodedJwt;
        }
    }
}

