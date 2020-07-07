using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Company.Infrastructure.Filters
{
    public class FileValidator : ActionFilterAttribute
    {
        private readonly int _maxSize;
        private readonly string[] AllowedFileExtensions =
        {
            "image/bmp", "image/gif", "image/jpeg", "image/png", "image/webp", "image/tiff"
        };

        public FileValidator(int maxSize)
        {
            _maxSize = maxSize;
        }
        
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var (_, value) = context.ActionArguments.FirstOrDefault(x => x.Value is IFormFile);

            if (value != null)
            {
                var fileArg = value as IFormFile;

                if (AllowedFileExtensions.All(x => x != fileArg.ContentType))
                {
                    context.Result = new BadRequestObjectResult($"The content-type '{fileArg.ContentType}' is not valid.");
                    return;
                }

                if (fileArg.Length > _maxSize)
                {
                    context.Result = new BadRequestObjectResult($"File is too big, max file size: {_maxSize}");
                    return;
                }
            }
        }
    }
}