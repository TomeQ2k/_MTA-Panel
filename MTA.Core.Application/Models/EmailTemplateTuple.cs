namespace MTA.Core.Application.Models
{
    public record EmailTemplateTuple
    (
        string Path,
        string[] AllowedParameters
    );
}