using GameStore.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameStore.BLL.Interfaces
{
    public interface IGenreService
    {
        Task<IEnumerable<Genre>> GetAll();

        Task<Genre> Get(int id);

        Task Create(Genre genre);

        Task Update(Genre genre);

        Task Delete(int id);
    }
}