using AutoMapper;
using GameStore.DAL.DBContexts.MongoDB.Intefaces;
using GameStore.DAL.DBContexts.MongoDB.MongoModel;
using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GameStore.DAL.DBContexts.EF.Repositories
{
    public class SqlGameRepository : IGenericRepository<Game>
    {
        private const string IdProperty = "Id";
        private const string CategoryIdProperty = "CategoryID";
        private const string SqlGamesIdProperty = "SqlGamesId";
        private const string SupplierIdProperty = "SupplierID";

        private readonly SqlContext _context;
        private readonly IMongoContext _mongo;

        public SqlGameRepository(SqlContext context, IMongoContext mongoContext)
        {
            _context = context;
            _mongo = mongoContext;
        }

        public void Create(Game game)
        {
            _context.Games.Add(game);

            _context.SaveChanges();
        }

        public IEnumerable<Game> Get()
        {
            var games = _context.Games
                .Where(x => x.IsDeleted == false).ToList();

            games = SeparationOfDeleted(games).ToList();

            return games;
        }

        public IEnumerable<Game> Get(Func<Game, bool> predicate,
            Func<IEnumerable<Game>, IOrderedEnumerable<Game>> sorting = null)
        {
            var query = _context.Games.ToList();
            var sortedEntities = query;

            if (sorting != null)
            {
                sortedEntities = sorting(query).ToList();
            }

            sortedEntities = SeparationOfDeleted(sortedEntities).ToList();

            //foreach (var game in sortedEntities)
            //{
            //    GetMongoGenre(game);
            //    GetMongoPublisher(game);
            //}

            var  result = sortedEntities.Where(predicate);

            //ClearMongoProperty(sortedEntities.Except(result));

            return result;
        }

        public void Update(Game item)
        {
            SaveMongoGenre(item);
            SaveMongoPublisher(item);

            if (_context.Entry(item).State == EntityState.Detached)
            {
                _context.Games.Attach(item);
            }

            _context.Entry(item).State = EntityState.Modified;

            _context.SaveChanges();
        }

        public void Remove(Game item)
        {
            item.IsDeleted = true;

            Update(item);
        }

        private IEnumerable<Game> SeparationOfDeleted(IEnumerable<Game> games)
        {
            var filteredGames = games;

            foreach (var game in filteredGames)
            {
                game.Genres = game.Genres.Where(x => x.IsDeleted == false).ToList();
                game.PlatformTypes = game.PlatformTypes.Where(x => x.IsDeleted == false).ToList();
                game.OrderDetails = game.OrderDetails.Where(x => x.IsDeleted == false).ToList();

                var publisher = _context.Publishers
                    .Where(x => x.IsDeleted == false)
                    .SingleOrDefault(x => x.Id == game.PublisherId);

                if (publisher == null)
                {
                    game.Publisher = null;
                    game.PublisherId = null;
                }
            }

            return filteredGames;
        }

        private void SaveMongoGenre(Game game)
        {
            var genres = game.Genres;

            DropMongoGenre(game);

            foreach (var genre in genres)
            {
                if (genre.CrossProperty != null)
                {
                    var filterForGameGenre =
                        Builders<CategorieMongo>.Filter.Eq(CategoryIdProperty, int.Parse(genre.CrossProperty.Substring(1)));

                    var genreFromMongo = _mongo.Categories.Find(filterForGameGenre).SingleOrDefault();

                    if (!genreFromMongo.SqlGamesId.Contains(game.Id))
                    {
                        genreFromMongo.SqlGamesId.Add(game.Id);
                    }

                    _mongo.Categories.ReplaceOne(filterForGameGenre, genreFromMongo);
                }
            }

            game.Genres = game.Genres.Where(g => g.CrossProperty == null).ToList();
        }

        private void DropMongoGenre(Game game)
        {
            var filterForGameCategories = Builders<CategorieMongo>.Filter.Exists(SqlGamesIdProperty);

            var categories = _mongo.Categories.Find(filterForGameCategories).ToList();

            foreach (var category in categories)
            {
                category.SqlGamesId = category.SqlGamesId.Where(x => x != game.Id).ToList();

                var filterForCategory = Builders<CategorieMongo>.Filter.Eq(IdProperty, category.Id);

                _mongo.Categories.ReplaceOne(filterForCategory, category);
            }
        }

        private void SaveMongoPublisher(Game game)
        {
            var publisher = game.Publisher;

            DropMongoPublisher(game);

            if (publisher != null && publisher.CrossProperty != null)
            {
                var filterForGameSupplier =
                    Builders<SupplierMongo>.Filter.Eq(SupplierIdProperty, int.Parse(publisher.CrossProperty.Substring(1)));

                var supplierFromMongo = _mongo.Suppliers.Find(filterForGameSupplier).SingleOrDefault();

                if (!supplierFromMongo.SqlGamesId.Contains(game.Id))
                {
                    supplierFromMongo.SqlGamesId.Add(game.Id);
                }

                _mongo.Suppliers.ReplaceOne(filterForGameSupplier, supplierFromMongo);

                game.Publisher = null;
                game.PublisherId = null;
            }
        }

        private void ClearMongoProperty(IEnumerable<Game> games)
        {
            foreach (var game in games)
            {
                var publisher = game.Publisher;

                if (publisher != null && publisher.CrossProperty != null)
                {
                    game.Publisher = null;
                    game.PublisherId = null;
                }

                game.Genres = game.Genres.Where(x => x.Id != 0).ToList();
            }
        }

        private void DropMongoPublisher(Game game)
        {
            var filterForGameSupplier = Builders<SupplierMongo>.Filter.Exists(SqlGamesIdProperty);

            var suppliers = _mongo.Suppliers.Find(filterForGameSupplier).ToList();

            foreach (var supplier in suppliers)
            {
                supplier.SqlGamesId = supplier.SqlGamesId.Where(x => x != game.Id).ToList();

                var filterForSupplier = Builders<SupplierMongo>.Filter.Eq(IdProperty, supplier.Id);

                _mongo.Suppliers.ReplaceOne(filterForSupplier, supplier);
            }
        }

        private void GetMongoGenre(Game game)
        {
            var filterForGameCategories = Builders<CategorieMongo>.Filter.Exists(SqlGamesIdProperty);

            var categories = _mongo.Categories.Find(filterForGameCategories).ToList();

            foreach (var category in categories)
            {
                if (category.SqlGamesId.Contains(game.Id))
                {
                    var genre = Mapper.Map<CategorieMongo, Genre>(category);

                    if (!game.Genres.Contains(genre))
                    {
                        game.Genres.Add(genre);
                    }
                }
            }
        }

        private void GetMongoPublisher(Game game)
        {
            var filterForGameSuppliers = Builders<SupplierMongo>.Filter.Exists(SqlGamesIdProperty);

            var suppliers = _mongo.Suppliers.Find(filterForGameSuppliers).ToList();

            foreach (var supplier in suppliers)
            {
                if (supplier.SqlGamesId.Contains(game.Id))
                {
                    var publisher = Mapper.Map<SupplierMongo, Publisher>(supplier);

                    game.Publisher = publisher;
                    game.PublisherId = publisher.Id;
                }
            }
        }
    }
}
