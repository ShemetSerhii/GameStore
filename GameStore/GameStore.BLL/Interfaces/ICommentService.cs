using System.Collections.Generic;
using System.Threading.Tasks;
using GameStore.Domain.Entities;

namespace GameStore.BLL.Interfaces
{
    public interface ICommentService
    {
        Task AddComment(Comment comment);

        Task<IEnumerable<Comment>> GetGamesComments(int gameId);

        Task<Comment> GetComment(int id);

        Task Delete(int id);
    }
}