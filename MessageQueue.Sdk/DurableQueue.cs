using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MessageQueue.Sdk
{
    public sealed class DurableQueue : IAsyncDisposable
    {
        public string Name { get; }

        private readonly SqliteQueryRunner queryRunner;

        public DurableQueue(string name)
        {
            this.Name = name;
            var databaseFilepath = $"{this.Name}.db";
            this.queryRunner = new SqliteQueryRunner(databaseFilepath);
        }

        public async ValueTask DisposeAsync() => await this.queryRunner.DisposeAsync();

        /// <summary>
        /// This must be called before pushing messages to the queue.
        /// </summary>
        public async Task InitializeAsync()
        {
            var sql = await SqlLoader.LoadSqlAsync("tbl_MessageQueue.sql");
            await this.queryRunner.WriteAsync(sql);
        }

        /// <summary>
        /// Reads the next message from the queue.
        /// </summary>
        public async Task PushAsync(string message)
        {
            var sql = await SqlLoader.LoadSqlAsync("proc_Write.sql");
            var rowsAffected = await this.queryRunner.WriteAsync(sql, new Dictionary<string, object> { ["message"] = message });
            Debug.Assert(rowsAffected == 1);
        }

        /// <summary>
        /// Reads the next message from the queue.
        /// </summary>
        public async Task<string?> PullAsync()
        {
            // See proc_Read.sql.
            const int messageColumnIndex = 1;

            var sql = await SqlLoader.LoadSqlAsync("proc_Read.sql");
            var resultRows = await this.queryRunner.ReadAsync(sql).ToListAsync();
            return resultRows.Count switch
            {
                0 => null,
                1 => SqlLoader.CastReference<string>(resultRows[0][messageColumnIndex]),
                _ => throw new SqlResultException($"Expected 0 or 1 rows, but got {resultRows.Count}.")
            };
        }

        /// <summary>
        /// Deletes all read messages from the queue.
        /// </summary>
        /// <returns>
        /// Number of deleted messages.
        /// </returns>
        public async Task<int> PruneAsync()
        {
            var sql = await SqlLoader.LoadSqlAsync("proc_Delete.sql");
            return await this.queryRunner.WriteAsync(sql);
        }
    }
}
