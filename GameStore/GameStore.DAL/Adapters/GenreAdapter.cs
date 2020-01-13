using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameStore.DAL.Adapters
{
    public class GenreAdapter : ICrossAdapter<Genre>
    {
        private readonly IRepository<Genre> _sql;

        public GenreAdapter(IRepository<Genre> sqlGenre)
        {
            _sql = sqlGenre;
        }

        public void Create(Genre item)
        {
            _sql.Create(item);
        }

        public IEnumerable<Genre> Get()
        {
            var genres = _sql.Get().ToList();

            return genres;
        }

        public IEnumerable<Genre> GetCross(int id, string crossProperty)
        {
            var genres = _sql.Get(x => x.Id == id);

            return genres;
        }

        public IEnumerable<Genre> Get(Func<Genre, bool> predicate,
            Func<IEnumerable<Genre>, IOrderedEnumerable<Genre>> sorting = null)
        {
            var genres = _sql.Get(predicate, sorting).ToList();

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

            _sql.Update(item);

            old = _sql.Get(x => x.Id == item.Id).SingleOrDefault();
        }

        public void Remove(Genre item)
        {
            _sql.Remove(item);
        }
    }
}
