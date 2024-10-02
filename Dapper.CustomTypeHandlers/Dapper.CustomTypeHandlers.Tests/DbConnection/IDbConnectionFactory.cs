using System.Data;

namespace Dapper.CustomTypeHandlers.Tests.DbConnection;

internal interface IDbConnectionFactory
{
    IDbConnection Connection();

    IDbConnection Connection(string name);
}