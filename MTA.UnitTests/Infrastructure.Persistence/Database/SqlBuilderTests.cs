using MTA.Core.Application.Builders;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Data.Helpers;
using MTA.UnitTests.TestModels;
using NUnit.Framework;

namespace MTA.UnitTests.Infrastructure.Persistence.Database
{
    [TestFixture]
    public class SqlBuilderTests
    {
        private SqlBuilder builder;

        private ColumnValueDictionary columnValueDictionary;
        private string date;

        [SetUp]
        public void SetUp()
        {
            builder = new SqlBuilder();

            date = "2020-01-01";

            columnValueDictionary = ColumnValueDictionary.Create(
                new ColumnValue("id", 2), new ColumnValue("name", "test"),
                new ColumnValue("date", date), new ColumnValue("money", 1.5));
        }

        [Test]
        public void Select_IgnoreColumns_ReturnSelectQueryWithoutIgnoredColumns()
        {
            var result = builder.Select<TestEntity>("IGNORED").Build().Query;

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Does.Not.Contain("IGNORED"));
            Assert.That(result, Does.Contain("SELECT `id`,`charactername`,`money`,`account`"));
        }

        [Test]
        public void Where_WhenCalled_ReturnQuery()
        {
            var result = builder.Where("test").Build().Query;

            Assert.That(result, Is.EqualTo("WHERE test "));
        }

        [Test]
        [TestCase(OrderByType.Ascending, "ORDER BY test ASC")]
        [TestCase(OrderByType.Descending, "ORDER BY test DESC")]
        public void OrderBy_WhenCalled_ReturnQuery(OrderByType type, string expectedReturn)
        {
            var result = builder.OrderBy("test", type).Build().Query;

            Assert.That(result, Does.Contain(expectedReturn));
        }

        [Test]
        public void WhereIn_WhenCalled_ReturnQuery()
        {
            var result = builder.WhereIn("test", new string[] {"1", "2", "3"}).Build().Query;

            Assert.That(result, Does.Contain("WHERE test IN ('1','2','3')"));
        }

        [Test]
        public void Delete_WhenCalled_ReturnQuery()
        {
            var idColumnValue = new ColumnValue("test", "test");

            var result = builder.Delete("test", idColumnValue).Build().Query;

            Assert.That(result, Does.Contain("DELETE FROM test WHERE `test` = 'test'"));
        }

        [Test]
        public void DeleteRange_WhenCalled_ReturnQuery()
        {
            var idColumnValues = new[]
            {
                new ColumnValue("test1", 1),
                new ColumnValue("test2", 2)
            };

            var result = builder.DeleteRange("test", idColumnValues).Build().Query;

            Assert.That(result, Does.Contain("DELETE FROM test WHERE `test1` IN ('1','2')"));
        }

        [Test]
        public void DeleteRange_EmptyColumnValue_ReturnQuery()
        {
            var idColumnValues = new ColumnValue[] { };

            var result = builder.DeleteRange("test", idColumnValues).Build().Query;

            Assert.That(result, Is.EqualTo(""));
        }

        [Test]
        public void InsertInto_WhenCalled_ReturnQuery()
        {
            var result = builder
                .InsertInto("table", columnValueDictionary)
                .Build();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Query,
                Does.Contain($"INSERT INTO table (id,name,date,money) VALUES ('2','test','{date}','1,5')"));
        }

        [Test]
        public void Update_WhenCalled_ReturnQuery()
        {
            var result = builder
                .Update("table", columnValueDictionary, new ColumnValue("pk", 1))
                .Build();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Query,
                Does.Contain($"UPDATE table SET id='2',name='test',date='{date}',money='1,5' WHERE pk = '1'"));
        }

        [Test]
        public void Build_WhenCalled_ReturnQueryStringWithDeletedWhitespaces()
        {
            var result = new SqlBuilder()
                .Select()
                .From("table")
                .Where("id")
                .Equals
                .Append(1)
                .Build();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Query, Is.EqualTo("SELECT * FROM table WHERE id = 1 "));
        }
    }
}