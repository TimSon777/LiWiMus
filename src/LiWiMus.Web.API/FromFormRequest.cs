#region

using System.Reflection;
using LiWiMus.Web.Shared;

#endregion

namespace LiWiMus.Web.API;

public abstract class FromFormRequest<TSelf> where TSelf : FromFormRequest<TSelf>, new()
{
    // ReSharper disable once MemberCanBeProtected.Global
    public static async ValueTask<TSelf?> BindAsync(HttpContext httpContext)
    {
        var form = httpContext.Request.Form;

        var obj = new TSelf();

        var type = typeof(TSelf);
        var properties = type.GetProperties().Where(p => p.CanWrite);

        foreach (var property in properties)
        {
            try
            {
                var value = await GetValueAsync(form, property);
                property.SetValue(obj, value);
            }
            catch (Exception)
            {
                return null;
            }
        }

        return obj;
    }

    private static async Task<object?> GetValueAsync(IFormCollection form, PropertyInfo property)
    {
        if (property.PropertyType.IsAssignableTo(typeof(IFormFile)))
        {
            return await GetFileAsync(form, property);
        }

        return GetSimpleValue(form, property);
    }

    private static async Task<IFormFile?> GetFileAsync(IFormCollection form, PropertyInfo property)
    {
        var file = form.Files.GetFile(property.Name);

        if (file is null)
        {
            return null;
        }

        if (property.PropertyType == typeof(ImageFormFile))
        {
            return await ImageFormFile.CreateAsync(file);
        }

        return file;
    }

    private static object? GetSimpleValue(IFormCollection form, PropertyInfo property)
    {
        string valueRaw = form[property.Name];
        object? value;

        if (valueRaw is null)
        {
            value = property.PropertyType.IsValueType ? Activator.CreateInstance(property.PropertyType) : null;
        }
        else
        {
            value = Convert.ChangeType(valueRaw, property.PropertyType);
        }

        return value;
    }
}