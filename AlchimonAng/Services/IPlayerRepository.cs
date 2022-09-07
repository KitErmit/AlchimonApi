using System;
using System.Linq;
using AlchimonAng.Models;
namespace AlchimonAng.Services
{
    public interface IPlayerRepository
    {
        Task<IList<Player>> GetList(); // получение всех объектов
        Task<Player> GetOne(string id); // получение одного объекта по id
        Task Create(Player newPlayer); // создание объекта
        Task Update(Player updatePlayer); // обновление объекта
        Task Delete(string id);
    }

    public class JsonPlayerRepository : IPlayerRepository
    {
        private readonly ISaveLoader<Dictionary<string, Player>> _saveLoader;

        private const string _path = "Base/UserRoster.txt";

        private Dictionary<string, Player> _roster = new();
        public JsonPlayerRepository(ISaveLoader<Dictionary<string, Player>> saveLoader)
        {
            _saveLoader = saveLoader;
        }
        public Task<IList<Player>> GetList()
        {
            return Task.FromResult((IList<Player>)_saveLoader.Load(_path).Select(p => p.Value).ToList());

        }
        public Task<Player> GetOne(string id)
        {
            var keyVal = _saveLoader.Load(_path).FirstOrDefault(a => a.Key == id);
            return Task.FromResult(keyVal.Value);

        }
        public Task Create(Player newPlayer)
        {
            _roster = _saveLoader.Load(_path);
            _roster.Add(newPlayer.Id, newPlayer);
            _saveLoader.Save(_roster, _path);
            _roster.Clear();
            return Task.CompletedTask;
        }
        public Task Update(Player updatePlayer)
        {
            var oldVersion = _saveLoader.Load(_path).FirstOrDefault(a => a.Key == updatePlayer.Id).Value;
            oldVersion = updatePlayer;
            _saveLoader.Save(_roster, _path);
            _roster.Clear();
            return Task.CompletedTask;
        }
        public Task Delete(string id)
        {
            _roster = _saveLoader.Load(_path);
            _roster.Remove(id);
            _saveLoader.Save(_roster, _path);
            _roster.Clear();
            return Task.CompletedTask;
        }
    }
}

