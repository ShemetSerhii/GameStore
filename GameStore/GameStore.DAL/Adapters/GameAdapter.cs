using GameStore.DAL.DBContexts.MongoDB.Intefaces;
using GameStore.DAL.DBContexts.MongoDB.Logging.Interfaces;
using GameStore.DAL.DBContexts.MongoDB.Logging.LogEntity;
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
        private readonly IGenericRepository<Game> _sql;
        private readonly IAdvancedMongoRepository<Game> _mongo;
        private readonly ILogging _logging;

        public GameAdapter(IGenericRepository<Game> gameSql, IAdvancedMongoRepository<Game> gameMongo, ILogging logging)
        {
            _sql = gameSql;
            _mongo = gameMongo;
            _logging = logging;
        }

        public void Create(Game game)
        {
            _sql.Create(game);

            _logging.Log(game.GetType(), _logging.CudDictionary[CUDEnum.Create], game.ToBsonDocument());
        }

        public IEnumerable<Game> GetCross(int id, string crossProperty)
        {
            if (!string.IsNullOrEmpty(crossProperty))
            {
                var games = _mongo.Get(x => x.Key == crossProperty);

                return games;
            }
            else
            {
                var games = _sql.Get(x => x.Id == id);

                return games;
            }
        }

        public IEnumerable<Game> Get()
        {
            var games = _sql.Get().ToList();

            //var products = _mongo.Get().ToList();

            //games.AddRange(products);
                
            return games;
        }

        public IEnumerable<Game> Get(Func<Game, bool> predicate, 
            Func<IEnumerable<Game>, IOrderedEnumerable<Game>> sorting = null)
        {
            var query = _sql.Get(predicate, sorting).ToList();

            //query.AddRange(_mongo.Get(predicate, sorting));

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

            if (item.CrossProperty == null)
            {
                _sql.Update(item);

                old = _sql.Get(x => x.Id == item.Id).SingleOrDefault();
            }
            else
            {
                _mongo.Update(item);

                old = _mongo.Get(x => x.Key == item.Key).SingleOrDefault();
            }
            
            _logging.Log(item.GetType(), _logging.CudDictionary[CUDEnum.Update], item.ToBsonDocument(), old.ToBsonDocument());
        }

        public void Remove(Game item)
        {
            if (item.CrossProperty == null)
            {
                _sql.Remove(item);
            }
            else
            {
                _mongo.Remove(item);
            }

            _logging.Log(item.GetType(), _logging.CudDictionary[CUDEnum.Delete], item.ToBsonDocument());
        }
    }
}
