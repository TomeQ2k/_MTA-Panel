using System;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Data.Repositories.Params;

namespace MTA.Core.Application.Logic.Requests.Queries.Params
{
    public abstract record AdminCharacterFiltersParams : PaginationRequest, IAdminCharacterFiltersParams
    {
        public DateTime MinLastLogin { get; init; }
        public DateTime MaxLastLogin { get; init; } = DateTime.Now;
        public DateTime MinCreationDate { get; init; }
        public DateTime MaxCreationDate { get; init; } = DateTime.Now;
        public string Name { get; init; }
        public GenderStatusType GenderStatusType { get; init; }
        public ActiveStatusType ActiveStatusType { get; init; }
        public CharacterDeadStatusType DeadStatusType { get; init; }

        public CharacterSortType SortType { get; init; }
    }
}