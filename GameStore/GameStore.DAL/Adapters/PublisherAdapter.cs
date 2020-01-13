using GameStore.DAL.DBContexts.MongoDB.Intefaces;
using GameStore.DAL.DBContexts.MongoDB.Logging.Interfaces;
using GameStore.DAL.DBContexts.MongoDB.Logging.LogEntity;
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
        private readonly IGenericRepository<Publisher> _sql;
        private readonly IAdvancedMongoRepository<Publisher> _mongo;
        private readonly ILogging _logging;

        public PublisherAdapter(IGenericRepository<Publisher> publisherSql, IAdvancedMongoRepository<Publisher> publisherMongo, ILogging logging)
        {
            _sql = publisherSql;
            _mongo = publisherMongo;
            _logging = logging;
        }

        public void Create(Publisher item)
        {
            _sql.Create(item);

            _logging.Log(item.GetType(), _logging.CudDictionary[CUDEnum.Create], item.ToBsonDocument());
        }

        public IEnumerable<Publisher> GetCross(int id, string crossProperty)
        {
            if (crossProperty != null)
            {
                var publishers = _mongo.Get(x => x.CrossProperty == crossProperty);

                return publishers;
            }
            else
            {
                var publishers = _sql.Get(x => x.Id == id);

                return publishers;
            }
        }

        public IEnumerable<Publisher> Get()
        {
            var publishers = _sql.Get().ToList();
            //var mongoPublishers = _mongo.Get();

            //publishers.AddRange(mongoPublishers);

            return publishers.OrderBy(x => x.CompanyName);
        }

        public IEnumerable<Publisher> Get(Func<Publisher, bool> predicate, 
            Func<IEnumerable<Publisher>, IOrderedEnumerable<Publisher>> sorting = null)
        {
            var publisher = _sql.Get(predicate).ToList();
            var mongoPublisher = _mongo.Get(predicate);

            publisher.AddRange(mongoPublisher);

            return publisher;
        }

        public void Update(Publisher item)
        {
            Publisher old;

            if (item.CrossProperty == null)
            {
                old = _sql.Get(x => x.Id == item.Id).SingleOrDefault();

                _sql.Update(item);
            }
            else
            {
                old = _mongo.Get(x => x.CrossProperty == item.CrossProperty).SingleOrDefault();

                _mongo.Update(item);
            }

            _logging.Log(item.GetType(), _logging.CudDictionary[CUDEnum.Update], item.ToBsonDocument(), old.ToBsonDocument());
        }

        public void Remove(Publisher item)
        {
            if (item.CrossProperty == null)
            {
                _sql.Remove(item);
            }
            else
            {
                _mongo.Remove(item);
            }

            _logging.Log(item.GetType(), _logging.CudDictionary[CUDEnum.Delete], item.ToBsonDocument());
        }
    }
}
