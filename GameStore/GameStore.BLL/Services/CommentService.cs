using GameStore.BLL.Interfaces;
using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace GameStore.BLL.Services
{
    public class CommentService : ICommentService
    {
        private IUnitOfWork unitOfWork { get; set; }

        public CommentService(IUnitOfWork uow)
        {
            unitOfWork = uow;
        }

        public void AddComment(Comment comment)
        {  
            unitOfWork.Comments.Create(comment);
        }

        public IEnumerable<Comment> GetAllCommentsByGame(string gameKey)
        {
            var game = unitOfWork.Games.Get(g => g.Key == gameKey).SingleOrDefault();

            var comments = unitOfWork.Comments.GetCross(game.Id, game.CrossProperty);

            return comments;
        }

        public Comment GetComment(int id)
        {
            var comment = unitOfWork.Comments.Get(
                p => p.Id == id).SingleOrDefault();

            return comment;
        }

        public void Delete(int id)
        {
            var comment = unitOfWork.Comments.Get(
                x => x.Id == id).SingleOrDefault();

            if (comment != null)
            {
                unitOfWork.Comments.Remove(comment);
            }
        }
    }
}
