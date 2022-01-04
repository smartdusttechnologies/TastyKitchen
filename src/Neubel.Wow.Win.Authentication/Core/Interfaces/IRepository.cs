using System.Collections.Generic;

namespace Neubel.Wow.Win.Authentication.Core.Interfaces
{
    /// <summary>
    /// This is an generic repository interface
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        IList<TEntity> GetAll();

        TEntity GetById(int id);

        int Add(TEntity entity);

        int Add(IList<TEntity> list);

        int Update(TEntity entity);

        int Update(IList<TEntity> list);

        int Delete(TEntity entity);
    }
}
