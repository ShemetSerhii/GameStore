namespace GameStore.DAL.Interfaces
{
    public interface IAdapter<TEntity> : IBaseAdapter<TEntity> where TEntity : class
    {
        void Create(TEntity item);
        void Remove(TEntity item);
        void Update(TEntity item);
    }
}