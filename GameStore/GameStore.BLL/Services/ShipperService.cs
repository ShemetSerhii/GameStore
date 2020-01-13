using GameStore.BLL.Interfaces;
using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace GameStore.BLL.Services
{
    public class ShipperService : IShipperService
    {
        private IUnitOfWork unitOfWork { get; set; }

        public ShipperService(IUnitOfWork uow)
        {
            unitOfWork = uow;
        }

        public IEnumerable<Shipper> GetAll()
        {
            var shippers = unitOfWork.Shippers.Get();

            return shippers;
        }

        public Shipper Get(int id)
        {
            var shipper = unitOfWork.Shippers.Get(x => x.Id == id).SingleOrDefault();

            return shipper;
        }
    }
}
