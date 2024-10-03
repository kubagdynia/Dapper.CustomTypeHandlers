using System.Threading.Tasks;
using Dapper.CustomTypeHandlers.Tests.DbConnection;
using Dapper.CustomTypeHandlers.Tests.Models;

namespace Dapper.CustomTypeHandlers.Tests.Repositories;

internal class TestGuidRepository : ITestGuidRepository
{
    private const string TableName = "Test_Objects";
        
    private readonly IDbConnectionFactory _connectionFactory;

    public TestGuidRepository(IDbConnectionFactory connectionFactory)
        => _connectionFactory = connectionFactory;

    public async Task<TestGuidObject> GetTestGuidObject(long id)
    {
        using var conn = _connectionFactory.Connection();
        var result = await conn.QueryFirstAsync<TestGuidObject>(
            $"SELECT Id, GuidId FROM {TableName} WHERE Id = @id", new { id });

        return result;
    }

    public async Task SaveTestGuidObject(TestGuidObject testObject)
    {
        using var conn = _connectionFactory.Connection();
        testObject.Id = await conn.QueryFirstAsync<long>(
            $"INSERT INTO {TableName} (GuidId) VALUES (@GuidId); SELECT last_insert_rowid()", testObject);
    }
}