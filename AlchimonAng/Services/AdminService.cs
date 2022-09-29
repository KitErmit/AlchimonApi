using System;
using System.Security.Claims;
using AlchimonAng.DB.Repository;
using AlchimonAng.Models;
using AlchimonAng.Utils.Constans;
using AlchimonAng.ViewModels;

namespace AlchimonAng.Services
{
    public interface IAdminService
    {
        Task<BoolTextRespViewModel> RoleCheck(ClaimsPrincipal user);
        Task<BoolTextRespViewModel> DeletePlayer(string id);
        Task<IList<Player>> GetRoster();
    }
    public class SimpleAdminService : IAdminService
    {
        private readonly IPlayerRepository _playerRepository;
        public SimpleAdminService(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        public async Task<BoolTextRespViewModel> RoleCheck(ClaimsPrincipal user)
        {
            ClaimsIdentity? identity = user.Identity as ClaimsIdentity;
            if (!identity.IsAuthenticated) return new BoolTextRespViewModel { Good = false, Text = "Клаймс ID в Rolecheck пусты" };
            string? role = identity.FindFirst(ClaimTypes.Role).Value;
            if (role == PlayerRoleConsts.God) return new BoolTextRespViewModel { Good = true, Text = "ok" };
            else return new BoolTextRespViewModel { Good = false, Text = "не админ" };
        }

        public async Task<IList<Player>> GetRoster()
        {
            var list = await _playerRepository.GetList();
            var gods = (IList<Player>)list.Where(p => p.role == PlayerRoleConsts.God).ToList();
            var players = (IList<Player>)list.Where(p => p.role == PlayerRoleConsts.Player).ToList();
            list = gods.Concat(players).ToList(); ;

            return list;
        }

        public async Task<BoolTextRespViewModel> DeletePlayer(string id)
        {
            try
            {
                await _playerRepository.Delete(id);
                _playerRepository.Save();
                _playerRepository.Dispose();
                return new BoolTextRespViewModel { Good = true, Text = "Удаление прошло успешно" };
            }
            catch(Exception ex)
            {
                return new BoolTextRespViewModel { Good = false, Text = "Удаление пошло не по плану: " + ex.Message };
            }
            
        }
    }
}

