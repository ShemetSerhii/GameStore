using GameStore.Domain.Entities;
using System.Collections.Generic;

namespace GameStore.BLL.Interfaces
{
    public interface IShipperService
    {
        IEnumerable<Shipper> GetAll();
        Shipper Get(int id);
    }
}