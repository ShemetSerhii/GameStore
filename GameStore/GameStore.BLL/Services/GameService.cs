using System;
using GameStore.BLL.Interfaces;
using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Threading.Tasks;
using GameStore.BLL.Services.Tools;

namespace GameStore.BLL.Services
{
    public class GameService : IGameService
    {
        private IUnitOfWork unitOfWork { get; set; }

        public GameService(IUnitOfWork uow)
        {
            unitOfWork = uow;
        }

        public IEnumerable<Game> GetAllGames()
        {
            var games = unitOfWork.Games.Get();

            return games;
        }

        public IEnumerable<Game> Pagination(IEnumerable<Game> games, int page, int pageSize)
        {
            var result = games.Skip((page - 1) * pageSize).Take(pageSize);

            ChangeCurrentCurrency(result);

            return result;
        }

        public Game GetGame(string key)
        {
            var game = unitOfWork.Games.Get(
                g => g.Key == key).SingleOrDefault();

            ChangeCurrentCurrency(game);

            return game;
        }

        public Game GetGameByInterimProperty(int id, string crossProperty)
        {
            var game = unitOfWork.Games.GetCross(id, crossProperty).SingleOrDefault();

            return game;
        }

        public void CreateGame(Game game)
        {
            if (game != null)
            {
                unitOfWork.Games.Create(game);
            }
        }

        public void UpdateGame(Game game)
        {
            if (game != null)
            {
                unitOfWork.Games.Update(game);
            }
        }

        public void DeleteGame(string key)
        {
            var game = unitOfWork.Games.Get(g => g.Key == key).SingleOrDefault();

            unitOfWork.Games.Remove(game);
        }

        public IEnumerable<Genre> GetGenres()
        {
            var genres = unitOfWork.Genres.Get();

            return genres.ToList();
        }

        public IEnumerable<PlatformType> GetPlatformTypes()
        {
            var platformTypes = unitOfWork.PlatformTypes.Get();

            return platformTypes.ToList();
        }

        public IEnumerable<Publisher> GetPublishers()
        {
            var publishers = unitOfWork.Publishers.Get();

            return publishers.ToList();
        }

        private void ChangeCurrentCurrency(IEnumerable<Game> games)
        {
            foreach (var game in games)
            {
                ChangeCurrentCurrency(game);
            }
        }

        private void ChangeCurrentCurrency(Game game)
        {
            if (Thread.CurrentThread.CurrentCulture.Name == "ru-RU")
            {
                game.Price *= CurrencyApiReader.CurrencyRU;

                game.Price = decimal.Round(game.Price, 2);
            }
        }
    }
}
