using ByteSizeLib;
using LiWiMus.Core.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SixLabors.ImageSharp;

namespace LiWiMus.Web.Shared.Binders.ImageBinder;

public class ImageModelBinder : IModelBinder
{
    public async Task BindModelAsync(ModelBindingContext ctx)
    {
        if (ctx == null)
        {
            throw new ArgumentNullException(nameof(ctx));
        }

        var formFiles = ctx.ActionContext.HttpContext.Request.Form.Files;

        if (!formFiles.Any())
        {
            return;
        }

        Image image;
        try
        {
            image = await Image.LoadAsync(formFiles[0].OpenReadStream());
        }
        catch (ImageFormatException)
        {
            ctx.Result = ModelBindingResult.Failed();
            ctx.ModelState.AddModelError(ctx.FieldName, "Bad image format");
            return;
        }

        var extension = Path.GetExtension(formFiles[0].FileName);
        var imageInfo = new ImageInfo(ByteSize.FromBytes(formFiles[0].Length), extension, image);

        ctx.Result = ModelBindingResult.Success(imageInfo);
    }
}