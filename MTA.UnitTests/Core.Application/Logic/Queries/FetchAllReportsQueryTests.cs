using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Features.Handlers.Queries;
using MTA.Core.Application.Features.Requests.Queries;
using MTA.Core.Application.Features.Requests.Queries.Params;
using MTA.Core.Application.Features.Responses.Queries;
using MTA.Core.Application.Models;
using MTA.Core.Application.Results;
using MTA.Core.Application.Services;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Entities;
using NUnit.Framework;

namespace MTA.UnitTests.Core.Application.Logic.Queries
{
    [TestFixture]
    public class FetchAllReportsQueryTests
    {
        private FindCategoriesToReadResult findCategoriesToReadResult;
        private List<ReportListDto> reportsDto;
        private Mock<IReportService> reportService;
        private Mock<IReportValidationService> reportValidationService;
        private Mock<IMapper> mapper;
        private Mock<IHttpContextWriter> httpContextWriter;
        private FetchAllReportsQuery fetchAllReportsQuery;

        [SetUp]
        public void SetUp()
        {
            findCategoriesToReadResult =
                new FindCategoriesToReadResult(new[] {ReportCategoryType.Ban, ReportCategoryType.Bug});

            reportService = new Mock<IReportService>();
            reportValidationService = new Mock<IReportValidationService>();
            mapper = new Mock<IMapper>();
            httpContextWriter = new Mock<IHttpContextWriter>();

            var reports = new List<Report> {new Report(), new Report()};
            reportsDto = new List<ReportListDto> {new ReportListDto(), new ReportListDto()};

            reportValidationService.Setup(rv => rv.FindCategoriesToRead()).ReturnsAsync(findCategoriesToReadResult);
            reportService.Setup(rs =>
                    rs.FetchAllReports(It.IsAny<ReportFiltersParams>(), It.IsAny<ReportCategoryType[]>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(new PagedList<Report>(reports, 5, 5, 5));

            mapper.Setup(m => m.Map<IEnumerable<ReportListDto>>(It.IsAny<IEnumerable<Report>>())).Returns(reportsDto);
            fetchAllReportsQuery = new FetchAllReportsQuery(reportService.Object, reportValidationService.Object,
                mapper.Object, httpContextWriter.Object);
        }

        [Test]
        public void Handle_NoPermissionForUser_ThrowNoPermissionsException()
        {
            var request = new FetchAllReportsRequest
            {
                CategoryType = ReportCategoryType.Account
            };

            Assert.That(() => fetchAllReportsQuery.Handle(request, It.IsAny<CancellationToken>()),
                Throws.TypeOf<NoPermissionsException>());
        }

        [Test]
        [TestCase(ReportCategoryType.All)]
        [TestCase(ReportCategoryType.Ban)]
        public async Task Handle_WhenCalled_ReturnFetchAllReportsPaginationResponse(ReportCategoryType type)
        {
            var request = new FetchAllReportsRequest
            {
                CategoryType = type
            };

            var result = await fetchAllReportsQuery.Handle(request, It.IsAny<CancellationToken>());

            Assert.That(result, Is.TypeOf<FetchAllReportsResponse>());
            Assert.That(result.Reports, Is.EqualTo(reportsDto));
            Assert.That(result, Is.Not.Null);
        }
    }
}