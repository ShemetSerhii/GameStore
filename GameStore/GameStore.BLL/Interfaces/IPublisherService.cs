using GameStore.Domain.Entities;
using System.Collections.Generic;

namespace GameStore.BLL.Interfaces
{
    public interface IPublisherService
    {
        IEnumerable<Publisher> GetAll();
        Publisher GetByName(string companyName);
        Publisher GetByInterimProperty(int id, string interimProperty);
        void Create(Publisher publisher);
        void Update(Publisher publisher);
        void Delete(string companyName);
        bool PublisherAccess(string companyName, string login);
    }
}