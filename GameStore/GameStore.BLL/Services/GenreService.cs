using GameStore.BLL.Interfaces;
using GameStore.DAL.Interfaces;
using GameStore.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace GameStore.BLL.Services
{
    public class GenreService : IGenreService
    {
        private IUnitOfWork unitOfWork { get; set; }

        public GenreService(IUnitOfWork uow)
        {
            unitOfWork = uow;
        }

        public IEnumerable<Genre> GetAll()
        {
            var games = unitOfWork.Genres.Get();

            return games;
        }

        public Genre Get(string name)
        {
            var genre = unitOfWork.Genres.Get(x => x.Name == name).SingleOrDefault();

            return genre;
        }

        public Genre GetGenreByByInterimProperty(int id, string crossProperty)
        {
            var genre = unitOfWork.Genres.GetCross(id, crossProperty).SingleOrDefault();

            return genre;
        }

        public void Create(Genre genre)
        {
            if (genre != null)
            {
                unitOfWork.Genres.Create(genre);
            }
        }

        public void Update(Genre genre)
        {
            if (genre != null)
            {
                unitOfWork.Genres.Update(genre);
            }
        }

        public void Delete(string name)
        {
            var genre = Get(name);

            if (genre != null)
            {
                unitOfWork.Genres.Remove(genre);
            }
        }
    }
}
