using System.IO;
using System.Threading.Tasks;

namespace MessageQueue.Sdk
{
    /// <summary>
    /// Loads the contents of a SQL script as a string.
    /// </summary>
    internal static class SqlLoader
    {
        public static async Task<string> LoadSqlAsync(string scriptName)
        {
            var scriptPath = Path.Combine("sql", scriptName);
            return await File.ReadAllTextAsync(scriptPath);
        }
    }
}