namespace LiWiMus.Core.Interfaces;

public interface IRazorViewRenderer
{
    Task<string> RenderViewAsync<TModel>(string viewName, TModel model);
}