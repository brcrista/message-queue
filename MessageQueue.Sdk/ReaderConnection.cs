using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MessageQueue.Sdk
{
    public sealed class ReaderConnection
    {
        private readonly SqliteDataReader dataReader;

        public ReaderConnection(string name)
        {
            var databaseFilepath = $"{name}.db";
            if (!File.Exists(databaseFilepath))
            {
                throw new MessageQueueNotFoundException(name);
            }

            this.dataReader = new SqliteDataReader(databaseFilepath);
        }

        /// <summary>
        /// Reads the next message from the queue.
        /// </summary>
        public async Task<string?> GetMessageAsync()
        {
            // See tbl_MessageQueue.sql.
            const int messageColumnIndex = 2;

            var sql = await SqlLoader.LoadSqlAsync("proc_Read.sql");
            var resultRows = await this.dataReader.ExecuteAsync(sql).ToListAsync();
            return resultRows.Count switch
            {
                0 => null,
                1 => SqliteDataReader.CastReference<string>(resultRows[messageColumnIndex]),
                _ => throw new SqlResultException($"Expected 0 or 1 rows, but got {resultRows.Count}.")
            };
        }
    }
}
