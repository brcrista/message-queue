using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MessageQueue.Sdk
{
    public sealed class WriterConnection : IAsyncDisposable
    {
        private readonly SqliteDataWriter dataWriter;

        public WriterConnection(string name)
        {
            var databaseFilepath = $"{name}.db";
            this.dataWriter = new SqliteDataWriter(databaseFilepath);
        }

        public async ValueTask DisposeAsync() => await this.dataWriter.DisposeAsync();

        /// <summary>
        /// Reads the next message from the queue.
        /// </summary>
        public async Task PushMessageAsync(string message)
        {
            var sql = await SqlLoader.LoadSqlAsync("proc_Write.sql");
            var rowsAffected = await this.dataWriter.WriteAsync(sql, new Dictionary<string, object> { ["message"] = message });
            Debug.Assert(rowsAffected == 1);
        }
    }
}
