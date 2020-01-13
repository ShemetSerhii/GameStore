using AutoMapper;
using GameStore.DAL.DBContexts.MongoDB.Intefaces;
using GameStore.DAL.DBContexts.MongoDB.MongoModel;
using GameStore.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameStore.DAL.DBContexts.MongoDB.Repositories
{
    public class MongoOrderRepository : IMongoRepository<Order>
    {
        private const string OrderIdProperty = "OrderID";
        private const string ProductIdProperty = "ProductID";

        private readonly IMongoContext _mongoContext;

        public MongoOrderRepository(IMongoContext context)
        {
            _mongoContext = context;
        }

        public IEnumerable<Order> Get()
        {
            var emptyOrders = _mongoContext.Orders.Find(new BsonDocument()).ToList();

            var storeOrders = Mapper.Map<IEnumerable<OrderMongo>, List<Order>>(emptyOrders);

            return storeOrders;
        }

        public IEnumerable<Order> Get(Func<Order, bool> predicate, Func<IEnumerable<Order>, IOrderedEnumerable<Order>> sorting = null)
        {
            var emptyOrders = _mongoContext.Orders.Find(new BsonDocument()).ToList();
            var storeOrders = Mapper.Map<IEnumerable<OrderMongo>, List<Order>>(emptyOrders);

            storeOrders = storeOrders.Where(predicate).ToList();

            var filterForOrderDetails = Builders<OrderDetailMongo>.Filter.In(OrderIdProperty, storeOrders.Select(x => x.Id));

            var orderDetails = _mongoContext.OrderDetails.Find(filterForOrderDetails).ToList();
            var storeOrderDetails = Mapper.Map<IEnumerable<OrderDetailMongo>, List<OrderDetail>>(orderDetails);

            var filterForProducts = Builders<ProductMongo>.Filter.In(ProductIdProperty, storeOrderDetails.Select(x => x.GameId));

            var product = _mongoContext.Products.Find(filterForProducts).ToList();
            var storeProduct = Mapper.Map<IEnumerable<ProductMongo>, List<Game>>(product);

            OrderDetailForeignConnector(storeOrderDetails, storeProduct);
            OrderForeignConnector(storeOrders, storeOrderDetails);           

            return storeOrders;
        }

        private void OrderDetailForeignConnector(IEnumerable<OrderDetail> orderDetails, IEnumerable<Game> products)
        {
            foreach (var orderDetail in orderDetails)
            {
                var game = products.SingleOrDefault(x => x.Id == orderDetail.GameId);

                orderDetail.Game = game;
                orderDetail.GameId = game.Id;
            }
        }

        private void OrderForeignConnector(IEnumerable<Order> orders, IEnumerable<OrderDetail> orderDetails)
        {
            foreach (var order in orders)
            {
                order.OrderDetails = orderDetails.Where(x => x.OrderId == order.Id).ToList();
            }
        }
    }
}
