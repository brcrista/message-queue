using System;
using System.Collections.Generic;

using Microsoft.Data.Sqlite;

namespace MessageQueue.Sdk
{
    /// <summary>
    /// Provides read-only access to a SQLite database.
    /// </summary>
    internal sealed class SqliteDataReader : SqliteQueryRunner
    {
        public SqliteDataReader(string databaseFilepath)
            : base(connectionString: new SqliteConnectionStringBuilder
            {
                ["Data Source"] = databaseFilepath,
                ["Mode"] = SqliteOpenMode.ReadOnly
            }.ConnectionString)
        {
        }

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

        /// <summary>
        /// Convert a column value from type <see cref="object"/> to a more specific value type, accounting for <see cref="DBNull"/>.
        /// </summary>
        /// <remarks>
        /// The normal C# cast operator will throw an <see cref="InvalidCastException"/> if you apply it to a value of type <see cref="DBNull"/>
        /// (the .NET type representing the SQL <c>NULL</c> value).
        /// This method will safely convert <see cref="DBNull"/> to <c>null</c>.
        /// </remarks>
        public static T? CastValue<T>(object value) where T : struct
        {
            if (DBNull.Value.Equals(value))
            {
                return null;
            }
            else
            {
                return (T)value;
            }
        }

        /// <summary>
        /// Convert a column value from type <see cref="object"/> to a more specific reference type, accounting for <see cref="DBNull"/>.
        /// </summary>
        /// <remarks>
        /// The normal C# cast operator will throw an <see cref="InvalidCastException"/> if you apply it to a value of type <see cref="DBNull"/>
        /// (the .NET type representing the SQL <c>NULL</c> value).
        /// This method will safely convert <see cref="DBNull"/> to <c>null</c>.
        /// </remarks>
        public static T? CastReference<T>(object value) where T : class
        {
            if (DBNull.Value.Equals(value))
            {
                return null;
            }
            else
            {
                return (T)value;
            }
        }
    }
}
