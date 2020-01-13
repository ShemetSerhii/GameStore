using System.Collections.Generic;
using System.Linq;
using GameStore.BLL.Interfaces;
using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities;

namespace GameStore.BLL.Services
{
    public class PublisherService : IPublisherService
    {
        private IUnitOfWork unitOfWork { get; set; }

        public PublisherService(IUnitOfWork uow)
        {
            unitOfWork = uow;
        }

        public IEnumerable<Publisher> GetAll()
        {
            var publishers = unitOfWork.Publishers.Get();

            return publishers;
        }

        public Publisher GetByName(string companyName)
        {
            var publisher = unitOfWork.Publishers.Get(
                p => p.CompanyName == companyName).SingleOrDefault();

            return publisher;
        }

        public Publisher GetByInterimProperty(int id, string crossProperty)
        {
            var publisher = unitOfWork.Publishers.GetCross(id, crossProperty).SingleOrDefault();

            return publisher;
        }

        public void Create(Publisher publisher)
        {
            if (publisher != null)
            {
                unitOfWork.Publishers.Create(publisher);
            }
        }

        public void Update(Publisher publisher)
        {
            if (publisher != null)
            {
                unitOfWork.Publishers.Update(publisher);
            }
        }

        public void Delete(string companyName)
        {
            var publisher = GetByName(companyName);

            unitOfWork.Publishers.Remove(publisher);
        }

        public bool PublisherAccess(string companyName, string login)
        {
            var publisher = GetByName(companyName);

            if (publisher.UserLogin == login)
            {
                return true;
            }

            return false;
        }
    }
}
