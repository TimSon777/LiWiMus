#region

using Microsoft.AspNetCore.Mvc.ModelBinding;

#endregion

namespace LiWiMus.Web.Shared;

public class ImageFormFileModelBinder : IModelBinder
{
    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (!bindingContext.HttpContext.Request.HasFormContentType)
        {
            return;
        }

        var modelName = bindingContext.ModelName;
        var form = bindingContext.HttpContext.Request.Form;
        var file = form.Files.GetFile(modelName);

        if (file is null)
        {
            return;
        }

        var imageFormFile = await ImageFormFile.CreateAsync(file);

        bindingContext.Result = imageFormFile is not null
            ? ModelBindingResult.Success(imageFormFile)
            : ModelBindingResult.Failed();
    }
}