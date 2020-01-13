using AutoMapper;
using GameStore.DAL.DBContexts.MongoDB.Intefaces;
using GameStore.DAL.DBContexts.MongoDB.MongoModel;
using GameStore.Domain.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameStore.DAL.DBContexts.MongoDB.Repositories
{
    public class MongoShipperRepository : IMongoRepository<Shipper>
    {
        private readonly IMongoContext _mongoContext;

        public MongoShipperRepository(IMongoContext context)
        {
            _mongoContext = context;
        }

        public IEnumerable<Shipper> Get()
        {
            var shippersMongo = _mongoContext.Shippers.AsQueryable().ToList();

            var shippers = Mapper.Map<List<ShipperMongo>, List<Shipper>>(shippersMongo);

            return shippers;
        }

        public IEnumerable<Shipper> Get(Func<Shipper, bool> predicate, Func<IEnumerable<Shipper>, IOrderedEnumerable<Shipper>> sorting = null)
        {
            var shippersMongo = _mongoContext.Shippers.AsQueryable().ToList();

            var shippers = Mapper.Map<List<ShipperMongo>, List<Shipper>>(shippersMongo);

            return shippers.Where(predicate);
        }
    }
}
