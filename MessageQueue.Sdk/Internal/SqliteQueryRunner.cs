using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.Data.Sqlite;

namespace MessageQueue.Sdk
{
    /// <summary>
    /// Executes queries against a SQLite database.
    /// </summary>
    internal abstract class SqliteQueryRunner : IAsyncDisposable
    {
        private readonly SqliteConnection dbConnection;

        public SqliteQueryRunner(string connectionString)
        {
            dbConnection = new SqliteConnection(connectionString);
            dbConnection.Open();
        }

        public async ValueTask DisposeAsync() => await dbConnection.DisposeAsync();

        protected SqliteCommand CreateCommand(string sql, IReadOnlyDictionary<string, object>? parameters)
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
