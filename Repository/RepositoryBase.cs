using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Repository
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected AppDbContext AppDbContext;

        public RepositoryBase(AppDbContext appDbContext)
        {
            AppDbContext = appDbContext;
        }

        public IQueryable<T> FindAll(bool trackChanges) =>
            !trackChanges ?
              AppDbContext.Set<T>().AsNoTracking() :
              AppDbContext.Set<T>();

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) =>
            !trackChanges ?
              AppDbContext.Set<T>().Where(expression).AsNoTracking() :
              AppDbContext.Set<T>().Where(expression);

        public void Create(T entity) => AppDbContext.Set<T>().Add(entity);
        public void Update(T entity) => AppDbContext.Set<T>().Update(entity);
        public void Delete(T entity) => AppDbContext.Set<T>().Remove(entity);
    }
}