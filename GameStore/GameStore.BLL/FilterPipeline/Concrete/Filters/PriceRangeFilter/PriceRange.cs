﻿using GameStore.BLL.FilterPipeline.Abstract;
using GameStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace GameStore.BLL.FilterPipeline.Concrete.Filters.PriceRangeFilter
{
    public class PriceRange : IFilter<IEnumerable<Game>>
    {
        private readonly decimal[] _priceRange;

        public PriceRange(decimal[] priceRange)
        {
            _priceRange = priceRange;
        }

        public Expression<Func<Game, bool>> Execute()
        {
            Expression<Func<Game, bool>> result = game => (game.Price >= _priceRange[0]) && (game.Price <= _priceRange[1]);

            return result;
        }
    }
}
