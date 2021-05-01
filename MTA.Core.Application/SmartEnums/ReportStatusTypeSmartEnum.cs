using System.Linq;
using Ardalis.SmartEnum;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.SmartEnums
{
    public abstract class ReportStatusTypeSmartEnum : SmartEnum<ReportStatusTypeSmartEnum>
    {
        protected ReportStatusTypeSmartEnum(string name, int value) : base(name, value)
        {
        }

        public static ReportStatusTypeSmartEnum Awaiting = new AwaitingType();
        public static ReportStatusTypeSmartEnum Assigned = new AssignedType();
        public static ReportStatusTypeSmartEnum Rejected = new RejectedType();
        public static ReportStatusTypeSmartEnum Opened = new OpenedType();
        public static ReportStatusTypeSmartEnum Closed = new ClosedType();
        public static ReportStatusTypeSmartEnum Archived = new ArchivedType();

        public abstract bool ShouldBeOpened(Report report, int userId);

        private sealed class AwaitingType : ReportStatusTypeSmartEnum
        {
            public AwaitingType() : base(nameof(Awaiting), (int) ReportStatusType.Awaiting)
            {
            }

            public override bool ShouldBeOpened(Report report, int userId)
                => !report.ReportComments.Any(rc => rc.UserId == report.AssigneeId);
        }

        private sealed class AssignedType : ReportStatusTypeSmartEnum
        {
            public AssignedType() : base(nameof(Assigned), (int) ReportStatusType.Assigned)
            {
            }

            public override bool ShouldBeOpened(Report report, int userId)
                => userId == report.AssigneeId || !report.ReportComments.Any(rc => rc.UserId == report.AssigneeId);
        }

        private sealed class RejectedType : ReportStatusTypeSmartEnum
        {
            public RejectedType() : base(nameof(Rejected), (int) ReportStatusType.Rejected)
            {
            }

            public override bool ShouldBeOpened(Report report, int userId) => false;
        }

        private sealed class OpenedType : ReportStatusTypeSmartEnum
        {
            public OpenedType() : base(nameof(Opened), (int) ReportStatusType.Opened)
            {
            }

            public override bool ShouldBeOpened(Report report, int userId) => false;
        }

        private sealed class ClosedType : ReportStatusTypeSmartEnum
        {
            public ClosedType() : base(nameof(Closed), (int) ReportStatusType.Closed)
            {
            }

            public override bool ShouldBeOpened(Report report, int userId) => false;
        }

        private sealed class ArchivedType : ReportStatusTypeSmartEnum
        {
            public ArchivedType() : base(nameof(Archived), (int) ReportStatusType.Archived)
            {
            }

            public override bool ShouldBeOpened(Report report, int userId)
                => report.CreatorId == userId && !report.HasArchived;
        }
    }
}