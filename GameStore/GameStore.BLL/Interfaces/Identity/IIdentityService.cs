using System.Collections.Generic;
using GameStore.Domain.Entities.Identity;

namespace GameStore.BLL.Interfaces.Identity
{
    public interface IIdentityService
    {
        IEnumerable<User> GetAll();
        User GetUser(string login);
        User Login(string login, string password);
        void Register(User user);
        void RegisterPublisher(User user, string companyName);
        void Update(User user);
        void Ban(User user);
        bool IsBanned(string login);
        bool CompanyNameValidation(string companyName);
    }
}