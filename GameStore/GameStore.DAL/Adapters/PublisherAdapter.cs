using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameStore.DAL.Adapters
{
    public class PublisherAdapter : ICrossAdapter<Publisher>
    {
        private readonly IRepository<Publisher> _sql;

        public PublisherAdapter(IRepository<Publisher> publisherSql)
        {
            _sql = publisherSql;
        }

        public void Create(Publisher item)
        {
            _sql.Create(item);
        }

        public IEnumerable<Publisher> GetCross(int id, string crossProperty)
        {

            var publishers = _sql.Get(x => x.Id == id);

            return publishers;
        }

        public IEnumerable<Publisher> Get()
        {
            var publishers = _sql.Get().ToList();

            return publishers.OrderBy(x => x.CompanyName);
        }

        public IEnumerable<Publisher> Get(Func<Publisher, bool> predicate,
            Func<IEnumerable<Publisher>, IOrderedEnumerable<Publisher>> sorting = null)
        {
            var publisher = _sql.Get(predicate).ToList();

            return publisher;
        }

        public void Update(Publisher item)
        {
            Publisher old;


            old = _sql.Get(x => x.Id == item.Id).SingleOrDefault();

            _sql.Update(item);
        }

        public void Remove(Publisher item)
        {
            _sql.Remove(item);
        }
    }
}
