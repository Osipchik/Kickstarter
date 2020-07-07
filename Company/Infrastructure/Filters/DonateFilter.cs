using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Company.Infrastructure.Filters
{
    public class DonateFilter : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.TryGetValue("amount", out var value))
            {
                var amount = (float) value;
                if (amount < 1) context.Result = new BadRequestObjectResult("min amount is 1");
            }
        }
    }
}