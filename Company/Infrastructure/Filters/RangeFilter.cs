using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Company.Infrastructure.Filters
{
    public class RangeFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.TryGetValue("take", out var takeValue))
            {
                var take = (int) takeValue;
                if (take <= 5 || take > 200)
                    context.Result = new BadRequestObjectResult("range must be between 5 and 200");
            }

            if (context.ActionArguments.TryGetValue("skip", out var skipValue))
            {
                var skip = (int) skipValue;
                if (skip < 0) context.Result = new BadRequestObjectResult("skip must be not less then 0");
            }
        }
    }
}