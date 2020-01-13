using GameStore.BLL.FilterPipeline.Abstract;
using GameStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace GameStore.BLL.FilterPipeline.Concrete.Filters.PublishTimeFilters
{
    public class LastYears : IFilter<IEnumerable<Game>>
    {
        private readonly int _year;

        public LastYears(int year)
        {
            _year = year;
        }

        public Expression<Func<Game, bool>> Execute()
        {
            Expression<Func<Game, bool>> result = game => game.DatePublication >= DateTime.UtcNow.AddYears(_year);

            return result;
        }
    }
}
