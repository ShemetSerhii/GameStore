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
    public class MongoSupplierRepository : IAdvancedMongoRepository<Publisher>
    {
        private const string SupplierIdProperty = "SupplierID";

        private readonly IMongoContext _mongoContext;
        private readonly SqlContext _sqlContext;

        public MongoSupplierRepository(IMongoContext context, SqlContext sqlContextContext)
        {
            _mongoContext = context;
            _sqlContext = sqlContextContext;
        }

        public IEnumerable<Publisher> Get()
        {
            var suppliers = _mongoContext.Suppliers.AsQueryable().Where(sup => sup.IsDeleted == null || sup.IsDeleted == false);

            var publishers = Mapper.Map<IEnumerable<SupplierMongo>, List<Publisher>>(suppliers);

            FindSqlGames(publishers);

            return publishers;
        }

        public IEnumerable<Publisher> Get(Func<Publisher, bool> predicate, Func<IEnumerable<Publisher>, IOrderedEnumerable<Publisher>> sorting = null)
        {
            var suppliers = _mongoContext.Suppliers.AsQueryable().Where(sup => sup.IsDeleted == null || sup.IsDeleted == false);
            var publishers = Mapper.Map<IEnumerable<SupplierMongo>, List<Publisher>>(suppliers);

            publishers = publishers.Where(predicate).ToList();
            var publishersId = publishers.Select(pub => pub.Id);

            var product = _mongoContext.Products.AsQueryable().Where(prod =>
                (prod.IsDeleted == null || prod.IsDeleted == false) &&
                publishersId.Contains(prod.SupplierID)).ToList();

            var storeProduct = Mapper.Map<IEnumerable<ProductMongo>, List<Game>>(product);

            PublisherForeignConnector(publishers, storeProduct);
            FindSqlGames(publishers);

            return publishers;
        }

        public void Remove(Publisher item)
        {
            item.IsDeleted = true;

            Update(item);
        }

        public void Update(Publisher item)
        {
            var supplier = Mapper.Map<Publisher, SupplierMongo>(item);

            var filterForSupplier = Builders<SupplierMongo>.Filter.Eq(SupplierIdProperty, supplier.SupplierID);
            var id = _mongoContext.Suppliers.Find(filterForSupplier).ToList().SingleOrDefault().Id;

            supplier.Id = id;

            _mongoContext.Suppliers.ReplaceOne(filterForSupplier, supplier);
        }

        private void FindSqlGames(IEnumerable<Publisher> publishers)
        {
            var filterForProduct = Builders<SupplierMongo>.Filter.In(SupplierIdProperty, publishers.Select(x => x.Id));

            var suppliers = _mongoContext.Suppliers.Find(filterForProduct).ToList();

            foreach (var supplier in suppliers)
            {
                if (supplier.SqlGamesId.Any())
                {
                    foreach (var gamesId in supplier.SqlGamesId)
                    {
                        var game = _sqlContext.Games.Find(gamesId);

                        publishers.SingleOrDefault(x => x.Id == supplier.SupplierID).Games.Add(game);
                    }
                }
            }
        }

        private void PublisherForeignConnector(IEnumerable<Publisher> publishers, IEnumerable<Game> games)
        {
            foreach (var publisher in publishers)
            {
                publisher.Games = games.Where(x => x.PublisherId == publisher.Id).ToList();
            }
        }
    }
}
