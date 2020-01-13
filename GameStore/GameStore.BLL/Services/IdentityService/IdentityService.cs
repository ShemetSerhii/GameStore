using GameStore.BLL.Interfaces.Identity;
using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameStore.Domain.Entities;

namespace GameStore.BLL.Services.IdentityService
{
    public class IdentityService : IIdentityService
    {
        private readonly IUnitOfWork _unitOfWork;

        public IdentityService(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        public Task<IEnumerable<User>> GetAll()
        {
            var users = _unitOfWork.UserRepository.GetAsync();

            return users;
        }

        public User GetUser(string login)
        {
            return null;
        }

        public User Login(string login, string password)
        {
            var user = _unitOfWork.Users.Get(x => x.Login == login).SingleOrDefault();

            if (user != null)
            {
                if (password.GetHashCode() == user.Password)
                {
                    return user;
                }
            }

            return null;
        }

        public void Register(User user)
        {
        }

        public Task Update(User user)
        {
            _unitOfWork.UserRepository.Update(user);

            return _unitOfWork.SaveAsync();
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
    }
}
