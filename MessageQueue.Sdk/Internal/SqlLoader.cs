using System;
using System.IO;
using System.Reflection;
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
            // The SQL scripts are packaged along with the assembly (see the .csproj).
            var executableDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (executableDirectory is null)
            {
                // This will happen only if Assembly.GetExecutingAssembly().Location is `/` or `null`.
                throw new InvalidOperationException();
            }

            var scriptPath = Path.Combine(executableDirectory, "sql", scriptName);
            return await File.ReadAllTextAsync(scriptPath);
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