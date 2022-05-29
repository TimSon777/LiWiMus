using HashidsNet;
using LiWiMus.Web.FileServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace LiWiMus.Web.FileServer.Controllers;

[ApiController]
[Route("[controller]")]
public class FilesController : ControllerBase
{
    private readonly FileContext _context;
    private readonly string _filesPath;
    private readonly IHashids _hashids;
    private readonly HttpClient _client;

    public FilesController(IHostEnvironment environment, FileContext context, IHashids hashids,
                           IHttpClientFactory clientFactory)
    {
        _context = context;
        _hashids = hashids;
        _filesPath = Path.Combine(environment.ContentRootPath, "Files");
        _client = clientFactory.CreateClient();
    }

    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok("test");
    }

    [HttpPost("url")]
    public async Task<ActionResult<FileDto>> Save([FromBody] SaveUrlRequest request)
    {
        try
        {
            var response = await _client.GetAsync(request.Url);
            response.EnsureSuccessStatusCode();

            var contentType = response.Content.Headers.ContentType?.ToString();
            var fileName = Path.GetFileName(request.Url);

            var fileInfo = new FileInfo(fileName, contentType ?? "");

            _context.Files.Add(fileInfo);
            await _context.SaveChangesAsync();

            var path = Path.Combine(_filesPath, fileInfo.Id.ToString());

            await using var stream = System.IO.File.Create(path);
            await response.Content.CopyToAsync(stream);

            var location = Url.Action(nameof(Get), new {hashId = _hashids.Encode(fileInfo.Id)});
            return new FileDto(location!);
        }
        catch (Exception e)
        {
            return BadRequest();
        }
    }

    [HttpPost]
    public async Task<FileDto> Save([FromForm] SaveFileRequest request)
    {
        var file = request.File;
        var fileInfo = new FileInfo(file.FileName, file.ContentType);

        _context.Files.Add(fileInfo);
        await _context.SaveChangesAsync();

        var path = Path.Combine(_filesPath, fileInfo.Id.ToString());

        await using var stream = System.IO.File.Create(path);
        await file.CopyToAsync(stream);

        var location = Url.Action(nameof(Get), new {hashId = _hashids.Encode(fileInfo.Id)});
        return new FileDto(location!);
    }

    // ReSharper disable once RouteTemplates.ActionRoutePrefixCanBeExtractedToControllerRoute
    [HttpGet("{hashId}")]
    public async Task<IActionResult> Get(string hashId)
    {
        if (!_hashids.TryDecodeSingle(hashId, out var id))
        {
            return UnprocessableEntity();
        }

        var fileInfo = await _context.Files.FindAsync(id);
        if (fileInfo is null)
        {
            return UnprocessableEntity();
        }

        var path = Path.Combine(_filesPath, id.ToString());
        if (System.IO.File.Exists(path))
        {
            return File(System.IO.File.OpenRead(path), fileInfo.ContentType, fileInfo.DownloadName);
        }

        _context.Files.Remove(fileInfo);
        await _context.SaveChangesAsync();
        return UnprocessableEntity();
    }

    // ReSharper disable once RouteTemplates.ActionRoutePrefixCanBeExtractedToControllerRoute
    [HttpDelete("{hashId}")]
    public async Task<IActionResult> Delete(string hashId)
    {
        if (!_hashids.TryDecodeSingle(hashId, out var id))
        {
            return UnprocessableEntity();
        }

        var fileInfo = await _context.Files.FindAsync(id);
        if (fileInfo is null)
        {
            return UnprocessableEntity();
        }

        _context.Files.Remove(fileInfo);
        await _context.SaveChangesAsync();

        var path = Path.Combine(_filesPath, id.ToString());
        if (!System.IO.File.Exists(path))
        {
            return UnprocessableEntity();
        }

        System.IO.File.Delete(path);
        return NoContent();
    }
}