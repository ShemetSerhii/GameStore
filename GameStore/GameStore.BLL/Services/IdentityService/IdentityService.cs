using GameStore.BLL.Interfaces.Identity;
using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using GameStore.Domain.Entities;

namespace GameStore.BLL.Services.IdentityService
{
    public class IdentityService : IIdentityService
    {
        private IUnitOfWork unitOfWork { get; }

        public IdentityService(IUnitOfWork uow)
        {
            unitOfWork = uow;
        }

        public IEnumerable<User> GetAll()
        {
            var users = unitOfWork.Users.Get();

            return users;
        }

        public User GetUser(string login)
        {
            var user = unitOfWork.Users.Get(x => x.Login == login).SingleOrDefault();

            return user;
        }

        public User Login(string login, string password)
        {
            var user = unitOfWork.Users.Get(x => x.Login == login).SingleOrDefault();

            if (user != null)
            {
                if (user.Password == 0)
                {
                    return UserMigrationFromMongo(user, password);
                }

                if (password.GetHashCode() == user.Password)
                {
                    return user;
                }
            }

            return null;
        }

        public void Register(User user)
        {
            UnBan(user);

            var userRole = unitOfWork.Roles.Get(rol => rol.Name == "User").SingleOrDefault();
            user.Roles.Add(userRole);

            unitOfWork.Users.Create(user);
        }

        public void RegisterPublisher(User user, string companyName)
        {
            var publisher = new Publisher
            {
                CompanyName = companyName,
                UserLogin = user.Login
            };

            var userRole = unitOfWork.Roles.Get(rol => rol.Name == "Publisher").SingleOrDefault();
            user.Roles.Add(userRole);

            unitOfWork.Publishers.Create(publisher);
        }

        public void Update(User user)
        {
            UnBan(user);

            unitOfWork.Users.Update(user);
        }

        public void Ban(User user)
        {
            user.IsBanned = true;

            unitOfWork.Users.Remove(user);
        }

        private void UnBan(User user)
        {
            if (user.BanExpires != null && user.BanExpires < DateTime.UtcNow)
            {
                user.IsBanned = false;
                user.BanExpires = null;
            }
        }

        public bool IsBanned(string login)
        {
            var user = GetUser(login);

            if (user == null)
            {
                return true;
            }

            return user.IsBanned;
        }

        public bool CompanyNameValidation(string companyName)
        {
            var company = unitOfWork.Publishers.Get(x => x.CompanyName == companyName).SingleOrDefault();

            if (company == null)
            {
                return false;
            }

            return true;
        }

        private User UserMigrationFromMongo(User user, string password)
        {
            user.Password = password.GetHashCode();

            Register(user);

            return user;
        }
    }
}
