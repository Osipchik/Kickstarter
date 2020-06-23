using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Funding.API.Infrastructure;
using Funding.API.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Funding.API.Repositories
{
    public class Repository<TEntity>
        where TEntity: class, IEntity
    {
        public readonly ApplicationContext _context;

        public Repository(ApplicationContext context)
        {
            _context = context;
        }
        
        public async ValueTask<TEntity> Add(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async ValueTask<TEntity> Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }
        
        public async ValueTask<TEntity> Delete(string id)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            if (entity == null)
            {
                return null;
            }

            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async ValueTask<TEntity> Find(string id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> filter)
        {
            return _context.Set<TEntity>().AsNoTracking().Where(filter).AsQueryable();
        }
    }
}