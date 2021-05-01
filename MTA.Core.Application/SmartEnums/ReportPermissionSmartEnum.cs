using System.Linq;
using Ardalis.SmartEnum;
using MTA.Core.Application.Helpers;
using MTA.Core.Application.Models;
using MTA.Core.Common.Enums;
using MTA.Core.Common.Enums.Permissions;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.SmartEnums
{
    public abstract class ReportPermissionSmartEnum : SmartEnum<ReportPermissionSmartEnum>
    {
        protected ReportPermissionSmartEnum(string name, int value) : base(name, value)
        {
        }

        public static ReportPermissionSmartEnum AddComment = new AddCommentType();
        public static ReportPermissionSmartEnum AttachImages = new AttachImagesType();
        public static ReportPermissionSmartEnum AcceptAssignment = new AcceptAssignmentType();
        public static ReportPermissionSmartEnum RejectAssignment = new RejectAssignmentType();
        public static ReportPermissionSmartEnum ManageSubscribers = new ManageSubscribersType();
        public static ReportPermissionSmartEnum CloseReport = new CloseReportType();
        public static ReportPermissionSmartEnum ArchiveReport = new ArchiveReportType();
        public static ReportPermissionSmartEnum MoveAssignment = new MoveAssignmentType();
        public static ReportPermissionSmartEnum TogglePrivacy = new TogglePrivacyType();

        public abstract PermissionModel<ReportPermission> IsPermitted(int userId, Report report);

        private sealed class AddCommentType : ReportPermissionSmartEnum
        {
            public AddCommentType() : base(nameof(AddComment), (int) ReportPermission.AddComment)
            {
            }

            public override PermissionModel<ReportPermission> IsPermitted(int userId, Report report)
            {
                var permissionModel = PermissionModel<ReportPermission>.Create(ReportPermission.AddComment);

                if (report.Private)
                    return permissionModel.AppendPermission(() => userId == report.CreatorId);

                return report.Status switch
                {
                    (int) ReportStatusType.Awaiting => permissionModel
                        .AppendPermission(() => userId == report.CreatorId && report.ReportComments.Count + 1 <=
                            ReportPermissionConstants.MaxCommentsWhenIsAwaiting)
                        .AppendPermission(() => userId == report.AssigneeId),
                    (int) ReportStatusType.Assigned => permissionModel
                        .AppendPermission(() => userId == report.CreatorId && report.ReportComments.Count + 1 <=
                            ReportPermissionConstants.MaxCommentsWhenIsAwaiting)
                        .AppendPermission(() => userId == report.AssigneeId &&
                                                !report.ReportComments.Any(rc => rc.UserId == userId)),
                    (int) ReportStatusType.Opened => permissionModel
                        .AppendPermission(() => userId == report.CreatorId && report.ReportComments
                            .TakeLast(ReportPermissionConstants.MaxCommentsInRow).Any(rc => rc.UserId != userId))
                        .AppendPermission(() => report.ReportSubscribers.Any(rs => rs.UserId == userId) && report
                            .ReportComments
                            .TakeLast(ReportPermissionConstants.MaxCommentsInRow).Any(rc => rc.UserId != userId))
                        .AppendPermission(() => userId == report.AssigneeId),
                    (int) ReportStatusType.Closed => permissionModel
                        .AppendPermission(() => userId == report.CreatorId),
                    _ => permissionModel
                };
            }
        }

        private sealed class AttachImagesType : ReportPermissionSmartEnum
        {
            public AttachImagesType() : base(nameof(AttachImages), (int) ReportPermission.AttachImages)
            {
            }

            public override PermissionModel<ReportPermission> IsPermitted(int userId, Report report)
            {
                var permissionModel = PermissionModel<ReportPermission>.Create(ReportPermission.AttachImages);

                if (report.Private)
                    return permissionModel.AppendPermission(() => userId == report.CreatorId);

                return report.Status switch
                {
                    (int) ReportStatusType.Awaiting => permissionModel
                        .AppendPermission(() => userId == report.CreatorId),
                    (int) ReportStatusType.Assigned => permissionModel
                        .AppendPermission(() => userId == report.CreatorId),
                    (int) ReportStatusType.Opened => permissionModel
                        .AppendPermission(() => userId == report.CreatorId)
                        .AppendPermission(() => userId == report.AssigneeId)
                        .AppendPermission(() => report.ReportSubscribers.Any(rs => rs.UserId == userId)),
                    _ => permissionModel
                };
            }
        }

        private sealed class AcceptAssignmentType : ReportPermissionSmartEnum
        {
            public AcceptAssignmentType() : base(nameof(AcceptAssignment), (int) ReportPermission.AcceptAssignment)
            {
            }

            public override PermissionModel<ReportPermission> IsPermitted(int userId, Report report)
            {
                var permissionModel = PermissionModel<ReportPermission>.Create(ReportPermission.AcceptAssignment);

                return report.Status switch
                {
                    (int) ReportStatusType.Awaiting or (int) ReportStatusType.Assigned => permissionModel
                        .AppendPermission(() => userId == report.AssigneeId),
                    _ => permissionModel
                };
            }
        }

        private sealed class RejectAssignmentType : ReportPermissionSmartEnum
        {
            public RejectAssignmentType() : base(nameof(RejectAssignment), (int) ReportPermission.RejectAssignment)
            {
            }

            public override PermissionModel<ReportPermission> IsPermitted(int userId, Report report)
            {
                var permissionModel = PermissionModel<ReportPermission>.Create(ReportPermission.RejectAssignment);

                return report.Status switch
                {
                    (int) ReportStatusType.Awaiting or (int) ReportStatusType.Assigned => permissionModel
                        .AppendPermission(() => userId == report.AssigneeId),
                    _ => permissionModel
                };
            }
        }

        private sealed class ManageSubscribersType : ReportPermissionSmartEnum
        {
            public ManageSubscribersType() : base(nameof(ManageSubscribers), (int) ReportPermission.ManageSubscribers)
            {
            }

            public override PermissionModel<ReportPermission> IsPermitted(int userId, Report report)
            {
                var permissionModel = PermissionModel<ReportPermission>.Create(ReportPermission.AttachImages);

                if (report.Private)
                    return permissionModel;

                return report.Status switch
                {
                    (int) ReportStatusType.Assigned => permissionModel
                        .AppendPermission(() => userId == report.AssigneeId),
                    (int) ReportStatusType.Opened => permissionModel
                        .AppendPermission(() => userId == report.AssigneeId),
                    _ => permissionModel
                };
            }
        }

        private sealed class CloseReportType : ReportPermissionSmartEnum
        {
            public CloseReportType() : base(nameof(CloseReport), (int) ReportPermission.CloseReport)
            {
            }

            public override PermissionModel<ReportPermission> IsPermitted(int userId, Report report)
            {
                var permissionModel = PermissionModel<ReportPermission>.Create(ReportPermission.CloseReport);

                if (report.Private)
                    return permissionModel;

                return report.Status switch
                {
                    (int) ReportStatusType.Opened => permissionModel
                        .AppendPermission(() => userId == report.AssigneeId),
                    _ => permissionModel
                };
            }
        }

        private sealed class ArchiveReportType : ReportPermissionSmartEnum
        {
            public ArchiveReportType() : base(nameof(ArchiveReport), (int) ReportPermission.ArchiveReport)
            {
            }

            public override PermissionModel<ReportPermission> IsPermitted(int userId, Report report)
                => PermissionModel<ReportPermission>.Create(ReportPermission.ArchiveReport)
                    .AppendPermission(() => userId == report.CreatorId);
        }

        private sealed class MoveAssignmentType : ReportPermissionSmartEnum
        {
            public MoveAssignmentType() : base(nameof(ArchiveReport), (int) ReportPermission.MoveAssignment)
            {
            }

            public override PermissionModel<ReportPermission> IsPermitted(int userId, Report report)
            {
                var permissionModel = PermissionModel<ReportPermission>.Create(ReportPermission.MoveAssignment);

                return report.Status switch
                {
                    (int) ReportStatusType.Closed or (int) ReportStatusType.Archived => permissionModel,
                    _ => permissionModel.AppendPermission(() => userId == report.AssigneeId)
                };
            }
        }

        private sealed class TogglePrivacyType : ReportPermissionSmartEnum
        {
            public TogglePrivacyType() : base(nameof(TogglePrivacy), (int) ReportPermission.TogglePrivacy)
            {
            }

            public override PermissionModel<ReportPermission> IsPermitted(int userId, Report report)
                => PermissionModel<ReportPermission>.Create(ReportPermission.TogglePrivacy);
        }
    }
}