using System;
using System.Security.Claims;
using AlchimonAng.ViewModels;

namespace AlchimonAng.Accessors
{
    public class UserContextAccessor
    {
        public ClaimsUserViewModel GetClimsParams(ClaimsPrincipal user)
        {
            ClaimsIdentity? identity = user.Identity as ClaimsIdentity;
            if (!identity.IsAuthenticated) throw new ArgumentNullException("Не авторизован");
            return new ClaimsUserViewModel
            {
                Id = identity.FindFirst(ClaimTypes.Country).Value,
                Nik = identity.FindFirst(ClaimTypes.Name).Value,
                Role = identity.FindFirst(ClaimTypes.Role).Value,
            };
        }
    }
}

