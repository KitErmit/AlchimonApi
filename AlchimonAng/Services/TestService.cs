using AlchimonAng.ViewModels;
using System.Security.Claims;

namespace AlchimonAng.Services
{
    
    public class TestService 
    {
        private readonly IPlayerRepository _playerRepository;
        public TestService(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }


        public async Task<BoolTextRespViewModel> Foo(ClaimsPrincipal user)
        {
            string respText = "";
            ClaimsIdentity? identity = user.Identity as ClaimsIdentity;
            if (identity is null || !identity.IsAuthenticated) return new BoolTextRespViewModel { Good = false, Text = "Pusto" };
            else
            {
                respText += "Authorize " + identity.FindFirst(ClaimTypes.Country).Value + " " + identity.FindFirst(ClaimTypes.Role).Value;
                return new BoolTextRespViewModel { Good = true, Text = respText };
            }
        }
    }
}

