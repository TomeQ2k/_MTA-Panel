using System;
using System.Collections.Generic;
using System.Linq;
using MTA.Core.Common.Enums;

namespace MTA.Core.Application.Logging
{
    public class LogActionPermissionDictionary : Dictionary<RoleType, LogAction[]>
    {
        public static LogActionPermissionDictionary Build() => new LogActionPermissionDictionary()
        {
            {RoleType.Owner, GetLogActionsExcept()},
            {
                RoleType.ViceOwner,
                GetLogActionsExcept(new[] {LogAction.ChatH, LogAction.ChatDon, LogAction.Api})
            },
            {
                RoleType.Admin,
                GetLogActionsExcept(new[]
                {
                    LogAction.ChatH, LogAction.ChatDon, LogAction.Api, LogAction.ChatI, LogAction.ChatA,
                    LogAction.PremiumPanel, LogAction.Reports
                })
            },
            {
                RoleType.TrialAdmin,
                GetLogActionsExcept(new[]
                {
                    LogAction.ChatH, LogAction.ChatDon, LogAction.Api, LogAction.ChatI, LogAction.ChatA,
                    LogAction.PremiumPanel, LogAction.Reports, LogAction.AdminCommands
                })
            },
            {
                RoleType.SupporterLeader,
                GetLogActionsExcept(new[]
                {
                    LogAction.ChatH, LogAction.ChatDon, LogAction.Api, LogAction.ChatI, LogAction.ChatA,
                    LogAction.PremiumPanel, LogAction.Reports, LogAction.AdminCommands, LogAction.ChatR,
                    LogAction.ChatD, LogAction.ChatF, LogAction.ChatO, LogAction.OpenCloseVehicles,
                    LogAction.FactionsPanel, LogAction.AmmunationShops
                })
            },
            {
                RoleType.Supporter,
                GetLogActionsExcept(new[]
                {
                    LogAction.ChatH, LogAction.ChatDon, LogAction.Api, LogAction.ChatI, LogAction.ChatA,
                    LogAction.PremiumPanel, LogAction.Reports, LogAction.AdminCommands, LogAction.ChatR,
                    LogAction.ChatD, LogAction.ChatF, LogAction.ChatO, LogAction.OpenCloseVehicles,
                    LogAction.FactionsPanel, LogAction.AmmunationShops
                })
            },
            {RoleType.MapperLeader, new[] {LogAction.Interiors}},
            {RoleType.Mapper, new[] {LogAction.Interiors}},
            {RoleType.VctLeader, new[] {LogAction.Vehicles, LogAction.OpenCloseVehicles}},
            {RoleType.Vct, new[] {LogAction.Vehicles, LogAction.OpenCloseVehicles}},
            {
                RoleType.Scripter, new[]
                {
                    LogAction.Anticheat, LogAction.Vehicles, LogAction.TransferMoney, LogAction.CharacterConnections,
                    LogAction.RoadBlockades, LogAction.Interiors
                }
            },
        };

        #region private

        private static LogAction[] GetLogActionsExcept(params LogAction[] exceptLogActions)
            => Enum.GetValues<LogAction>().Except(exceptLogActions).ToArray();

        #endregion
    }
}