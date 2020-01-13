using GameStore.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameStore.BLL.Interfaces
{
    public interface IGameService
    {
        IEnumerable<Game> GetAllGames();
        IEnumerable<Game> Pagination(IEnumerable<Game> games, int page, int pageSize);
        Game GetGame(string key);
        Game GetGameByInterimProperty(int id, string interimProperty);
        void UpdateGame(Game game);
        void DeleteGame(string key);
        void CreateGame(Game game);
        IEnumerable<Genre> GetGenres();
        IEnumerable<PlatformType> GetPlatformTypes();
        IEnumerable<Publisher> GetPublishers();
    }
}
