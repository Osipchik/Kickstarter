using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Company.Infrastructure.Filters
{
    public class QuantileFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.TryGetValue("quantile", out var value))
            {
                var quantile = (int?) value;
                if (quantile.HasValue && Math.Abs(quantile.Value) > 100)
                    context.Result = new BadRequestObjectResult("quantile must be between 0 and 100");
            }
        }
    }
}