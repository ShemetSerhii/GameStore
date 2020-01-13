using System;
using GameStore.BLL.FilterPipeline.Model;
using System.Collections.Generic;
using System.Linq;
using GameStore.Domain.Entities;

namespace GameStore.BLL.FilterPipeline.Abstract
{
    public abstract class Pipeline<T>
    {
        protected readonly List<IFilter<T>> Filters = new List<IFilter<T>>();

        protected Func<T, IOrderedEnumerable<Game>> Sort { get; set; }

        public Pipeline<T> Register(IFilter<T> filter)
        {
            Filters.Add(filter);
            return this;
        }

        public Pipeline<T> SortRegister(Func<T, IOrderedEnumerable<Game>> sort)
        {
            Sort = sort;
            return this;
        }

        public abstract T Process();

        public abstract void FilterRegister(FilterModelDTO filterModelDto);
    }
}
