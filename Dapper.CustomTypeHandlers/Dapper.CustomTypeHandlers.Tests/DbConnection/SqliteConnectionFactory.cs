using System;
using System.Data;

namespace Dapper.CustomTypeHandlers.Tests.DbConnection;

internal class SqliteConnectionFactory : BaseSqliteConnectionFactory
{
    public SqliteConnectionFactory() : base($"TestDb_{Guid.NewGuid()}.sqlite")
    {

    }

    protected override void CreateDb(IDbConnection dbConnection)
    {
        dbConnection.Execute(
            @"CREATE TABLE Test_Objects
                (
                    ID                                  integer primary key AUTOINCREMENT,
                    FirstName                           varchar(100) not null,
                    LastName                            varchar(100) not null,
                    StartWork                           datetime not null,
                    Content                             TEXT
                )");
    }
}