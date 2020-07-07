using System;
using System.Linq.Expressions;
using Company.Data;
using Company.Infrastructure;
using Company.Models;

namespace Company.Services
{
    public static class PreviewExpressionBuilder
    {
        public static Expression<Func<Preview, bool>> Build(CompanyStatus status, int? quantile, int categoryId = 0)
        {
            Expression<Func<Preview, bool>> filter = i => i.Status == status;

            if (quantile.HasValue) filter = AddQuantileFilter(filter, quantile.Value);

            if (categoryId != 0) filter = AddCategoryFilter(filter, categoryId);

            return filter;
        }

        private static Expression<Func<Preview, bool>> AddCategoryFilter(Expression<Func<Preview, bool>> filter,
            int categoryId)
        {
            var quantileFilter = CategoryFilter(categoryId);
            var swap = new SwapVisitor(filter, quantileFilter);

            return swap.CombineExpressions(filter, quantileFilter);
        }

        private static Expression<Func<Preview, bool>> CategoryFilter(int categoryId)
        {
            Expression<Func<Preview, bool>> expression;

            if (categoryId > 0)
                expression = i => i.CategoryId == categoryId;
            else
                expression = i => i.SubCategoryId == -categoryId;

            return expression;
        }

        private static Expression<Func<Preview, bool>> AddQuantileFilter(Expression<Func<Preview, bool>> filter,
            int quantile)
        {
            var quantileFilter = QuantileFilter(quantile);
            var swap = new SwapVisitor(filter, quantileFilter);

            return swap.CombineExpressions(filter, quantileFilter);
        }

        private static Expression<Func<Preview, bool>> QuantileFilter(int quantile)
        {
            Expression<Func<Preview, bool>> expression;

            if (quantile >= 0)
                expression = i => i.Funding.Funded >= quantile;
            else
                expression = i => i.Funding.Funded <= -quantile;

            return expression;
        }
    }
}