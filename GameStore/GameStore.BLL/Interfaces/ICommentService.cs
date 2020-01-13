using System.Collections.Generic;
using GameStore.Domain.Entities;

namespace GameStore.BLL.Interfaces
{
    public interface ICommentService
    {
        void AddComment(Comment comment);
        IEnumerable<Comment> GetAllCommentsByGame(string gameKey);
        Comment GetComment(int id);
        void Delete(int id);
    }
}