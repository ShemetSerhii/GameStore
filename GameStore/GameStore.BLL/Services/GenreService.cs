using GameStore.BLL.Interfaces;
using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameStore.BLL.Services
{
    public class GenreService : IGenreService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GenreService(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        public Task<IEnumerable<Genre>> GetAll()
        {
            return _unitOfWork.GenreRepository.GetAsync();
        }

        public Task<Genre> Get(int id)
        {
            return _unitOfWork.GenreRepository.GetAsync(id);
        }

        public Task Create(Genre genre)
        {
            _unitOfWork.GenreRepository.Create(genre);

            return _unitOfWork.SaveAsync();
        }

        public Task Update(Genre genre)
        {
            _unitOfWork.GenreRepository.Update(genre);

            return _unitOfWork.SaveAsync();
        }

        public async Task Delete(int id)
        {
            var genre = await _unitOfWork.GenreRepository.GetAsync(id);

            if (genre != null)
            {
                _unitOfWork.GenreRepository.Delete(genre);

                await _unitOfWork.SaveAsync();
            }
        }
    }
}
