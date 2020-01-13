using GameStore.BLL.FilterPipeline.Abstract;
using GameStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace GameStore.BLL.FilterPipeline.Concrete.Filters
{
    public class PlatformFilter : IFilter<IEnumerable<Game>>
    {
        private readonly string[] _platforms;

        public PlatformFilter(string[] platforms)
        {
            _platforms = platforms;
        }

        public Expression<Func<Game, bool>> Execute()
        {
            Expression<Func<Game, bool>> result = game => game.PlatformTypes.Select(g => g.Type).Intersect(_platforms).ToList().Any();

            return result;
        }
    }
}
