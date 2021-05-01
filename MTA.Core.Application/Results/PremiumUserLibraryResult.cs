using System.Collections.Generic;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Results
{
    public record PremiumUserLibraryResult
    (
        IEnumerable<PremiumFile> SkinFiles,
        IEnumerable<PremiumFile> InteriorFiles
    );
}