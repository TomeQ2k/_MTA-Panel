using Ardalis.SmartEnum;
using MTA.Core.Application.Builders;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Data;

namespace MTA.Core.Application.SmartEnums
{
    public abstract class ReportCategoryTypeSmartEnum : SmartEnum<ReportCategoryTypeSmartEnum>
    {
        protected ReportCategoryTypeSmartEnum(string name, int value) : base(name, value)
        {
        }

        public static ReportCategoryTypeSmartEnum Question = new QuestionType();
        public static ReportCategoryTypeSmartEnum Ban = new BanType();
        public static ReportCategoryTypeSmartEnum Penalty = new PenaltyType();
        public static ReportCategoryTypeSmartEnum User = new UserType();
        public static ReportCategoryTypeSmartEnum Donation = new DonationType();
        public static ReportCategoryTypeSmartEnum Account = new AccountType();
        public static ReportCategoryTypeSmartEnum Bug = new BugType();

        private const string AdminRole = "k.admin";
        private const string SupporterRole = "k.supporter";

        private SqlQuery WhenReportIsPrivateQuery()
            => new SqlBuilder()
                .Where(AdminRole).GreaterEquals.Append(3)
                .Build();

        public abstract SqlQuery WhereReportCategoryRolesAre(bool isPrivate = false);

        private sealed class QuestionType : ReportCategoryTypeSmartEnum
        {
            public QuestionType() : base(nameof(Question), (int) ReportCategoryType.Question)
            {
            }

            public override SqlQuery WhereReportCategoryRolesAre(bool isPrivate = false)
                => !isPrivate
                    ? new SqlBuilder()
                        .Where(AdminRole).Greater.Append(0)
                        .Or.Append(SupporterRole).Greater.Append(0)
                        .Build()
                    : WhenReportIsPrivateQuery();
        }

        private sealed class BanType : ReportCategoryTypeSmartEnum
        {
            public BanType() : base(nameof(Ban), (int) ReportCategoryType.Ban)
            {
            }

            public override SqlQuery WhereReportCategoryRolesAre(bool isPrivate = false)
                => !isPrivate
                    ? new SqlBuilder()
                        .Where(AdminRole).Greater.Append(0)
                        .Build()
                    : WhenReportIsPrivateQuery();
        }

        private sealed class PenaltyType : ReportCategoryTypeSmartEnum
        {
            public PenaltyType() : base(nameof(Penalty), (int) ReportCategoryType.Penalty)
            {
            }

            public override SqlQuery WhereReportCategoryRolesAre(bool isPrivate = false)
                => !isPrivate
                    ? new SqlBuilder()
                        .Where(AdminRole).Greater.Append(0)
                        .Or.Append(SupporterRole).Greater.Append(0)
                        .Build()
                    : WhenReportIsPrivateQuery();
        }

        private sealed class UserType : ReportCategoryTypeSmartEnum
        {
            public UserType() : base(nameof(User), (int) ReportCategoryType.User)
            {
            }

            public override SqlQuery WhereReportCategoryRolesAre(bool isPrivate = false)
                => !isPrivate
                    ? new SqlBuilder()
                        .Where(AdminRole).Greater.Append(0)
                        .Or.Append(SupporterRole).Greater.Append(0)
                        .Build()
                    : WhenReportIsPrivateQuery();
        }

        private sealed class DonationType : ReportCategoryTypeSmartEnum
        {
            public DonationType() : base(nameof(Donation), (int) ReportCategoryType.Donation)
            {
            }

            public override SqlQuery WhereReportCategoryRolesAre(bool isPrivate = false)
                => new SqlBuilder()
                    .Where(AdminRole).Equals.Append(4)
                    .Build();
        }

        private sealed class AccountType : ReportCategoryTypeSmartEnum
        {
            public AccountType() : base(nameof(Account), (int) ReportCategoryType.Account)
            {
            }

            public override SqlQuery WhereReportCategoryRolesAre(bool isPrivate = false)
                => !isPrivate
                    ? new SqlBuilder()
                        .Where(AdminRole).Greater.Append(0)
                        .Build()
                    : WhenReportIsPrivateQuery();
        }

        private sealed class BugType : ReportCategoryTypeSmartEnum
        {
            public BugType() : base(nameof(Bug), (int) ReportCategoryType.Bug)
            {
            }

            public override SqlQuery WhereReportCategoryRolesAre(bool isPrivate = false)
                => !isPrivate
                    ? new SqlBuilder()
                        .Where(AdminRole).Greater.Append(0)
                        .Or.Append(SupporterRole).Greater.Append(0)
                        .Build()
                    : WhenReportIsPrivateQuery();
        }
    }
}