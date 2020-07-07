using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Company.Infrastructure;

namespace Company.Services.Repository
{
    public interface IRepository<TEntity>
        where TEntity : IEntity
    {
        IMapper Mapper { get; }

        Task<TEntity> Add(TEntity entity);

        Task<TEntity> Update(TEntity entity);

        Task<TEntity> Delete(TEntity entity);
        
        ValueTask<int> RemoveMany(Expression<Func<TEntity, bool>> expression);

        Task<TEntity> Find(string id);

        Task<TEntity> Find<TProperty>(string id, Expression<Func<TEntity, TProperty>> include)
            where TProperty : class;

        Task<TEntity> Find<TProperty>(string id, IEnumerable<Expression<Func<TEntity, TProperty>>> include)
            where TProperty : class;

        Task<List<TMap>> FindRange<TMap>(int take, int skip, Expression<Func<TEntity, bool>> expression)
            where TMap : class;

        Task<List<TMap>> FindRange<TMap, TProperty>(int take, int skip, Expression<Func<TEntity, bool>> expression,
            Expression<Func<TEntity, TProperty>> include)
            where TMap : class where TProperty : class;
    }
}