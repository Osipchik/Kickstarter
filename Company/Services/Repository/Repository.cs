using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Company.Data;
using Company.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Company.Services.Repository
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity
    {
        private readonly AppDbContext _context;

        public Repository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            Mapper = mapper;
        }

        public IMapper Mapper { get; }

        public async Task<TEntity> Add(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<TEntity> Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<TEntity> Find(string id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity> Find<TProperty>(string id, Expression<Func<TEntity, TProperty>> include)
            where TProperty : class
        {
            return await _context.Set<TEntity>().Include(include).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<TEntity> Find<TProperty>(string id, IEnumerable<Expression<Func<TEntity, TProperty>>> include)
            where TProperty : class
        {
            var query = _context.Set<TEntity>();

            foreach (var expression in include) query.Include(expression);

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<List<TMap>> FindRange<TMap>(int take, int skip, Expression<Func<TEntity, bool>> expression)
            where TMap : class
        {
            var query = _context.Set<TEntity>().AsNoTracking().Where(expression).Skip(skip).Take(take);

            return await query.Select(i => Mapper.Map<TMap>(i)).ToListAsync();
        }

        public async Task<List<TMap>> FindRange<TMap, TProperty>(int take, int skip,
            Expression<Func<TEntity, bool>> expression,
            Expression<Func<TEntity, TProperty>> include)
            where TMap : class where TProperty : class
        {
            var query = _context.Set<TEntity>().AsNoTracking().Where(expression).Skip(skip).Take(take).Include(include);

            return await query.Select(i => Mapper.Map<TMap>(i)).ToListAsync();
        }

        public async ValueTask<int> RemoveMany(Expression<Func<TEntity, bool>> expression)
        {
            var entities = _context.Set<TEntity>().Where(expression);
            var count = entities.Count();
            _context.Set<TEntity>().RemoveRange(entities);
            await _context.SaveChangesAsync();

            return count;
        }
    }
}