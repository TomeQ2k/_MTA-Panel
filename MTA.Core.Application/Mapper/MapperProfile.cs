using AutoMapper;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Models;
using MTA.Core.Common.Enums;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<User, UserListDto>();
            CreateMap<User, UserWithCharactersDto>()
                .ForMember(dest => dest.Activated, opt => opt.MapFrom(u => u.IsActivated))
                .ForMember(dest => dest.IsBlocked, opt => opt.MapFrom(u => u.IsBlocked));
            CreateMap<User, UserAuthDto>();
            CreateMap<User, MostActiveAdminUserDto>();
            CreateMap<User, UserAdminListDto>()
                .ForMember(dest => dest.Activated, opt => opt.MapFrom(u => u.IsActivated))
                .ForMember(dest => dest.IsBlocked, opt => opt.MapFrom(u => u.IsBlocked));
            CreateMap<User, UserAssigneeDto>()
                .ForMember(dest => dest.AssignedReportsCount, opt => opt.MapFrom(u => u.AssignedReports.Count));

            CreateMap<Character, CharacterDto>()
                .ForMember(dest => dest.Active, opt => opt.MapFrom(c => c.IsActive))
                .ForMember(dest => dest.IsDead, opt => opt.MapFrom(c => c.IsDead))
                .ForMember(dest => dest.IsFactionLeader, opt => opt.MapFrom(c => c.IsFactionLeader))
                .ForMember(dest => dest.FactionRankName,
                    opt => opt.MapFrom(c => c.Faction.GetFactionRankName(c.FactionRank)));
            CreateMap<Character, CharacterWithUserDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(c => c.User.Id))
                .ForMember(dest => dest.CharacterId, opt => opt.MapFrom(c => c.Id))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(c => c.User.Username))
                .ForMember(dest => dest.Charactername, opt => opt.MapFrom(c => c.Name));
            CreateMap<Character, CharacterAdminListDto>()
                .ForMember(dest => dest.Active, opt => opt.MapFrom(c => c.IsActive))
                .ForMember(dest => dest.IsDead, opt => opt.MapFrom(c => c.IsDead));
            CreateMap<Character, MostActiveCharacterDto>();
            CreateMap<Character, TopCharacterByMoneyDto>();

            CreateMap<Serial, SerialDto>();

            CreateMap<Faction, FactionDto>()
                .ForMember(dest => dest.FactionName, opt => opt.MapFrom(f => f.Name));
            CreateMap<Faction, TopFactionByBankBalanceDto>();

            CreateMap<Article, ArticleDto>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(a => StorageLocation.BuildLocation(a.ImageUrl)));
            CreateMap<UpdateArticleRequest, Article>();

            CreateMap<Changelog, ChangelogDto>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(c => StorageLocation.BuildLocation(c.ImageUrl)));

            CreateMap<UpdateChangelogRequest, Changelog>();

            CreateMap<Estate, EstateDto>();

            CreateMap<Vehicle, VehicleDto>()
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(v => v.VehiclesShop.VehicleBrand))
                .ForMember(dest => dest.Model, opt => opt.MapFrom(v => v.VehiclesShop.VehicleModel))
                .ForMember(dest => dest.Year, opt => opt.MapFrom(v => v.VehiclesShop.VehicleYear));

            CreateMap<Notification, NotificationDto>()
                .ForMember(dest => dest.IsRead, opt => opt.MapFrom(n => n.IsRead));

            CreateMap<Question, QuestionDto>();
            CreateMap<Domain.Entities.Application, ApplicationDto>();

            CreateMap<Report, ReportDto>()
                .ForMember(dest => dest.IsPrivate, opt => opt.MapFrom(r => r.Private))
                .ForMember(dest => dest.IsArchived, opt => opt.MapFrom(r => r.Archived));
            CreateMap<Report, ReportListDto>()
                .ForMember(dest => dest.IsPrivate, opt => opt.MapFrom(r => r.Private));

            CreateMap<PenaltyReport, PenaltyReportDto>();

            CreateMap<UserReport, UserReportDto>();

            CreateMap<BugReport, BugReportDto>();

            CreateMap<ReportComment, ReportCommentDto>()
                .ForMember(dest => dest.IsPrivate, opt => opt.MapFrom(r => r.Private));

            CreateMap<ReportSubscriber, ReportSubscriberDto>();

            CreateMap<ReportImage, ReportImageDto>()
                .ForMember(dest => dest.Url, opt => opt.MapFrom(ri => StorageLocation.BuildLocation(ri.Path)));

            CreateMap<Ban, PenaltyDto>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(b => b.DateCreated));

            CreateMap<AdminAction, AdminActionDto>();
            CreateMap<AdminAction, PenaltyDto>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(a => a.DateAdded));

            CreateMap<Order, OrderDto>();

            CreateMap<PremiumFile, PremiumFileDto>()
                .ForMember(dest => dest.Url, opt => opt.MapFrom(pf => StorageLocation.BuildLocation(pf.Path)));

            CreateMap<Purchase, PurchaseDto>()
                .ForMember(dest => dest.AccountName, opt => opt.MapFrom(p => p.Account.Username));

            CreateMap<MtaLog, MtaLogModel>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(l => l.Time));
            CreateMap<PhoneSms, MtaLogModel>()
                .ForMember(dest => dest.Action, opt => opt.MapFrom(_ => LogAction.PhoneSms))
                .ForMember(dest => dest.Source, opt => opt.MapFrom(p => p.From))
                .ForMember(dest => dest.Affected, opt => opt.MapFrom(p => p.To));

            CreateMap<GameItem, GameItemDto>()
                .ForMember(dest => dest.IsProtected, opt => opt.MapFrom(i => i.IsProtected));
            ;
        }
    }
}