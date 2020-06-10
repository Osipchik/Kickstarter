using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Company.API.Infrastructure;
using Company.API.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Company.API.Repositories
{
    public abstract class BaseRepository<TEntity>
        where TEntity: class, IEntity
    {
        protected readonly ApplicationContext Context;

        public BaseRepository(ApplicationContext context)
        {
            Context = context;
        }

        public async Task<TEntity> Add(TEntity entity)
        {
            await Context.Set<TEntity>().AddAsync(entity);
            await Context.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            await Context.SaveChangesAsync();
            return entity;
        }
        
        public async Task<TEntity> Delete(string id)
        {
            var entity = await Context.Set<TEntity>().FindAsync(id);
            if (entity == null)
            {
                return null;
            }

            Context.Set<TEntity>().Remove(entity);
            await Context.SaveChangesAsync();

            return entity;
        }

        public async Task<TEntity> Find(string id)
        {
            return await Context.Set<TEntity>().FindAsync(id);
        }

        public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression)
        {
            return Context.Set<TEntity>().Where(expression).AsNoTracking();
        }

        public IQueryable<TEntity> FindAll()
        {
            return Context.Set<TEntity>().AsNoTracking();
        }
    }
}