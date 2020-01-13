using GameStore.BLL.FilterPipeline.Abstract;
using GameStore.BLL.FilterPipeline.Concrete.Filters;
using GameStore.BLL.FilterPipeline.Concrete.Filters.PriceRangeFilter;
using GameStore.BLL.FilterPipeline.Concrete.Filters.PublishTimeFilters;
using GameStore.BLL.FilterPipeline.Concrete.Filters.SearchFilter;
using GameStore.Domain.Entities;
using System.Collections.Generic;

namespace GameStore.BLL.FilterPipeline.Concrete
{
    public class FilterContext
    {
        public Dictionary<FiltersEnum, IFilter<IEnumerable<Game>>> FilterDictionary;

        public FilterContext(string[] genres = null,
            string[] platforms = null,
            string[] publishers = null,
            decimal[] priceRange = null,
            string partOfName = null)
        {
            FilterDictionary = new Dictionary<FiltersEnum, IFilter<IEnumerable<Game>>>
            {
                {FiltersEnum.GenreFilter, new GenreFilter(genres) },
                {FiltersEnum.PlatformFilter, new PlatformFilter(platforms) },
                {FiltersEnum.PublisherFilter, new PublisherFilter(publishers) },
                {FiltersEnum.PriceRange, new PriceRange(priceRange) },
                {FiltersEnum.SearchByName, new SearchByName(partOfName) },
                {FiltersEnum.LastWeek, new LastWeek() },
                {FiltersEnum.LastMonth, new LastMonth() },
                {FiltersEnum.LastYear, new LastYears(-1) },
                {FiltersEnum.LastTwoYear, new LastYears(-2) },
                {FiltersEnum.LastThreeYears, new LastYears(-3) }
            };
        }
    }
}
