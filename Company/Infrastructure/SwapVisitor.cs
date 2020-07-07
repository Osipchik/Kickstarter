using System;
using System.Linq.Expressions;

namespace Company.Infrastructure
{
    internal class SwapVisitor : ExpressionVisitor
    {
        private readonly Expression _from, _to;

        public SwapVisitor(Expression from, Expression to)
        {
            _from = from;
            _to = to;
        }

        public override Expression Visit(Expression node)
        {
            return node == _from ? _to : base.Visit(node);
        }

        public Expression<Func<TEntity, bool>> CombineExpressions<TEntity>(Expression<Func<TEntity, bool>> expr1,
            Expression<Func<TEntity, bool>> expr2)
            where TEntity : class
        {
            var expr = new SwapVisitor(expr1.Parameters[0], expr2.Parameters[0]).Visit(expr1.Body);
            var body = Expression.AndAlso(expr!, expr2.Body);

            return Expression.Lambda<Func<TEntity, bool>>(body, expr2.Parameters);
        }
    }
}