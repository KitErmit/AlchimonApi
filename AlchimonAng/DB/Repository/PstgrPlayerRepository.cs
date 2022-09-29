using System;
using AlchimonAng.Models;
using Microsoft.EntityFrameworkCore;

namespace AlchimonAng.DB.Repository
{
    public class PstgrPlayerRepository : IPlayerRepository
    {
        private AlBdContext db;
        public PstgrPlayerRepository(AlBdContext db)
        {
            this.db = db;
        }

        public Task<IList<Player>> GetList()
            =>
            Task.FromResult((IList<Player>)db.Players.Include(p => p.Karman).ToList());

        public Task<Player> GetOne(string id)
            =>
            Task.FromResult(db.Players.Include(p => p.Karman).First(p => p.Id == id));

        public Task<Player> Create(Player newPlayer) => Task.FromResult(db.Players.Add(newPlayer).Entity);

        public Task<Player> Update(Player updatePlayer) => Task.FromResult( db.Players.Update(updatePlayer).Entity);

        public async Task<Task> Delete(string id)
        {
            var delPl = await GetOne(id);
            if(delPl != null) db.Players.Remove(delPl);
            return Task.CompletedTask;
        }
        public void Save() => db.SaveChanges();


        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
