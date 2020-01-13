using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameStore.DAL.Adapters
{
    public class ShipperAdapter : IBaseAdapter<Shipper>
    {

        public ShipperAdapter()
        {
        }

        public IEnumerable<Shipper> Get()
        {
            return null;
        }

        public IEnumerable<Shipper> Get(Func<Shipper, bool> predicate,
            Func<IEnumerable<Shipper>, IOrderedEnumerable<Shipper>> sorting = null)
        {
            return null;
        }
    }
}
