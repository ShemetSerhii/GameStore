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
        private readonly SqlContext _context;

        public SqlGameRepository(SqlContext context)
        {
            _context = context;
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

            var result = sortedEntities.Where(predicate);

            return result;
        }

        public void Update(Game item)
        {

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
    }
}
