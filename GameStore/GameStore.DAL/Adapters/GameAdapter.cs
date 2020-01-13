using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameStore.DAL.Adapters
{
    public class GameAdapter : ICrossAdapter<Game>
    {
        private readonly IRepository<Game> _sql;

        public GameAdapter(IRepository<Game> gameSql)
        {
            _sql = gameSql;
        }

        public void Create(Game game)
        {
            _sql.Create(game);
        }

        public IEnumerable<Game> GetCross(int id, string crossProperty)
        {

            var games = _sql.Get(x => x.Id == id);

            return games;
        }

        public IEnumerable<Game> Get()
        {
            var games = _sql.Get().ToList();

            return games;
        }

        public IEnumerable<Game> Get(Func<Game, bool> predicate,
            Func<IEnumerable<Game>, IOrderedEnumerable<Game>> sorting = null)
        {
            var query = _sql.Get(predicate, sorting).ToList();

            if (sorting != null)
            {
                query = sorting(query).ToList();

                return query;
            }

            return query;
        }

        public void Update(Game item)
        {
            Game old;

            _sql.Update(item);

            old = _sql.Get(x => x.Id == item.Id).SingleOrDefault();
        }

        public void Remove(Game item)
        {
            _sql.Remove(item);
        }
    }
}
