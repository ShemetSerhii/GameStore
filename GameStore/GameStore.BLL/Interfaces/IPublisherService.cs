using GameStore.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameStore.BLL.Interfaces
{
    public interface IPublisherService
    {
        Task<IEnumerable<Publisher>> GetAll();

        Task<Publisher> Get(int id);

        Task Create(Publisher publisher);

        Task Update(Publisher publisher);

        Task Delete(int id);
    }
}