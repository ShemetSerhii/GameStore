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
    public class GenreAdapter : ICrossAdapter<Genre>
    {
        private const char MongoLabel = 'M';
        private const char SqlLabel = 'S';

        private readonly IGenericRepository<Genre> _sql;
        private readonly IAdvancedMongoRepository<Genre> _mongo;
        private readonly ILogging _logging;

        public GenreAdapter(IGenericRepository<Genre> sqlGenre, IAdvancedMongoRepository<Genre> mongoGenre, ILogging logging)
        {
            _sql = sqlGenre;
            _mongo = mongoGenre;
            _logging = logging;
        }

        public void Create(Genre item)
        {
            _sql.Create(item);

            _logging.Log(item.GetType(), _logging.CudDictionary[CUDEnum.Create], item.ToBsonDocument());
        }

        public IEnumerable<Genre> Get()
        {
            var genres = _sql.Get().ToList();

            //genres.AddRange(_mongo.Get());

            return genres;
        }

        public IEnumerable<Genre> GetCross(int id, string crossProperty)
        {
            if (crossProperty != null && crossProperty[0] == MongoLabel)
            {
                var genres = _mongo.Get(x => x.CrossProperty == crossProperty);

                return genres;
            }
            else
            {
                var genres = _sql.Get(x => x.Id == id);

                return genres;
            }
        }

        public IEnumerable<Genre> Get(Func<Genre, bool> predicate, 
            Func<IEnumerable<Genre>, IOrderedEnumerable<Genre>> sorting = null)
        {
            var genres = _sql.Get(predicate, sorting).ToList();

            genres.AddRange(_mongo.Get(predicate, sorting));

            if (sorting != null)
            {
                genres = sorting(genres).ToList();

                return genres;
            }

            return genres;
        }

        public void Update(Genre item)
        {
            Genre old;

            if (item.CrossProperty == null || item.CrossProperty[0] == SqlLabel)
            {
                _sql.Update(item);

                old = _sql.Get(x => x.Id == item.Id).SingleOrDefault();
            }
            else
            {
                _mongo.Update(item);

                old = _mongo.Get(x => x.Id == item.Id).SingleOrDefault();
            }

            _logging.Log(item.GetType(), _logging.CudDictionary[CUDEnum.Update], item.ToBsonDocument(), old.ToBsonDocument());
        }

        public void Remove(Genre item)
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
