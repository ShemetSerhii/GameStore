using GameStore.Domain.Entities.Identity;

namespace GameStore.WEB.Auth.Interfaces
{
    public interface IUserProvider
    {
        User User { get; set; }
    }
}