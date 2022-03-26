using LiWiMus.Web.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LiWiMus.Web.Binders.ImageBinder;

public class ImageModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext ctx)
    {
        if (ctx == null)
        {
            throw new ArgumentNullException(nameof(ctx));
        }

        var formFiles = ctx.ActionContext.HttpContext.Request.Form.Files;

        if (!formFiles.Any())
        {
            return Task.CompletedTask;
        }
        
        var isOk = formFiles[0].TryParseToImage(out var image);

        if (!isOk)
        {
            ctx.Result = ModelBindingResult.Failed();
            return Task.CompletedTask;
        }
        
        ctx.Result = ModelBindingResult.Success(image);
        return Task.CompletedTask;
    }
}