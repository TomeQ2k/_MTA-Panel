using System;
using System.Linq;
using MTA.Core.Application.ServicesUtils;
using MTA.Core.Domain.Entities;
using NUnit.Framework;

namespace MTA.UnitTests.Core.Application.ServicesUtils
{
    [TestFixture]
    public class RolesServiceUtilsTests
    {
        [Test]
        public void GetRoleClaims_UserIsNull_ReturnEmptyClaims()
        {
            var result = RolesServiceUtils.GetRoleClaims(null);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void GetRoleClaims_UserHasDefaultRoles_ReturnEmptyClaims()
        {
            var result = RolesServiceUtils.GetRoleClaims(new User());

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void GetRoleClaims_UserIsAdmin_ReturnClaimsWithAdminRole()
        {
            var user = new User();
            user.SetRoles(2);

            var result = RolesServiceUtils.GetRoleClaims(user);

            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Value, Is.EqualTo("admin").IgnoreCase);
        }

        [Test]
        public void GetRoleClaims_UserIsSupporterAndScripter_ReturnClaimsWithSupporterAndScripterRole()
        {
            var user = new User();
            user.SetRoles(supporter: 1, scripter: 3);

            var result = RolesServiceUtils.GetRoleClaims(user);

            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().Value, Is.EqualTo("supporter").IgnoreCase);
            Assert.That(result.Last().Value, Is.EqualTo("scripter").IgnoreCase);
        }

        [Test]
        public void GetRoleClaims_UserIsTrialAdminAndMapperIsOutOfRange_ThrowArgumentOutOfRangeException()
        {
            var user = new User();
            user.SetRoles(admin: 1, mapper: 5);

            Assert.That(() => RolesServiceUtils.GetRoleClaims(user),
                Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void GetRoleClaims_VctRoleIsOutOfRange_ThrowArgumentOutOfRangeException()
        {
            var user = new User();
            user.SetRoles(vct: 5);

            Assert.That(() => RolesServiceUtils.GetRoleClaims(user),
                Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }
    }
}