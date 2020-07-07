using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Story.API
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
            return controller.NotFound(new {Message = $"Item with id: {id} not found."});
        }
    }
}