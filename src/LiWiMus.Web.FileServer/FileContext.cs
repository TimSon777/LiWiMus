using Microsoft.EntityFrameworkCore;

namespace LiWiMus.Web.FileServer;

public class FileContext : DbContext
{
    public DbSet<FileInfo> Files => Set<FileInfo>();

    public FileContext(DbContextOptions<FileContext> options) : base(options)
    {
    }
}