using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Company.Infrastructure.Filters
{
    public class CategoryFilter : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.TryGetValue("category", out var value))
            {
                var category = (int) value;
                if (category == 0) context.Result = new BadRequestObjectResult("categoryId can't be zero");
            }
        }
    }
}