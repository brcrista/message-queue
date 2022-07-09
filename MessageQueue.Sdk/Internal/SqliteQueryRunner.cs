using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.Data.Sqlite;

namespace MessageQueue.Sdk
{
    /// <summary>
    /// Executes queries against a SQLite database.
    /// </summary>
    internal class SqliteQueryRunner : IAsyncDisposable
    {
        private readonly SqliteConnection dbConnection;

        public SqliteQueryRunner(string databaseFilepath)
        {
            var connectionString = new SqliteConnectionStringBuilder
            {
                ["Data Source"] = databaseFilepath,
                ["Mode"] = SqliteOpenMode.ReadWriteCreate
            }.ConnectionString;

            dbConnection = new SqliteConnection(connectionString);
            dbConnection.Open();
        }

        public async ValueTask DisposeAsync() => await dbConnection.DisposeAsync();

        public async IAsyncEnumerable<object[]> ReadAsync(string sql, IReadOnlyDictionary<string, object>? parameters = null)
        {
            var command = CreateCommand(sql, parameters);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var row = new object[reader.FieldCount];
                reader.GetValues(row);
                yield return row;
            }
        }

        public async Task<int> WriteAsync(string sql, IReadOnlyDictionary<string, object>? parameters = null)
        {
            var command = CreateCommand(sql, parameters);
            return await command.ExecuteNonQueryAsync();
        }

        private SqliteCommand CreateCommand(string sql, IReadOnlyDictionary<string, object>? parameters)
        {
            var command = dbConnection.CreateCommand();
            command.CommandText = sql;

            if (parameters is not null)
            {
                foreach (var kvp in parameters)
                {
                    command.Parameters.AddWithValue(kvp.Key, kvp.Value);
                }
            }

            return command;
        }
    }
}
