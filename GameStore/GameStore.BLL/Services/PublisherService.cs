using System.Collections.Generic;
using System.Threading.Tasks;
using GameStore.BLL.Interfaces;
using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities;

namespace GameStore.BLL.Services
{
    public class PublisherService : IPublisherService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PublisherService(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        public Task<IEnumerable<Publisher>> GetAll()
        {
            return _unitOfWork.PublisherRepository.GetAsync();
        }

        public Task<Publisher> Get(int id)
        {
            return _unitOfWork.PublisherRepository.GetAsync(id);
        }

        public Task Create(Publisher publisher)
        {
            _unitOfWork.PublisherRepository.Create(publisher);

            return _unitOfWork.SaveAsync();
        }

        public Task Update(Publisher publisher)
        {
            _unitOfWork.PublisherRepository.Update(publisher);

            return _unitOfWork.SaveAsync();
        }

        public async Task Delete(int id)
        {
            var publisher = await _unitOfWork.PublisherRepository.GetAsync(id);

            _unitOfWork.PublisherRepository.Delete(publisher);

            await _unitOfWork.SaveAsync();
        }
    }
}
