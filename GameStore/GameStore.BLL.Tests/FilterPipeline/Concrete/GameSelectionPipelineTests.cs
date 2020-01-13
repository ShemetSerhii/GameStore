using GameStore.BLL.FilterPipeline.Concrete;
using GameStore.BLL.FilterPipeline.Model;
using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameStore.BLL.Tests.FilterPipeline.Concrete
{
    [TestFixture]
    public class GameSelectionPipelineTests
    {
        private Mock<IUnitOfWork> _unitMock;
        private List<Game> _games;

        [Test]
        public void FilterSearchByName_WhenPartOfNameIsName_ReturnGameWhichContainsThisPart()
        {
            var filterModel = new FilterModelDTO
            {
                PartOfName = "Name",
            };

            var filter = new FilterContext(null, null, null, null, filterModel.PartOfName);
            var exp = filter.FilterDictionary[FiltersEnum.SearchByName].Execute();

            var result = _games.Where(exp.Compile());

            Assert.AreEqual(result.First().Name, "GameName");
        }

        [Test]
        public void GenreFilter_WhenGenresIdTransferredToFilterContext_ReturnGameWhichContainsThisGenresId()
        {
            var filterModel = new FilterModelDTO
            {
                GenresId = new[] { 1, 2 },
            };

            var filter = new FilterContext(filterModel.GenresId);
            var exp = filter.FilterDictionary[FiltersEnum.GenreFilter].Execute();

            var result = _games.Where(exp.Compile());

            Assert.AreEqual(result.Count(), 1);
        }

        [Test]
        public void PlatformFilter_WhenPlatformTypesIdTransferredToFilterContext_ReturnGameWhichContainsThisPlatformTypesId()
        {
            var filterModel = new FilterModelDTO
            {
                PlatformTypesId = new[] { 1 },
            };

            var filter = new FilterContext(null, filterModel.PlatformTypesId);
            var exp = filter.FilterDictionary[FiltersEnum.PlatformFilter].Execute();

            var result = _games.Where(exp.Compile());

            Assert.AreEqual(result.Count(), 1);
        }

        [Test]
        public void PublisherFilter_WhenPublisherIdTransferredToFilterContext_ReturnGameWhichContainsThisPublisherId()
        {
            var filterModel = new FilterModelDTO
            {
                PublisherId = new[] { 1 },
            };

            var filter = new FilterContext(null, null, filterModel.PublisherId);
            var exp = filter.FilterDictionary[FiltersEnum.PublisherFilter].Execute();

            var result = _games.Where(exp.Compile());

            Assert.AreEqual(result.Count(), 1);
        }

        [Test]
        public void PriceRangeFilter_WhenPublisherIdTransferredToFilterContext_ReturnGameWhichContainsThisPublisherId()
        {
            var filterModel = new FilterModelDTO
            {
                PriceRangeFrom = 10,
                PriceRangeTo = 20
            };

            var priceRange = new [] {filterModel.PriceRangeFrom, filterModel.PriceRangeTo};

            var filter = new FilterContext(null, null, null, priceRange);
            var exp = filter.FilterDictionary[FiltersEnum.PriceRange].Execute();

            var result = _games.Where(exp.Compile());

            Assert.AreEqual(result.Count(), 2);
        }


        [Test]
        public void LastMonthFilter_WhenPublicationTimeTransferredToFilterDictionary_ReturnGameWhichContainsThisPublicationTime()
        {
            var filterModel = new FilterModelDTO
            {
                PublicationTime = FiltersEnum.LastMonth,
            };

            var filter = new FilterContext();
            var exp = filter.FilterDictionary[filterModel.PublicationTime].Execute();

            var result = _games.Where(exp.Compile());

            Assert.AreEqual(result.Count(), 1);
        }

        [Test]
        public void LastWeekFilter_WhenPublicationTimeTransferredToFilterDictionary_ReturnGameWhichContainsThisPublicationTime()
        {
            var filterModel = new FilterModelDTO
            {
                PublicationTime = FiltersEnum.LastWeek,
            };

            var filter = new FilterContext();
            var exp = filter.FilterDictionary[filterModel.PublicationTime].Execute();

            var result = _games.Where(exp.Compile());

            Assert.AreEqual(result.Count(), 1);
        }

        [Test]
        public void LastYearFilter_WhenPublicationTimeTransferredToFilterDictionary_ReturnGamesWhichContainsThisPublicationTime()
        {
            var filterModel = new FilterModelDTO
            {
                PublicationTime = FiltersEnum.LastYear,
            };

            var filter = new FilterContext();
            var exp = filter.FilterDictionary[filterModel.PublicationTime].Execute();

            var result = _games.Where(exp.Compile());

            Assert.AreEqual(result.Count(), 2);
        }

        [Test]
        public void Sorting_WhenSortingTypeIsMostPopular_ReturnGamesWhereFirstGameHas50Visits()
        {
            var filterModel = new FilterModelDTO
            {
                SortingType = SortFilter.MostPopular
            };

            var sortingResolver = new SortingResolver();
            var sorting = sortingResolver.CreateSorting(filterModel);

            var result = sorting(_games);

            Assert.AreEqual(result.First().Visits, 50);
        }

        [Test]
        public void GameSelectionPipeline_WhenRegisteredFilterModelDTO_ReturnGames()
        {
            var pipeline = new GameSelectionPipeline(_unitMock.Object);
            var filterModel = new FilterModelDTO
            {
                GenresId = new[] { 1 },
                PlatformTypesId = new[] { 1 },
                PublisherId = new[] { 1 },
                PublicationTime = FiltersEnum.LastWeek,
                PriceRangeFrom = 10,
                PriceRangeTo = 20
            };


            pipeline.FilterRegister(filterModel);

            var result = pipeline.Process();

            Assert.AreEqual(result.Count(), 2);
        }


        [SetUp]
        public void SetUp()
        {
            _unitMock = new Mock<IUnitOfWork>();

            _games = new List<Game>
            {
                new Game()
                {
                   Name = "GameName",
                   PublisherId = 1,
                   PlatformTypes = new List<PlatformType>
                   {
                       new PlatformType {Id = 1}
                   },
                   Price = 10,
                   DatePublication = DateTime.UtcNow.AddDays(-5),
                   Visits = 20,
                },
                new Game()
                {
                    Name = "Test",
                    Genres = new List<Genre>
                    {
                        new Genre {Id = 1},
                        new Genre {Id = 2}
                    },
                    Price = 20,
                    DatePublication = DateTime.UtcNow.AddMonths(-6),
                    Visits = 50
                }
            };

            _unitMock.Setup(x => x.Games.Get(It.IsAny<Func<Game, bool>>(), null)).Returns(new List<Game>
            {
                new Game(),
                new Game()
            });
        }
    }
}
