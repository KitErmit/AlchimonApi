using System;
using System.Linq;
using AlchimonAng.Models;
namespace AlchimonAng.DB.Repository
{
    public interface IPlayerRepository : IDisposable
    {
        Task<IList<Player>> GetList(); // получение всех объектов
        Task<Player> GetOne(string id); // получение одного объекта по id
        Task<Player> Create(Player newPlayer); // создание объекта
        Task<Player> Update(Player updatePlayer); // обновление объекта
        Task<Task> Delete(string id);
        void Save();
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

            return Task.FromResult((IList<Player>)_saveLoader.Load(_path).Select(p => p.Value).OrderBy(p => p.Nik).ToList());

        }
        public Task<Player> GetOne(string id)
        {
            var keyVal = _saveLoader.Load(_path).FirstOrDefault(a => a.Key == id);
            return Task.FromResult(keyVal.Value);

        }
        public async Task<Player> Create(Player newPlayer) //Возвращаем айди
        {
            Dictionary<string, Player> _roster = _saveLoader.Load(_path);
            _roster.Add(newPlayer.Id, newPlayer);
            _saveLoader.Save(_roster, _path);
            return await GetOne(newPlayer.Id);
             
        }
        public async Task<Player> Update(Player updatePlayer)
        {
            Dictionary<string, Player> _roster = _saveLoader.Load(_path);
            var oldVersion = _roster.FirstOrDefault(a => a.Key == updatePlayer.Id).Value;
            _roster.Remove(oldVersion.Id);
            _roster.Add(updatePlayer.Id, updatePlayer);
            _saveLoader.Save(_roster, _path);
            return _saveLoader.Load(_path)[updatePlayer.Id];
        }
        public async Task<Task> Delete(string id)
        {
            Dictionary<string, Player> _roster = _saveLoader.Load(_path);
            Player player = await GetOne(id);
            Console.WriteLine(player.Nik + "Удален");
            _roster.Remove(id);
            _saveLoader.Save(_roster, _path);

            if (_saveLoader.Load(_path).Any(p => p.Value.Id == id)) throw new Exception($"Айди: {id} еще существует в ростере");
            _roster.Clear();
            return Task.CompletedTask;
        }
        public void Save() { }


        public virtual void Dispose(bool disposing)
        {}

        public void Dispose()
        {}
    }
}