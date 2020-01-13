using GameStore.BLL.Interfaces;
using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameStore.BLL.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork; 

        public CommentService(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        public Task AddComment(Comment comment)
        {
            _unitOfWork.CommentRepository.Create(comment);

            return _unitOfWork.SaveAsync();
        }

        public Task<IEnumerable<Comment>> GetGamesComments(int gameId)
        {
            return _unitOfWork.CommentRepository.GetAsync(comment => comment.GameId == gameId);
        }

        public Task<Comment> GetComment(int id)
        {
            return _unitOfWork.CommentRepository.GetAsync(id);
        }

        public async Task Delete(int id)
        {
            var comment = await _unitOfWork.CommentRepository.GetAsync(id);

            if (comment != null)
            {
                _unitOfWork.CommentRepository.Delete(comment);
            }
        }
    }
}
