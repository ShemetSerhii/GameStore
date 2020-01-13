using GameStore.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameStore.BLL.Interfaces
{
    public interface IGameService
    {
        IEnumerable<Game> Pagination(IEnumerable<Game> games, int page, int pageSize);

        Task<Game> GetByKey(string key);

        Task Update(Game game);

        Task Delete(int id);

        Task Create(Game game);

        Task<IEnumerable<Game>> GetAll();

        Task<IEnumerable<Game>> GetByGenre(Genre genre);

        Task<IEnumerable<Game>> GetByPlatformType(PlatformType platformType);

    }
}
