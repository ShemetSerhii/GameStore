using GameStore.BLL.FilterPipeline.Model;
using GameStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameStore.BLL.FilterPipeline.Concrete
{
    public class SortingResolver
    {
        private readonly Dictionary<SortFilter,
            Func<IEnumerable<Game>, IOrderedEnumerable<Game>>> _sorts;

        public SortingResolver()
        {
            _sorts = new Dictionary<SortFilter, Func<IEnumerable<Game>, IOrderedEnumerable<Game>>>
            {
                {SortFilter.MostPopular, games => games.OrderByDescending(x => x.Visits)},
                {SortFilter.MostCommented, games => games.OrderByDescending(x => x.Comments.Count)},
                {SortFilter.SortByDateAdded, games => games.OrderByDescending(x => x.DateAdded)},
                {SortFilter.PriceASC, games => games.OrderBy(x => x.Price)},
                {SortFilter.PriceDESC, games => games.OrderByDescending(x => x.Price)}
            };
        }

        public Func<IEnumerable<Game>, IOrderedEnumerable<Game>> CreateSorting(FilterModelDTO model)
        {
            if (model.SortingType != 0)
            {
                return _sorts[model.SortingType];
            }

            return null;
        }
    }
}
