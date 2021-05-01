using Moq;
using MTA.Core.Application.Models;
using MTA.Core.Common.Enums.Permissions;
using NUnit.Framework;

namespace MTA.UnitTests.Core.Application.Models
{
    [TestFixture]
    public class ReportPermissionModelTests
    {
        private PermissionModel<ReportPermission> permissionModel;

        [SetUp]
        public void SetUp()
        {
            permissionModel = PermissionModel<ReportPermission>.Create(It.IsAny<ReportPermission>());
        }

        [Test]
        [TestCase(true, true, true)]
        [TestCase(true, false, true)]
        [TestCase(false, false, false)]
        [TestCase(false, true, true)]
        public void AppendPermission_IsPermitted_ReturnThis(bool condition1, bool condition2, bool expected)
        {
            var result = permissionModel.AppendPermission(() => condition1)
                .AppendPermission(() => condition2);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsPermitted, Is.EqualTo(expected));
        }
    }
}