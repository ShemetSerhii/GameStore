using GameStore.DAL.DBContexts.MongoDB.Intefaces;
using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameStore.DAL.Adapters
{
    public class ShipperAdapter : IBaseAdapter<Shipper>
    {
        private readonly IMongoRepository<Shipper> _mongo;

        public ShipperAdapter(IMongoRepository<Shipper> mongoRepository)
        {
            _mongo = mongoRepository;
        }

        public IEnumerable<Shipper> Get()
        {
            var shippers = _mongo.Get();

            return shippers;
        }

        public IEnumerable<Shipper> Get(Func<Shipper, bool> predicate,
            Func<IEnumerable<Shipper>, IOrderedEnumerable<Shipper>> sorting = null)
        {
            var shippers = _mongo.Get(predicate, sorting);

            return shippers;
        }
    }
}
