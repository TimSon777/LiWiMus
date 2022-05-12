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

    public FilesController(IHostEnvironment environment, FileContext context, IHashids hashids)
    {
        _context = context;
        _hashids = hashids;
        _filesPath = Path.Combine(environment.ContentRootPath, "Files");
    }

    [HttpPost]
    public async Task<FileDto> Save([FromForm] SaveFileRequest request)
    {
        var file = request.File;
        var fileInfo = new FileInfo(file.FileName, file.ContentType);

        _context.Files.Add(fileInfo);
        await _context.SaveChangesAsync();

        var id = _hashids.Encode(fileInfo.Id);

        var path = Path.Combine(_filesPath, id);

        await using var stream = System.IO.File.Create(path);
        await file.CopyToAsync(stream);

        var location = Url.Action(nameof(Get), new {hashId = id});
        return new FileDto(location!);
    }

    // ReSharper disable once RouteTemplates.ActionRoutePrefixCanBeExtractedToControllerRoute
    [HttpGet("{hashId}")]
    public async Task<IActionResult> Get(string hashId)
    {
        var id = _hashids.DecodeSingle(hashId);
        var fileInfo = await _context.Files.FindAsync(id);
        if (fileInfo is null)
        {
            return UnprocessableEntity();
        }

        var path = Path.Combine(_filesPath, hashId);
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
        var id = _hashids.DecodeSingle(hashId);
        var fileInfo = await _context.Files.FindAsync(id);
        if (fileInfo is null)
        {
            return UnprocessableEntity();
        }

        _context.Files.Remove(fileInfo);
        await _context.SaveChangesAsync();

        var path = Path.Combine(_filesPath, hashId);
        if (!System.IO.File.Exists(path))
        {
            return UnprocessableEntity();
        }

        System.IO.File.Delete(path);
        return NoContent();
    }
}