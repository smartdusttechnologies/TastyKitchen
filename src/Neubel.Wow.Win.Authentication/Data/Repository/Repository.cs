using Neubel.Wow.Win.Authentication.Core.Interfaces;
using System.Collections.Generic;

namespace Neubel.Wow.Win.Authentication.Data.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        public int Add(TEntity entity)
        {
            throw new System.NotImplementedException();
        }

        public int Add(IList<TEntity> list)
        {
            throw new System.NotImplementedException();
        }

        public int Delete(TEntity entity)
        {
            throw new System.NotImplementedException();
        }

        public IList<TEntity> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public TEntity GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public int Update(TEntity entity)
        {
            throw new System.NotImplementedException();
        }

        public int Update(IList<TEntity> list)
        {
            throw new System.NotImplementedException();
        }
    }
}
