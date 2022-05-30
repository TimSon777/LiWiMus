using FluentValidation;
using LiWiMus.Web.FileServer.Models;

namespace LiWiMus.Web.FileServer;

public class FileValidator : AbstractValidator<SaveFileRequest>
{
    public FileValidator()
    {
        const long maxLength = 10_000_000;

        RuleFor(request => request.File)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .Must(file => file.Length < maxLength);
    }
}