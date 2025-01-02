namespace InteractReef.Database.Core
{
	public interface IRepository<TEntity> where TEntity : IEntity
	{
		TEntity Add(TEntity entity);
		TEntity Update(int id, TEntity entity);
		void Delete(TEntity entity);
		IQueryable<TEntity> GetAll();
		TEntity? GetById(int id);
	}
}
