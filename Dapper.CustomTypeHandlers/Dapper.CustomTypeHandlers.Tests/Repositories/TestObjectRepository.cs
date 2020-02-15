using Dapper.CustomTypeHandlers.Tests.DbConnection;
using Dapper.CustomTypeHandlers.Tests.Models;
using System.Threading.Tasks;

namespace Dapper.CustomTypeHandlers.Tests.Repositories
{
    internal class TestObjectRepository : ITestObjectRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public TestObjectRepository(IDbConnectionFactory connectionFactory) => _connectionFactory = connectionFactory;

        public async Task<TestXmlObject> GetTestXmlObject(long id)
        {
            using (var conn = _connectionFactory.Connection())
            {
                TestXmlObject result = await conn.QueryFirstAsync<TestXmlObject>(
                    @"SELECT Id, FirstName, LastName, StartWork, Content FROM Test_Objects WHERE Id = @id", new { id });

                return result;
            }
        }

        public async Task SaveTestXmlObject(TestXmlObject testObject)
        {
            using (var conn = _connectionFactory.Connection())
            {
                testObject.Id = await conn.QueryFirstAsync<long>(
                    @"INSERT INTO Test_Objects (FirstName, LastName, StartWork, Content)
                         VALUES (@FirstName, @LastName, @StartWork, @Content);
                      select last_insert_rowid()", testObject);
            }
        }

        public async Task<TestJsonObject> GetTestJsonObject(long id)
        {
            using (var conn = _connectionFactory.Connection())
            {
                var result = await conn.QueryFirstAsync<TestJsonObject>(
                    @"SELECT Id, FirstName, LastName, StartWork, Content
                      FROM Test_Objects
                      WHERE Id = @id", new { id });

                return result;
            }
        }

        public async Task SaveTestJsonObject(TestJsonObject testObject)
        {
            using (var conn = _connectionFactory.Connection())
            {
                testObject.Id = await conn.QueryFirstAsync<long>(
                    @"INSERT INTO Test_Objects (FirstName, LastName, StartWork, Content)
                         VALUES (@FirstName, @LastName, @StartWork, @Content);
                      select last_insert_rowid()", testObject);
            }
        }
    }
}
