using System.Collections.Generic;

namespace GameStore.DAL.Interfaces
{
    public interface ICrossAdapter<TEntity> : IAdapter<TEntity> where TEntity: class 
    {
        IEnumerable<TEntity> GetCross(int id, string crossProperty);
    }
}