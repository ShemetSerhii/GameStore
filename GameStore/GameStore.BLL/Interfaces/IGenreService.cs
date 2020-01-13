using GameStore.Domain.Entities;
using System.Collections.Generic;

namespace GameStore.BLL.Interfaces
{
    public interface IGenreService
    {
        IEnumerable<Genre> GetAll();
        Genre Get(string name);
        Genre GetGenreByByInterimProperty(int id, string interimProperty);
        void Create(Genre genre);
        void Update(Genre genre);
        void Delete(string name);
    }
}