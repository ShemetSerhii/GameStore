using GameStore.BLL.Interfaces;
using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameStore.BLL.Services
{
    public class ShipperService : IShipperService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShipperService(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        public Task<IEnumerable<Shipper>> GetAll()
        {
            return _unitOfWork.ShipperRepository.GetAsync();
        }

        public Task<Shipper> Get(int id)
        {
            return _unitOfWork.ShipperRepository.GetAsync(id);
        }
    }
}
