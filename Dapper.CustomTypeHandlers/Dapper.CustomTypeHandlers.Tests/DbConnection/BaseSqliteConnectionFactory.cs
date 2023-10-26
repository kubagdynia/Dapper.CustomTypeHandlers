using Microsoft.Data.Sqlite;
using System;
using System.Data;
using System.IO;

namespace Dapper.CustomTypeHandlers.Tests.DbConnection
{
    internal abstract class BaseSqliteConnectionFactory : IDbConnectionFactory
    {
        private readonly string _fileName;

        protected BaseSqliteConnectionFactory(string dbFilename)
        {
            _fileName = Path.Combine(Environment.CurrentDirectory, dbFilename);
            InitializeDatabase();
        }

        public IDbConnection Connection()
        {
            var connectionString = $"DataSource={_fileName}";
            var conn = new SqliteConnection(connectionString);

            return conn;
        }

        public IDbConnection Connection(string name)
        {
            return Connection();
        }

        protected abstract void CreateDb(IDbConnection dbConnection);

        private void InitializeDatabase()
        {
            if (File.Exists(_fileName))
            {
                return;
            }

            FileStream fileStream = File.Create(_fileName);
            fileStream.Close();

            using var conn = Connection();
            conn.Open();
            try
            {
                CreateDb(conn);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
