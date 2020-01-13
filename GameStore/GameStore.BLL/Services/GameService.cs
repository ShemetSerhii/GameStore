using GameStore.BLL.Interfaces;
using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.BLL.Services
{
    public class GameService : IGameService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GameService(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        public Task Create(Game game)
        {
            _unitOfWork.GameRepository.Create(game);

            return _unitOfWork.SaveAsync();
        }

        public Task Update(Game game)
        {
            _unitOfWork.GameRepository.Update(game);

            return _unitOfWork.SaveAsync();
        }

        public async Task Delete(int id)
        {
            var game = await _unitOfWork.GameRepository.GetAsync(id);

            _unitOfWork.GameRepository.Delete(game);

            await _unitOfWork.SaveAsync();
        }

        public Task<IEnumerable<Game>> GetAll()
        {
            return _unitOfWork.GameRepository.GetAsync();
        }

        public IEnumerable<Game> Pagination(IEnumerable<Game> games, int page, int pageSize)
        {
            var result = games.Skip((page - 1) * pageSize).Take(pageSize);

            return result;
        }

        public async Task<Game> GetByKey(string key)
        {
            var games = await _unitOfWork.GameRepository.GetAsync(g => g.Key == key);

            return games.SingleOrDefault();
        }

        public Task<IEnumerable<Game>> GetByGenre(Genre genre)
        {
            return _unitOfWork.GameRepository.GetAsync(game => game.Genres.Contains(genre));
        }

        public Task<IEnumerable<Game>> GetByPlatformType(PlatformType platformType)
        {
            return _unitOfWork.GameRepository.GetAsync(game => game.PlatformTypes.Contains(platformType));
        }
    }
}
