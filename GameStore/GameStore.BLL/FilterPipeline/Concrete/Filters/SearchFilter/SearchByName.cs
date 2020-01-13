using GameStore.BLL.FilterPipeline.Abstract;
using GameStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace GameStore.BLL.FilterPipeline.Concrete.Filters.SearchFilter
{
    public class SearchByName : IFilter<IEnumerable<Game>>
    {
        private readonly string _partOfName;

        public SearchByName(string partOfName)
        {
            _partOfName = partOfName;
        }

        public Expression<Func<Game, bool>> Execute()
        {
            Expression<Func<Game, bool>> result = game => game.Name.Contains(_partOfName);

            return result;
        }
    }
}
