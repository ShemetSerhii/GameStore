using AutoMapper;
using GameStore.DAL.DBContexts.EF;
using GameStore.DAL.DBContexts.MongoDB.Intefaces;
using GameStore.DAL.DBContexts.MongoDB.MongoModel;
using GameStore.Domain.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameStore.DAL.DBContexts.MongoDB.Repositories
{
    public class MongoProductRepository : IAdvancedMongoRepository<Game>
    {
        private const string ProductIdProperty = "ProductID";
        private const string CategoryIdProperty = "CategoryID";
        private const string CategoryNameProperty = "CategoryName";
        private const string MongoLabel = "$$M$$";

        private readonly IMongoContext _mongoContext;
        private readonly SqlContext _sqlContext;

        public MongoProductRepository(IMongoContext context, SqlContext sqlContextContext)
        {
            _mongoContext = context;
            _sqlContext = sqlContextContext;
        }

        public IEnumerable<Game> Get()
        {
            var products = _mongoContext.Products.AsQueryable().Where(prod => prod.IsDeleted == null || prod.IsDeleted == false);

            var storeProduct = Mapper.Map<IEnumerable<ProductMongo>, List<Game>>(products);

            return storeProduct;
        }

        public IEnumerable<Game> Get(Func<Game, bool> predicate, Func<IEnumerable<Game>, IOrderedEnumerable<Game>> sorting = null)
        {
            var products = _mongoContext.Products.AsQueryable().Where(prod => prod.IsDeleted == null || prod.IsDeleted == false).ToList();

            var storeProduct = Mapper.Map<IEnumerable<ProductMongo>, List<Game>>(products);

            MainConnector(storeProduct);

            storeProduct = storeProduct.Where(predicate).ToList();

            return storeProduct;
        }

        public void Remove(Game item)
        {
            item.IsDeleted = true;

            Update(item);
        }

        public void Update(Game item)
        {
            var product = Mapper.Map<Game, ProductMongo>(item);

            var filterForProduct = Builders<ProductMongo>.Filter.Eq(ProductIdProperty, product.ProductID);

            var recoverItem = _mongoContext.Products.Find(filterForProduct).ToList().SingleOrDefault();

            product.Id = recoverItem.Id;
            product.CategoryID = recoverItem.CategoryID;

            _mongoContext.Products.ReplaceOne(filterForProduct, product);
        }


        private void MainConnector(List<Game> storeProduct)
        {
            var publishersId = SeparationNullTypes(storeProduct.Select(prod => prod.PublisherId));
            
            var supplier = _mongoContext.Suppliers.AsQueryable().Where(sup => (sup.IsDeleted == null || sup.IsDeleted == false) && publishersId.Contains(sup.SupplierID));
            var publisher = Mapper.Map<IEnumerable<SupplierMongo>, List<Publisher>>(supplier);

            var filterForPartProduct = Builders<ProductMongo>.Filter.In(ProductIdProperty, storeProduct.Select(x => x.Id));
            var partProduct = _mongoContext.Products.Find(filterForPartProduct).ToList();

            var filterForCategory = Builders<CategorieMongo>.Filter.In(CategoryIdProperty, partProduct.Select(x => x.CategoryID));
            var category = _mongoContext.Categories.Find(filterForCategory).ToList();
            var genres = Mapper.Map<IEnumerable<CategorieMongo>, List<Genre>>(category);

            SupplierForeignConnector(storeProduct, publisher);
            CategoryForeignConnector(storeProduct, genres);

            for (var index = 0; index < storeProduct.Count; index++)
            {
                CreateCrossGameSetupGenres(partProduct[index], storeProduct[index]);
                CreateCrossGameSetupPlatform(partProduct[index], storeProduct[index]);
                CreateCrossGameSetupPublisher(partProduct[index], storeProduct[index]);
            }
        }

        private void SupplierForeignConnector(IEnumerable<Game> games, IEnumerable<Publisher> publishers)
        {
            foreach (var game in games)
            {
                var publisher = publishers.SingleOrDefault(x => int.Parse(x.CrossProperty.Substring(1)) == game.PublisherId);

                if (publisher != null)
                {
                    game.Publisher = publisher;
                    game.PublisherId = publisher.Id;
                }
            }
        }

        private void CategoryForeignConnector(IEnumerable<Game> games, IEnumerable<Genre> genres)
        {
            foreach (var game in games)
            {
                game.Genres = FindGenres(game, genres);
            }
        }

        private List<Genre> FindGenres(Game game, IEnumerable<Genre> genres)
        {
            var list = new List<Genre>();

            var index = game.CrossProperty.LastIndexOf(MongoLabel, StringComparison.Ordinal) + 5;
            var key = game.CrossProperty.Substring(index);

            game.CrossProperty = game.CrossProperty.Substring(0, index - 5);

            list.AddRange(genres.Where(x => x.Id == int.Parse(key)));

            return list;
        }

        private void CreateCrossGameSetupPublisher(ProductMongo product, Game crossGame)
        {
            if (product.Publisher != null)
            {
                Publisher publisher;

                if (_sqlContext.Publishers.SingleOrDefault(x => x.CompanyName == product.Publisher) != null)
                {
                    publisher = _sqlContext.Publishers.SingleOrDefault(x => x.CompanyName == product.Publisher);

                    crossGame.Publisher = publisher;
                    crossGame.PublisherId = publisher.Id;
                }

                var supplier = _mongoContext.Suppliers.AsQueryable().SingleOrDefault(sup => sup.CompanyName == product.Publisher);

                if (supplier != null)
                {
                    publisher = Mapper.Map<SupplierMongo, Publisher>(supplier);

                    crossGame.Publisher = publisher;
                    crossGame.PublisherId = publisher.Id;
                }
            }
        }

        private void CreateCrossGameSetupGenres(ProductMongo product, Game crossGame)
        {
            if (product.Genres.Any())
            {
                var list = new List<Genre>();

                if (_sqlContext.Genres.Any(x => product.Genres.Contains(x.Name)))
                {
                    list.AddRange(_sqlContext.Genres.Where(x => product.Genres.Contains(x.Name)));
                }

                var filter = Builders<CategorieMongo>.Filter.In(CategoryNameProperty, product.Genres);

                if (_mongoContext.Categories.Find(filter).ToList().Any())
                {
                    var categories = _mongoContext.Categories.Find(filter).ToList();

                    var genre = Mapper.Map<IEnumerable<CategorieMongo>, List<Genre>>(categories);

                    list.AddRange(genre);
                }

                crossGame.Genres = list;
            }
        }

        private void CreateCrossGameSetupPlatform(ProductMongo product, Game crossGame)
        {
            if (product.PlatformTypes.Any())
            {
                var list = new List<PlatformType>();

                if (_sqlContext.PlatformTypes.Any(x => product.PlatformTypes.Contains(x.Type)))
                {
                    list.AddRange(_sqlContext.PlatformTypes.Where(x => product.PlatformTypes.Contains(x.Type)));
                }

                crossGame.PlatformTypes = list;
            }
        }

        private IEnumerable<int> SeparationNullTypes(IEnumerable<int?> list)
        {
            var result = new List<int>();

            foreach (var item in list)
            {
                if (item.HasValue)
                {
                    result.Add(item.Value);
                }
            }

            return result;
        }
    }
}
