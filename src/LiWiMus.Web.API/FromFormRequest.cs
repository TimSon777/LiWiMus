using System.Reflection;

namespace LiWiMus.Web.API;

public abstract class FromFormRequest<TSelf> where TSelf : FromFormRequest<TSelf>, new()
{
    // ReSharper disable once MemberCanBeProtected.Global
    public static ValueTask<TSelf?> BindAsync(HttpContext httpContext)
    {
        var form = httpContext.Request.Form;

        var obj = new TSelf();

        var type = typeof(TSelf);
        var properties = type.GetProperties().Where(p => p.CanWrite);

        foreach (var property in properties)
        {
            try
            {
                var value = GetValue(form, property);
                property.SetValue(obj, value);
            }
            catch (Exception)
            {
                return ValueTask.FromResult<TSelf?>(null);
            }
        }

        return ValueTask.FromResult<TSelf?>(obj);
    }

    private static object? GetValue(IFormCollection form, PropertyInfo property)
    {
        var propName = property.Name;
        var propType = property.PropertyType;

        var file = form.Files.FirstOrDefault(f => f.Name.Equals(propName, StringComparison.InvariantCultureIgnoreCase));
        if (file is not null)
        {
            return file;
        }

        string valueRaw = form[property.Name];
        object? value;

        if (valueRaw is null)
        {
            value = propType.IsValueType ? Activator.CreateInstance(propType) : null;
        }
        else
        {
            value = Convert.ChangeType(valueRaw, propType);
        }

        return value;
    }
}