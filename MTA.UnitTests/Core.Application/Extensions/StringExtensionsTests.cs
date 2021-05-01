using MTA.Core.Application.Extensions;
using NUnit.Framework;

namespace MTA.UnitTests.Core.Application.Extensions
{
    [TestFixture]
    public class StringExtensionsTests
    {
        [Test]
        [TestCase("siema", false)]
        [TestCase("s i ema", true)]
        [TestCase(" s i ema", true)]
        [TestCase("s i ema ", true)]
        [TestCase(null, true)]
        [TestCase("", true)]
        [TestCase(" ", true)]
        public void HasWhitespaces_WhenCalled_ReturnHasWhitespaces(string value, bool expectedResult)
        {
            var result = value.HasWhitespaces();

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase("example@example.example", true)]
        [TestCase("example@example.", false)]
        [TestCase("example@example", false)]
        [TestCase("example@", false)]
        [TestCase("example", false)]
        [TestCase("@example", false)]
        [TestCase("@example.", false)]
        [TestCase("@example.example", false)]
        [TestCase("", false)]
        public void IsEmailAddress_WhenCalled_ReturnIsValid(string value, bool expectedResult)
        {
            var result = value.IsEmailAddress();

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void RemoveLastCharacter_WhenCalled_ReturnStringWithoutLastCharacter()
        {
            var result = "test".RemoveLastCharacter();

            Assert.That(result, Is.EqualTo("tes"));
        }
    }
}