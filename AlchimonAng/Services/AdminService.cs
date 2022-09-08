using System;
namespace AlchimonAng.Services
{
    public interface IAdminService
    {
        Task DeletePlayer(string id);
    }
    public class SimpleAdminService : IAdminService
    {
        private readonly IPlayerRepository _playerRepository;
        public SimpleAdminService(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }
        public async Task DeletePlayer(string id)
        {
            await _playerRepository.Delete(id);
        }
    }
}

