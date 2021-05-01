using System;
using MTA.Core.Common.Enums;

namespace MTA.Core.Domain.Data.Repositories.Params
{
    public interface IAdminUserFiltersParams
    {
        DateTime MinRegisterDate { get; init; }
        DateTime MaxRegisterDate { get; init; }
        DateTime MinLastLogin { get; init; }
        DateTime MaxLastLogin { get; init; }
        ActivatedStatusType ActivatedStatusType { get; init; }
        AppStateType AppStateType { get; init; }
        BanStatusType BanStatusType { get; init; }
        string Ip { get; init; }
        string Serial { get; init; }

        UserSortType SortType { get; init; }
    }
}