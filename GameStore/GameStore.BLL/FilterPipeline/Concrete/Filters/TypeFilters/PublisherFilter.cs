using GameStore.BLL.FilterPipeline.Abstract;
using GameStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace GameStore.BLL.FilterPipeline.Concrete.Filters
{
    public class PublisherFilter : IFilter<IEnumerable<Game>>
    {
        private readonly string[] _publishers;

        public PublisherFilter(string[] publishers)
        {
            _publishers = publishers;
        }

        public Expression<Func<Game, bool>> Execute()
        {
            Expression<Func<Game, bool>> result = game => game.Publisher != null && _publishers.Contains(game.Publisher.CompanyName);

            return result;
        }
    }
}
