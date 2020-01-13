using System.Collections.Generic;
using System.Threading.Tasks;
using GameStore.Domain.Entities.Identity;

namespace GameStore.BLL.Interfaces.Identity
{
    public interface IIdentityService
    {
        Task<IEnumerable<User>> GetAll();

        User GetUser(string login);

        User Login(string login, string password);

        void Register(User user);

        void RegisterPublisher(User user, string companyName);

        Task Update(User user);


        bool IsBanned(string login);

        bool CompanyNameValidation(string companyName);
    }
}