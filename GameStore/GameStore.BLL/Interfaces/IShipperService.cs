using GameStore.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameStore.BLL.Interfaces
{
    public interface IShipperService
    {
        Task<IEnumerable<Shipper>> GetAll();

        Task<Shipper> Get(int id);
    }
}