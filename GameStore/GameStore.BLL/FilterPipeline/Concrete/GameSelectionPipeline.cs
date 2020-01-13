using GameStore.BLL.FilterPipeline.Abstract;
using GameStore.BLL.FilterPipeline.Model;
using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace GameStore.BLL.FilterPipeline.Concrete
{
    public class GameSelectionPipeline : Pipeline<IEnumerable<Game>>
    {
        private IUnitOfWork unitOfWork { get; set; }

        public GameSelectionPipeline(IUnitOfWork uow)
        {
            unitOfWork = uow;
        }

        public override IEnumerable<Game> Process()
        {
            Expression<Func<Game, bool>> expression = game => game.IsDeleted == false;
            foreach (var filter in Filters)
            {
                expression = CombineFilters(expression, filter.Execute());
            }

            var games = unitOfWork.Games.Get(expression.Compile(), Sort);

            return games;
        }

        protected Expression<Func<Game, bool>> CombineFilters(
            Expression<Func<Game, bool>> existingFilter,
            Expression<Func<Game, bool>> newFilter)
        {
            var parameter = Expression.Parameter(typeof(Game));

            var leftVisitor = new ExpressionMergingVisitor(existingFilter.Parameters[0], parameter);
            var left = leftVisitor.Visit(existingFilter.Body);

            var rightVisitor = new ExpressionMergingVisitor(newFilter.Parameters[0], parameter);
            var right = rightVisitor.Visit(newFilter.Body);

            var combinedFilter = Expression.Lambda<Func<Game, bool>>(Expression.AndAlso(left, right), parameter);

            return combinedFilter;
        }

        public override void FilterRegister(FilterModelDTO filterModel)
        {
            var priceRange = new[] { filterModel.PriceRangeFrom, filterModel.PriceRangeTo };

            var filters = new FilterContext(filterModel.GenresName, filterModel.PlatformTypesName, filterModel.PublishersName, priceRange, filterModel.PartOfName);
            var sort = new SortingResolver();

            if (filterModel.GenresName.Any())
            {
                Register(filters.FilterDictionary[FiltersEnum.GenreFilter]);
            }

            if (filterModel.PlatformTypesName.Any())
            {
                Register(filters.FilterDictionary[FiltersEnum.PlatformFilter]);
            }

            if (filterModel.PublishersName.Any())
            {
                Register(filters.FilterDictionary[FiltersEnum.PublisherFilter]);
            }
            
            if (filterModel.PriceRangeFrom != 0 || filterModel.PriceRangeTo != 0)
            {
                Register(filters.FilterDictionary[FiltersEnum.PriceRange]);
            }

            if (filterModel.PublicationTime != 0)
            {
                Register(filters.FilterDictionary[filterModel.PublicationTime]);
            }

            if (filterModel.PartOfName != null)
            {
                Register(filters.FilterDictionary[FiltersEnum.SearchByName]);
            }

            SortRegister(sort.CreateSorting(filterModel));
        }
    }
}
