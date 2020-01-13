using GameStore.BLL.FilterPipeline.Abstract;
using GameStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace GameStore.BLL.FilterPipeline.Concrete.Filters
{
    public class GenreFilter : IFilter<IEnumerable<Game>>
    {
        private readonly string[] _genres;

        public GenreFilter(string[] genres)
        {
            _genres = genres;
        }

        public Expression<Func<Game, bool>> Execute()
        {
            Expression<Func<Game, bool>> result = game => game.Genres.Select(g => g.Name).Intersect(_genres).ToList().Any();

            return result;
        }
    }
}
