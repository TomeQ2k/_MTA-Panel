namespace MTA.Core.Application.Models
{
    public record FileModel
    (
        string Path,
        string Url,
        long Size
    );
}