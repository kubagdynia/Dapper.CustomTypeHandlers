using System;
using System.Data;

namespace Dapper.CustomTypeHandlers.Tests.DbConnection
{
    public interface IDbConnectionFactory : IDisposable
    {
        IDbConnection Connection();

        IDbConnection Connection(string name);
    }
}
