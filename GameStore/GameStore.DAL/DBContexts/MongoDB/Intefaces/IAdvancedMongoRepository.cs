namespace GameStore.DAL.DBContexts.MongoDB.Intefaces
{
    public interface IAdvancedMongoRepository<TEntity> : IMongoRepository<TEntity> where TEntity : class
    {
        void Remove(TEntity item);
        void Update(TEntity item);
    }
}