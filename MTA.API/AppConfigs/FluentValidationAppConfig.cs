using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Features.Requests.Queries;

namespace MTA.API.AppConfigs
{
    public static class FluentValidationAppConfig
    {
        public static IMvcBuilder ConfigureFluentValidation(this IMvcBuilder mvcBuilder)
            => mvcBuilder.AddFluentValidation(v =>
            {
                v.RegisterValidatorsFromAssemblyContaining<SignInRequest>();
                v.RegisterValidatorsFromAssemblyContaining<SignUpRequest>();
                v.RegisterValidatorsFromAssemblyContaining<ConfirmAccountRequest>();
                v.RegisterValidatorsFromAssemblyContaining<ResetPasswordRequest>();
                v.RegisterValidatorsFromAssemblyContaining<SendResetPasswordRequest>();
                v.RegisterValidatorsFromAssemblyContaining<VerifyResetPasswordRequest>();
                v.RegisterValidatorsFromAssemblyContaining<GetAuthValidationsRequest>();
                v.RegisterValidatorsFromAssemblyContaining<EditEmailTemplateRequest>();
                v.RegisterValidatorsFromAssemblyContaining<SendActivationEmailRequest>();
                v.RegisterValidatorsFromAssemblyContaining<GetArticleRequest>();
                v.RegisterValidatorsFromAssemblyContaining<CreateArticleRequest>();
                v.RegisterValidatorsFromAssemblyContaining<UpdateArticleRequest>();
                v.RegisterValidatorsFromAssemblyContaining<DeleteArticleRequest>();
                v.RegisterValidatorsFromAssemblyContaining<CreateChangelogRequest>();
                v.RegisterValidatorsFromAssemblyContaining<UpdateChangelogRequest>();
                v.RegisterValidatorsFromAssemblyContaining<DeleteChangelogRequest>();
                v.RegisterValidatorsFromAssemblyContaining<GetUserWithCharactersRequest>();
                v.RegisterValidatorsFromAssemblyContaining<GetCharacterRequest>();
                v.RegisterValidatorsFromAssemblyContaining<ToggleBlockCharacterRequest>();
                v.RegisterValidatorsFromAssemblyContaining<CleanAccountRequest>();
                v.RegisterValidatorsFromAssemblyContaining<ChangeEmailRequest>();
                v.RegisterValidatorsFromAssemblyContaining<ChangePasswordRequest>();
                v.RegisterValidatorsFromAssemblyContaining<SendResetPasswordByAdminRequest>();
                v.RegisterValidatorsFromAssemblyContaining<SendChangeEmailEmailByAdminRequest>();
                v.RegisterValidatorsFromAssemblyContaining<AddSerialRequest>();
                v.RegisterValidatorsFromAssemblyContaining<DeleteSerialRequest>();
                v.RegisterValidatorsFromAssemblyContaining<SendAddSerialEmailRequest>();
                v.RegisterValidatorsFromAssemblyContaining<GetUserSerialsRequest>();
                v.RegisterValidatorsFromAssemblyContaining<BlockAccountRequest>();
                v.RegisterValidatorsFromAssemblyContaining<AddCreditsRequest>();
                v.RegisterValidatorsFromAssemblyContaining<MarkAsReadNotificationRequest>();
                v.RegisterValidatorsFromAssemblyContaining<GetPartQuestionsRequest>();
                v.RegisterValidatorsFromAssemblyContaining<PassRPTestPartOneRequest>();
                v.RegisterValidatorsFromAssemblyContaining<GenerateAnswersForPartTwoRequest>();
                v.RegisterValidatorsFromAssemblyContaining<ReviewRPTestRequest>();
                v.RegisterValidatorsFromAssemblyContaining<GetCharactersWithUserByCharacternameRequest>();
                v.RegisterValidatorsFromAssemblyContaining<GetAdminActionsByActionAndUserIdRequest>();
                v.RegisterValidatorsFromAssemblyContaining<CreateUserReportRequest>();
                v.RegisterValidatorsFromAssemblyContaining<CreateBugReportRequest>();
                v.RegisterValidatorsFromAssemblyContaining<BaseCreateReportRequest>();
                v.RegisterValidatorsFromAssemblyContaining<CreateOtherReportRequest>();
                v.RegisterValidatorsFromAssemblyContaining<CreatePenaltyReportRequest>();
                v.RegisterValidatorsFromAssemblyContaining<GetReportsAllowedAssigneesRequest>();
                v.RegisterValidatorsFromAssemblyContaining<AttachReportImagesRequest>();
                v.RegisterValidatorsFromAssemblyContaining<AddReportCommentRequest>();
                v.RegisterValidatorsFromAssemblyContaining<AddReportSubscriberRequest>();
                v.RegisterValidatorsFromAssemblyContaining<RemoveReportSubscriberRequest>();
                v.RegisterValidatorsFromAssemblyContaining<TogglePrivacyReportRequest>();
                v.RegisterValidatorsFromAssemblyContaining<AcceptReportAssignmentRequest>();
                v.RegisterValidatorsFromAssemblyContaining<RejectReportAssignmentRequest>();
                v.RegisterValidatorsFromAssemblyContaining<CloseReportRequest>();
                v.RegisterValidatorsFromAssemblyContaining<ArchiveReportRequest>();
                v.RegisterValidatorsFromAssemblyContaining<MoveReportAssignmentRequest>();
                v.RegisterValidatorsFromAssemblyContaining<GetMtaLogsRequest>();
                v.RegisterValidatorsFromAssemblyContaining<GetApiLogsRequest>();
                v.RegisterValidatorsFromAssemblyContaining<GetUserPurchasesRequest>();
                v.RegisterValidatorsFromAssemblyContaining<AddObjectProtectionRequest>();
                v.RegisterValidatorsFromAssemblyContaining<AddSerialSlotRequest>();
                v.RegisterValidatorsFromAssemblyContaining<TransferCharacterRequest>();
                v.RegisterValidatorsFromAssemblyContaining<AddCustomSkinRequest>();
                v.RegisterValidatorsFromAssemblyContaining<AddCustomInteriorRequest>();
                v.RegisterValidatorsFromAssemblyContaining<ChangeUploadedSkinRequest>();
                v.RegisterValidatorsFromAssemblyContaining<ChangeUploadedInteriorFileRequestValidator>();
                v.RegisterValidatorsFromAssemblyContaining<RestoreDefaultSkinRequest>();
                v.RegisterValidatorsFromAssemblyContaining<RestoreDefaultInteriorRequest>();
                v.RegisterValidatorsFromAssemblyContaining<SetOrderApprovalStateRequest>();
                v.RegisterValidatorsFromAssemblyContaining<CreatePaymentRequest>();
                v.RegisterValidatorsFromAssemblyContaining<CapturePaymentRequest>();
                v.RegisterValidatorsFromAssemblyContaining<DonateServerRequest>();

                v.ImplicitlyValidateRootCollectionElements = true;
            });
    }
}