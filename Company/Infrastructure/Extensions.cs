using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Company.Infrastructure
{
    public static class Extensions
    {
        private const string UserIdClaimType = "sub";

        public static string UserId(this HttpContext context)
        {
            return context.User.Claims.FirstOrDefault(claim => claim.Type == UserIdClaimType)?.Value;
        }

        public static NotFoundObjectResult NotFoundResponse(this ControllerBase controller, string id)
        {
            return controller.NotFound($"Item with id: {id} not found.");
        }

        public static bool IsUserHasAccess(this HttpContext context, IEntity entity)
        {
            return context.User.IsInRole(UserRoles.Admin) || entity.OwnerId == context.UserId();
        }

        public static bool IsEntityCanBeChanged(this ControllerBase controller, IEntity entity, string id, out IActionResult result)
        {
            result = null;
            
            if (entity == null)
            {
                result = controller.BadRequest($"Item with id: {id} not found");
            }
            else if (!controller.HttpContext.IsUserHasAccess(entity))
            {
                result = controller.Forbid();
            }
            
            return result == null;
        }
    }
}