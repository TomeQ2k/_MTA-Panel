using System.Collections.Generic;
using AutoMapper;
using Moq;
using MTA.Core.Application.Logging;
using MTA.Core.Application.Logic.Requests.Queries;
using MTA.Core.Application.Logic.Requests.Queries.Params;
using MTA.Core.Application.Models;
using MTA.Core.Application.Services;
using MTA.Core.Application.Services.ReadOnly;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Entities;
using MTA.Infrastructure.Persistence.Logging;
using NUnit.Framework;

namespace MTA.UnitTests.Infrastructure.Persistence.Logging
{
    [TestFixture]
    public class LogReaderTests
    {
        private LogReader logReader;

        private Mock<IDatabase> database;
        private Mock<IReadOnlyFilesManager> filesManager;
        private Mock<IMapper> mapper;
        private Mock<IHttpContextWriter> httpContextWriter;
        private Mock<LogKeyWordsDictionary> logKeyWordsDictionary;

        private MtaLogFiltersParams mtaLogFilters;
        private IEnumerable<SourceAffectedModel> sourceAffectedModels;

        [SetUp]
        public void SetUp()
        {
            mtaLogFilters = new GetMtaLogsRequest()
            {
                Actions = new[] {LogAction.Anticheat, LogAction.Api}
            };
            sourceAffectedModels = new List<SourceAffectedModel>();

            database = new Mock<IDatabase>();
            filesManager = new Mock<IReadOnlyFilesManager>();
            mapper = new Mock<IMapper>();
            httpContextWriter = new Mock<IHttpContextWriter>();
            logKeyWordsDictionary = new Mock<LogKeyWordsDictionary>();

            database.Setup(d => d.MtaLogRepository.GetMtaLogs(mtaLogFilters, It.IsNotNull<IList<string>>()))
                .ReturnsAsync(new List<MtaLog>() {new MtaLog(), new MtaLog()});
            database.Setup(d => d.PhoneSmsRepository.GetPhoneSms(mtaLogFilters, It.IsNotNull<IList<string>>()))
                .ReturnsAsync(new List<PhoneSms>() {new PhoneSms(), new PhoneSms()});
            mapper.Setup(m => m.Map<IEnumerable<MtaLogModel>>(It.IsNotNull<IEnumerable<MtaLog>>()))
                .Returns(new List<MtaLogModel>());
            mapper.Setup(m => m.Map<IEnumerable<MtaLogModel>>(It.IsNotNull<IEnumerable<PhoneSms>>()))
                .Returns(new List<MtaLogModel>());

            logReader = new LogReader(database.Object, filesManager.Object, mapper.Object, httpContextWriter.Object,
                logKeyWordsDictionary.Object);
        }

        #region GetMtaLogsFromDatabase

        [Test]
        public void GetMtaLogsFromDatabase_TheOnlyActionIsPhoneSms_VerifyGetPhoneSmsCalled()
        {
            mtaLogFilters = new GetMtaLogsRequest()
            {
                Actions = new[] {LogAction.PhoneSms}
            };

            logReader.GetMtaLogsFromDatabase(mtaLogFilters, sourceAffectedModels);

            database.Verify(d => d.MtaLogRepository.GetMtaLogs(mtaLogFilters, It.IsNotNull<IList<string>>()),
                Times.Never);
            database.Verify(d => d.PhoneSmsRepository.GetPhoneSms(mtaLogFilters, It.IsNotNull<IList<string>>()),
                Times.Once);
        }

        [Test]
        public void GetMtaLogsFromDatabase_ActionsDoNotContainPhoneSms_VerifyGetMtaLogsCalled()
        {
            logReader.GetMtaLogsFromDatabase(mtaLogFilters, sourceAffectedModels);

            database.Verify(d => d.MtaLogRepository.GetMtaLogs(mtaLogFilters, It.IsNotNull<IList<string>>()),
                Times.Once);
            database.Verify(d => d.PhoneSmsRepository.GetPhoneSms(mtaLogFilters, It.IsNotNull<IList<string>>()),
                Times.Never);
        }

        [Test]
        public void GetMtaLogsFromDatabase_ActionsContainPhoneSmsAndOther_VerifyGetMtaLogsAndGetPhoneSmsCalled()
        {
            mtaLogFilters = new GetMtaLogsRequest()
            {
                Actions = new[] {LogAction.Anticheat, LogAction.Api, LogAction.PhoneSms}
            };

            logReader.GetMtaLogsFromDatabase(mtaLogFilters, sourceAffectedModels);

            database.Verify(d => d.MtaLogRepository.GetMtaLogs(mtaLogFilters, It.IsNotNull<IList<string>>()),
                Times.Once);
            database.Verify(d => d.PhoneSmsRepository.GetPhoneSms(mtaLogFilters, It.IsNotNull<IList<string>>()),
                Times.Once);
        }

        #endregion
    }
}