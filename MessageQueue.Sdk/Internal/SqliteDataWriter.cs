using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.Data.Sqlite;

namespace MessageQueue.Sdk
{
    /// <summary>
    /// Provides read-write access to a SQLite database.
    /// </summary>
    internal sealed class SqliteDataWriter : SqliteQueryRunner
    {
        public SqliteDataWriter(string databaseFilepath)
            : base(connectionString: new SqliteConnectionStringBuilder
            {
                ["Data Source"] = databaseFilepath,
                ["Mode"] = SqliteOpenMode.ReadWriteCreate
            }.ConnectionString)
        {
        }

        public async Task<int> WriteAsync(string sql, IReadOnlyDictionary<string, object>? parameters = null)
        {
            var command = CreateCommand(sql, parameters);
            return await command.ExecuteNonQueryAsync();
        }
    }
}
