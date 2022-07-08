using System.IO;
using System.Threading.Tasks;

namespace MessageQueue.Sdk
{
    internal static class SqlLoader
    {
        /// <summary>
        /// Loads the contents of a SQL script as a string.
        /// </summary>
        public static async Task<string> LoadSqlAsync(string scriptName)
        {
            var scriptPath = Path.Combine("sql", scriptName);
            return await File.ReadAllTextAsync(scriptPath);
        }
    }
}