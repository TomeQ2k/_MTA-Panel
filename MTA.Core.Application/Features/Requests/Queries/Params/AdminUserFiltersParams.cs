using System;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Data.Repositories.Params;

namespace MTA.Core.Application.Features.Requests.Queries.Params
{
    public abstract record AdminUserFiltersParams : PaginationRequest, IAdminUserFiltersParams
    {
        public DateTime MinRegisterDate { get; init; }
        public DateTime MaxRegisterDate { get; init; } = DateTime.Now;
        public DateTime MinLastLogin { get; init; }
        public DateTime MaxLastLogin { get; init; } = DateTime.Now;
        public ActivatedStatusType ActivatedStatusType { get; init; }
        public AppStateType AppStateType { get; init; }
        public BanStatusType BanStatusType { get; init; }
        public string Ip { get; init; }
        public string Serial { get; init; }

        public UserSortType SortType { get; init; }
    }
}