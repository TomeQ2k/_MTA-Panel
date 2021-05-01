using System.Linq;
using System.Threading.Tasks;
using Ardalis.SmartEnum;
using MTA.Core.Application.Services;
using MTA.Core.Common.Enums;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.SmartEnums
{
    public abstract class LogActionPermissionSmartEnum : SmartEnum<LogActionPermissionSmartEnum>
    {
        protected LogActionPermissionSmartEnum(string name, int value) : base(name, value)
        {
        }

        public static LogActionPermissionSmartEnum ChatH = new ChatHType();
        public static LogActionPermissionSmartEnum ChatI = new ChatIType();
        public static LogActionPermissionSmartEnum ChatA = new ChatAType();
        public static LogActionPermissionSmartEnum AdminCommands = new AdminCommandsType();
        public static LogActionPermissionSmartEnum Anticheat = new AnticheatType();
        public static LogActionPermissionSmartEnum Vehicles = new VehiclesType();
        public static LogActionPermissionSmartEnum ChatIC = new ChatICType();
        public static LogActionPermissionSmartEnum ChatB = new ChatBType();
        public static LogActionPermissionSmartEnum ChatR = new ChatRType();
        public static LogActionPermissionSmartEnum ChatD = new ChatDType();
        public static LogActionPermissionSmartEnum ChatF = new ChatFType();
        public static LogActionPermissionSmartEnum ChatMe = new ChatMeType();
        public static LogActionPermissionSmartEnum ChatDistrict = new ChatDistrictType();
        public static LogActionPermissionSmartEnum ChatDo = new ChatDoType();
        public static LogActionPermissionSmartEnum ChatPm = new ChatPmType();
        public static LogActionPermissionSmartEnum ChatGov = new ChatGovType();
        public static LogActionPermissionSmartEnum ChatDon = new ChatDonType();
        public static LogActionPermissionSmartEnum ChatO = new ChatOType();
        public static LogActionPermissionSmartEnum ChatS = new ChatSType();
        public static LogActionPermissionSmartEnum ChatM = new ChatMType();
        public static LogActionPermissionSmartEnum ChatW = new ChatWType();
        public static LogActionPermissionSmartEnum ChatC = new ChatCType();
        public static LogActionPermissionSmartEnum ChatN = new ChatNType();
        public static LogActionPermissionSmartEnum SupporterChatG = new SupporterChatGType();
        public static LogActionPermissionSmartEnum TransferMoney = new TransferMoneyType();
        public static LogActionPermissionSmartEnum PremiumPanel = new PremiumPanelType();
        public static LogActionPermissionSmartEnum CharacterConnections = new CharacterConnectionsType();
        public static LogActionPermissionSmartEnum RoadBlockades = new RoadBlockadesType();
        public static LogActionPermissionSmartEnum PhoneCalls = new PhoneCallsType();
        public static LogActionPermissionSmartEnum PhoneSms = new PhoneSmsType();
        public static LogActionPermissionSmartEnum OpenCloseVehicles = new OpenCloseVehiclesType();
        public static LogActionPermissionSmartEnum Api = new ApiType();
        public static LogActionPermissionSmartEnum TransferPanelData = new TransferPanelDataType();
        public static LogActionPermissionSmartEnum Deaths = new DeathsType();
        public static LogActionPermissionSmartEnum FactionsPanel = new FactionsPanelType();
        public static LogActionPermissionSmartEnum AmmunationShops = new AmmunationShopsType();
        public static LogActionPermissionSmartEnum Interiors = new InteriorsType();
        public static LogActionPermissionSmartEnum Reports = new ReportsType();
        public static LogActionPermissionSmartEnum MoveItems = new MoveItemsType();
        public static LogActionPermissionSmartEnum ChatAme = new ChatAmeType();

        private static RoleType OwnerRole = Constants.OwnerRole;
        private static RoleType[] OwnersRoles = Constants.AllOwnersRoles;
        private static RoleType[] AdminsRoles = Constants.AdminRoles;
        private static RoleType[] AllAdminsRoles = Constants.AllAdminsRoles;
        private static RoleType[] SupportersRoles = Constants.AdminsAndSupportersRoles;
        private static RoleType[] MappersRoles = Constants.MappersRoles.Concat(Constants.AllOwnersRoles).ToArray();
        private static RoleType[] VctRoles = Constants.VctRoles.Concat(Constants.AllOwnersRoles).ToArray();
        private static RoleType[] ScriptersRoles = Constants.AllOwnersRoles.Concat(new[] {RoleType.Scripter}).ToArray();

        public abstract Task<bool> IsPermitted(IRolesService rolesService, int userId);

        private sealed class ChatHType : LogActionPermissionSmartEnum
        {
            public ChatHType() : base(nameof(ChatH), (int) LogAction.ChatH)
            {
            }

            public override async Task<bool> IsPermitted(IRolesService rolesService, int userId)
                => await rolesService.IsPermitted(userId, OwnerRole);
        }

        private sealed class ChatIType : LogActionPermissionSmartEnum
        {
            public ChatIType() : base(nameof(ChatI), (int) LogAction.ChatI)
            {
            }

            public override async Task<bool> IsPermitted(IRolesService rolesService, int userId)
                => await rolesService.IsPermitted(userId, OwnersRoles);
        }

        private sealed class ChatAType : LogActionPermissionSmartEnum
        {
            public ChatAType() : base(nameof(ChatA), (int) LogAction.ChatA)
            {
            }

            public override async Task<bool> IsPermitted(IRolesService rolesService, int userId)
                => await rolesService.IsPermitted(userId, OwnersRoles);
        }

        private sealed class AdminCommandsType : LogActionPermissionSmartEnum
        {
            public AdminCommandsType() : base(nameof(AdminCommands), (int) LogAction.AdminCommands)
            {
            }

            public override async Task<bool> IsPermitted(IRolesService rolesService, int userId)
                => await rolesService.IsPermitted(userId, AdminsRoles);
        }

        private sealed class AnticheatType : LogActionPermissionSmartEnum
        {
            public AnticheatType() : base(nameof(Anticheat), (int) LogAction.Anticheat)
            {
            }

            public override async Task<bool> IsPermitted(IRolesService rolesService, int userId)
                => await rolesService.IsPermitted(userId, SupportersRoles.Concat(ScriptersRoles).ToArray());
        }

        private sealed class VehiclesType : LogActionPermissionSmartEnum
        {
            public VehiclesType() : base(nameof(Vehicles), (int) LogAction.Vehicles)
            {
            }

            public override async Task<bool> IsPermitted(IRolesService rolesService, int userId)
                => await rolesService.IsPermitted(userId,
                    SupportersRoles.Concat(VctRoles).Concat(ScriptersRoles).ToArray());
        }

        private sealed class ChatICType : LogActionPermissionSmartEnum
        {
            public ChatICType() : base(nameof(ChatIC), (int) LogAction.ChatIC)
            {
            }

            public override async Task<bool> IsPermitted(IRolesService rolesService, int userId)
                => await rolesService.IsPermitted(userId, SupportersRoles);
        }

        private sealed class ChatBType : LogActionPermissionSmartEnum
        {
            public ChatBType() : base(nameof(ChatB), (int) LogAction.ChatB)
            {
            }

            public override async Task<bool> IsPermitted(IRolesService rolesService, int userId)
                => await rolesService.IsPermitted(userId, SupportersRoles);
        }

        private sealed class ChatRType : LogActionPermissionSmartEnum
        {
            public ChatRType() : base(nameof(ChatR), (int) LogAction.ChatR)
            {
            }

            public override async Task<bool> IsPermitted(IRolesService rolesService, int userId)
                => await rolesService.IsPermitted(userId, AllAdminsRoles);
        }

        private sealed class ChatDType : LogActionPermissionSmartEnum
        {
            public ChatDType() : base(nameof(ChatD), (int) LogAction.ChatD)
            {
            }

            public override async Task<bool> IsPermitted(IRolesService rolesService, int userId)
                => await rolesService.IsPermitted(userId, AllAdminsRoles);
        }

        private sealed class ChatFType : LogActionPermissionSmartEnum
        {
            public ChatFType() : base(nameof(ChatF), (int) LogAction.ChatF)
            {
            }

            public override async Task<bool> IsPermitted(IRolesService rolesService, int userId)
                => await rolesService.IsPermitted(userId, AllAdminsRoles);
        }

        private sealed class ChatMeType : LogActionPermissionSmartEnum
        {
            public ChatMeType() : base(nameof(ChatMe), (int) LogAction.ChatMe)
            {
            }

            public override async Task<bool> IsPermitted(IRolesService rolesService, int userId)
                => await rolesService.IsPermitted(userId, SupportersRoles);
        }

        private sealed class ChatDistrictType : LogActionPermissionSmartEnum
        {
            public ChatDistrictType() : base(nameof(ChatDistrict), (int) LogAction.ChatDistrict)
            {
            }

            public override async Task<bool> IsPermitted(IRolesService rolesService, int userId)
                => await rolesService.IsPermitted(userId, SupportersRoles);
        }

        private sealed class ChatDoType : LogActionPermissionSmartEnum
        {
            public ChatDoType() : base(nameof(ChatDo), (int) LogAction.ChatDo)
            {
            }

            public override async Task<bool> IsPermitted(IRolesService rolesService, int userId)
                => await rolesService.IsPermitted(userId, SupportersRoles);
        }

        private sealed class ChatPmType : LogActionPermissionSmartEnum
        {
            public ChatPmType() : base(nameof(ChatPm), (int) LogAction.ChatPm)
            {
            }

            public override async Task<bool> IsPermitted(IRolesService rolesService, int userId)
                => await rolesService.IsPermitted(userId, SupportersRoles);
        }

        private sealed class ChatGovType : LogActionPermissionSmartEnum
        {
            public ChatGovType() : base(nameof(ChatGov), (int) LogAction.ChatGov)
            {
            }

            public override async Task<bool> IsPermitted(IRolesService rolesService, int userId)
                => await rolesService.IsPermitted(userId, SupportersRoles);
        }

        private sealed class ChatDonType : LogActionPermissionSmartEnum
        {
            public ChatDonType() : base(nameof(ChatDon), (int) LogAction.ChatDon)
            {
            }

            public override async Task<bool> IsPermitted(IRolesService rolesService, int userId)
                => await rolesService.IsPermitted(userId, OwnerRole);
        }

        private sealed class ChatOType : LogActionPermissionSmartEnum
        {
            public ChatOType() : base(nameof(ChatO), (int) LogAction.ChatO)
            {
            }

            public override async Task<bool> IsPermitted(IRolesService rolesService, int userId)
                => await rolesService.IsPermitted(userId, AllAdminsRoles);
        }

        private sealed class ChatSType : LogActionPermissionSmartEnum
        {
            public ChatSType() : base(nameof(ChatS), (int) LogAction.ChatS)
            {
            }

            public override async Task<bool> IsPermitted(IRolesService rolesService, int userId)
                => await rolesService.IsPermitted(userId, SupportersRoles);
        }

        private sealed class ChatMType : LogActionPermissionSmartEnum
        {
            public ChatMType() : base(nameof(ChatM), (int) LogAction.ChatM)
            {
            }

            public override async Task<bool> IsPermitted(IRolesService rolesService, int userId)
                => await rolesService.IsPermitted(userId, SupportersRoles);
        }

        private sealed class ChatWType : LogActionPermissionSmartEnum
        {
            public ChatWType() : base(nameof(ChatW), (int) LogAction.ChatW)
            {
            }

            public override async Task<bool> IsPermitted(IRolesService rolesService, int userId)
                => await rolesService.IsPermitted(userId, SupportersRoles);
        }

        private sealed class ChatCType : LogActionPermissionSmartEnum
        {
            public ChatCType() : base(nameof(ChatC), (int) LogAction.ChatC)
            {
            }

            public override async Task<bool> IsPermitted(IRolesService rolesService, int userId)
                => await rolesService.IsPermitted(userId, SupportersRoles);
        }

        private sealed class ChatNType : LogActionPermissionSmartEnum
        {
            public ChatNType() : base(nameof(ChatN), (int) LogAction.ChatN)
            {
            }

            public override async Task<bool> IsPermitted(IRolesService rolesService, int userId)
                => await rolesService.IsPermitted(userId, SupportersRoles);
        }

        private sealed class SupporterChatGType : LogActionPermissionSmartEnum
        {
            public SupporterChatGType() : base(nameof(SupporterChatG), (int) LogAction.SupporterChatG)
            {
            }

            public override async Task<bool> IsPermitted(IRolesService rolesService, int userId)
                => await rolesService.IsPermitted(userId, SupportersRoles);
        }

        private sealed class TransferMoneyType : LogActionPermissionSmartEnum
        {
            public TransferMoneyType() : base(nameof(TransferMoney), (int) LogAction.TransferMoney)
            {
            }

            public override async Task<bool> IsPermitted(IRolesService rolesService, int userId)
                => await rolesService.IsPermitted(userId, SupportersRoles.Concat(ScriptersRoles).ToArray());
        }

        private sealed class PremiumPanelType : LogActionPermissionSmartEnum
        {
            public PremiumPanelType() : base(nameof(PremiumPanel), (int) LogAction.PremiumPanel)
            {
            }

            public override async Task<bool> IsPermitted(IRolesService rolesService, int userId)
                => await rolesService.IsPermitted(userId, OwnersRoles);
        }

        private sealed class CharacterConnectionsType : LogActionPermissionSmartEnum
        {
            public CharacterConnectionsType() : base(nameof(CharacterConnections),
                (int) LogAction.CharacterConnections)
            {
            }

            public override async Task<bool> IsPermitted(IRolesService rolesService, int userId)
                => await rolesService.IsPermitted(userId, SupportersRoles.Concat(ScriptersRoles).ToArray());
        }

        private sealed class RoadBlockadesType : LogActionPermissionSmartEnum
        {
            public RoadBlockadesType() : base(nameof(RoadBlockades),
                (int) LogAction.RoadBlockades)
            {
            }

            public override async Task<bool> IsPermitted(IRolesService rolesService, int userId)
                => await rolesService.IsPermitted(userId, SupportersRoles.Concat(ScriptersRoles).ToArray());
        }

        private sealed class PhoneCallsType : LogActionPermissionSmartEnum
        {
            public PhoneCallsType() : base(nameof(PhoneCalls),
                (int) LogAction.PhoneCalls)
            {
            }

            public override async Task<bool> IsPermitted(IRolesService rolesService, int userId)
                => await rolesService.IsPermitted(userId, SupportersRoles);
        }

        private sealed class PhoneSmsType : LogActionPermissionSmartEnum
        {
            public PhoneSmsType() : base(nameof(PhoneSms),
                (int) LogAction.PhoneSms)
            {
            }

            public override async Task<bool> IsPermitted(IRolesService rolesService, int userId)
                => await rolesService.IsPermitted(userId, SupportersRoles);
        }

        private sealed class OpenCloseVehiclesType : LogActionPermissionSmartEnum
        {
            public OpenCloseVehiclesType() : base(nameof(OpenCloseVehicles),
                (int) LogAction.OpenCloseVehicles)
            {
            }

            public override async Task<bool> IsPermitted(IRolesService rolesService, int userId)
                => await rolesService.IsPermitted(userId, AllAdminsRoles.Concat(VctRoles).ToArray());
        }

        private sealed class ApiType : LogActionPermissionSmartEnum
        {
            public ApiType() : base(nameof(Api), (int) LogAction.Api)
            {
            }

            public override async Task<bool> IsPermitted(IRolesService rolesService, int userId)
                => await rolesService.IsPermitted(userId, OwnerRole);
        }

        private sealed class TransferPanelDataType : LogActionPermissionSmartEnum
        {
            public TransferPanelDataType() : base(nameof(TransferPanelData), (int) LogAction.TransferPanelData)
            {
            }

            public override async Task<bool> IsPermitted(IRolesService rolesService, int userId)
                => await rolesService.IsPermitted(userId, SupportersRoles);
        }

        private sealed class DeathsType : LogActionPermissionSmartEnum
        {
            public DeathsType() : base(nameof(Deaths), (int) LogAction.Deaths)
            {
            }

            public override async Task<bool> IsPermitted(IRolesService rolesService, int userId)
                => await rolesService.IsPermitted(userId, SupportersRoles);
        }

        private sealed class FactionsPanelType : LogActionPermissionSmartEnum
        {
            public FactionsPanelType() : base(nameof(FactionsPanel), (int) LogAction.FactionsPanel)
            {
            }

            public override async Task<bool> IsPermitted(IRolesService rolesService, int userId)
                => await rolesService.IsPermitted(userId, AllAdminsRoles);
        }

        private sealed class AmmunationShopsType : LogActionPermissionSmartEnum
        {
            public AmmunationShopsType() : base(nameof(AmmunationShops), (int) LogAction.AmmunationShops)
            {
            }

            public override async Task<bool> IsPermitted(IRolesService rolesService, int userId)
                => await rolesService.IsPermitted(userId, AllAdminsRoles);
        }

        private sealed class InteriorsType : LogActionPermissionSmartEnum
        {
            public InteriorsType() : base(nameof(Interiors), (int) LogAction.Interiors)
            {
            }

            public override async Task<bool> IsPermitted(IRolesService rolesService, int userId)
                => await rolesService.IsPermitted(userId,
                    SupportersRoles.Concat(MappersRoles).Concat(ScriptersRoles).ToArray());
        }

        private sealed class ReportsType : LogActionPermissionSmartEnum
        {
            public ReportsType() : base(nameof(Reports), (int) LogAction.Reports)
            {
            }

            public override async Task<bool> IsPermitted(IRolesService rolesService, int userId)
                => await rolesService.IsPermitted(userId, OwnersRoles);
        }

        private sealed class MoveItemsType : LogActionPermissionSmartEnum
        {
            public MoveItemsType() : base(nameof(MoveItems), (int) LogAction.MoveItems)
            {
            }

            public override async Task<bool> IsPermitted(IRolesService rolesService, int userId)
                => await rolesService.IsPermitted(userId, SupportersRoles);
        }

        private sealed class ChatAmeType : LogActionPermissionSmartEnum
        {
            public ChatAmeType() : base(nameof(ChatAme), (int) LogAction.ChatAme)
            {
            }

            public override async Task<bool> IsPermitted(IRolesService rolesService, int userId)
                => await rolesService.IsPermitted(userId, SupportersRoles);
        }
    }
}