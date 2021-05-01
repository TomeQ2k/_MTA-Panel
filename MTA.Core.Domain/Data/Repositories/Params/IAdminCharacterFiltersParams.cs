using System;
using MTA.Core.Common.Enums;

namespace MTA.Core.Domain.Data.Repositories.Params
{
    public interface IAdminCharacterFiltersParams
    {
        DateTime MinLastLogin { get; init; }
        DateTime MaxLastLogin { get; init; }
        DateTime MinCreationDate { get; init; }
        DateTime MaxCreationDate { get; init; }
        string Name { get; init; }
        GenderStatusType GenderStatusType { get; init; }
        ActiveStatusType ActiveStatusType { get; init; }
        CharacterDeadStatusType DeadStatusType { get; init; }

        CharacterSortType SortType { get; init; }
    }
}