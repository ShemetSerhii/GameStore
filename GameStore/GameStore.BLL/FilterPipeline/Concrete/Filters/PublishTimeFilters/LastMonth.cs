using GameStore.BLL.FilterPipeline.Abstract;
using GameStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace GameStore.BLL.FilterPipeline.Concrete.Filters.PublishTimeFilters
{
    public class LastMonth : IFilter<IEnumerable<Game>>
    {
        public Expression<Func<Game, bool>> Execute()
        {
            Expression<Func<Game, bool>> result = game => game.DatePublication >= DateTime.UtcNow.AddMonths(-1);

            return result;
        }
    }
}
