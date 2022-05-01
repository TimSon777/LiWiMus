using SixLabors.ImageSharp;

namespace LiWiMus.Web.API.Playlists.Update;

public class Request : FromFormRequest<Request>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public bool IsPublic { get; set; }
    public IFormFile Photo { get; set; } = null!;
    public IImageInfo PhotoInfo { get; set; } = null!;

    public new static async ValueTask<Request?> BindAsync(HttpContext httpContext)
    {
        var request = await FromFormRequest<Request>.BindAsync(httpContext);
        if (request?.Photo is not null)
        {
            request.PhotoInfo = await Image.IdentifyAsync(request.Photo.OpenReadStream());
        }

        return request;
    }
}