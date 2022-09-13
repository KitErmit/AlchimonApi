using System;
using System.Linq;
using AlchimonAng.Models;
namespace AlchimonAng.Services
{
    public interface IPlayerRepository
    {
        Task<IList<Player>> GetList(); // получение всех объектов
        Task<Player> GetOne(string id); // получение одного объекта по id
        Task<string> Create(Player newPlayer); // создание объекта
        Task<Player> Update(Player updatePlayer); // обновление объекта
        Task<Task> Delete(string id);
    }

    public class JsonPlayerRepository : IPlayerRepository
    {
        private readonly ISaveLoader<Dictionary<string, Player>> _saveLoader;

        private const string _path = "Base/UserRoster.txt";

        public JsonPlayerRepository(ISaveLoader<Dictionary<string, Player>> saveLoader)
        {
            _saveLoader = saveLoader;
        }
        public Task<IList<Player>> GetList()
        {
            return Task.FromResult((IList<Player>)_saveLoader.Load(_path).Select(p => p.Value).OrderBy(p=>p.Nik).ToList());

        }
        public Task<Player> GetOne(string id)
        {
            var keyVal = _saveLoader.Load(_path).FirstOrDefault(a => a.Key == id);
            return Task.FromResult(keyVal.Value);

        }
        public async Task<string> Create(Player newPlayer) //Возвращаем айди
        {
            Dictionary<string, Player> _roster = _saveLoader.Load(_path);
            _roster.Add(newPlayer.Id, newPlayer);
            _saveLoader.Save(_roster, _path);
            var pl = _roster.Select(a=>a.Value).FirstOrDefault(a => a.Email == newPlayer.Email);
            return pl.Id;
        }
        public async Task<Player> Update(Player updatePlayer)
        {
            Dictionary<string, Player> _roster = _saveLoader.Load(_path);
            var oldVersion = _roster.FirstOrDefault(a => a.Key == updatePlayer.Id).Value;
            _roster.Remove(oldVersion.Id);
            _roster.Add(updatePlayer.Id, updatePlayer);
            _saveLoader.Save(_roster, _path);
            var returnablePl = _roster[updatePlayer.Id];
            return returnablePl;
        }
        public async Task<Task> Delete(string id)
        {
            Dictionary<string, Player> _roster = _saveLoader.Load(_path);
            Player player = await GetOne(id);
            Console.WriteLine(player.Nik + "Удален");
            _roster.Remove(id);
            _saveLoader.Save(_roster, _path);
            _roster.Clear();
            return Task.CompletedTask;
        }
    }
}