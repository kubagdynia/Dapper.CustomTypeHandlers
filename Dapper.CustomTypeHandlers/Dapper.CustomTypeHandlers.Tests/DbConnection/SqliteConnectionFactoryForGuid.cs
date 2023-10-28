using System;
using System.Data;

namespace Dapper.CustomTypeHandlers.Tests.DbConnection
{
    internal class SqliteConnectionFactoryForGuid : BaseSqliteConnectionFactory
    {
        public SqliteConnectionFactoryForGuid() : base($"TestGuidDb_{Guid.NewGuid()}.sqlite")
        {
        }

        protected override void CreateDb(IDbConnection dbConnection)
            =>
                dbConnection.Execute(
                    @"CREATE TABLE Test_Objects
                          (
                            ID       integer primary key AUTOINCREMENT,
                            GuidId   varchar(36)
                          )");
    }
}