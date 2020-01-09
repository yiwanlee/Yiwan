using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryDemo.Repository.Core
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Insert(TEntity entity);
        void Delete(TEntity entity);
        void Update(TEntity entity);

        //TEntity SingleOrDefault(Expression<Func<TEntity, bool>> expression);

        //IQueryable<TEntity> All();
    }
}
