using AutoMapper;
using GameStore.DAL.DBContexts.MongoDB.Intefaces;
using GameStore.DAL.DBContexts.MongoDB.MongoModel.EntityModel;
using GameStore.Domain.Entities.Identity;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameStore.DAL.DBContexts.MongoDB.Repositories
{
    public class MongoUserRepository : IMongoRepository<User>
    {
        private readonly IMongoContext _mongoContext;

        public MongoUserRepository(IMongoContext context)
        {
            _mongoContext = context;
        }

        public IEnumerable<User> Get()
        {
            var customers = _mongoContext.Customers.AsQueryable().ToList();

            var users = Mapper.Map<List<CustomerMongo>, List<User>>(customers);

            return users;
        }

        public IEnumerable<User> Get(Func<User, bool> predicate, Func<IEnumerable<User>, IOrderedEnumerable<User>> sorting = null)
        {
            var customers = _mongoContext.Customers.AsQueryable().ToList();

            var users = Mapper.Map<List<CustomerMongo>, List<User>>(customers);

            return users.Where(predicate);
        }
    }
}
