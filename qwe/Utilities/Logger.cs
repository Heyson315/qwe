using System;
using System.Web;

namespace qwe.Utilities
{
    /// <summary>
    /// Logger utility for tracking application events and errors
    /// </summary>
    public class Logger
    {
        private static readonly string LogPath = HttpContext.Current?.Server.MapPath("~/App_Data/Logs") ?? "Logs";

        static Logger()
        {
            // Ensure log directory exists
            if (!System.IO.Directory.Exists(LogPath))
            {
                System.IO.Directory.CreateDirectory(LogPath);
            }
        }

        /// <summary>
        /// Log informational message
        /// </summary>
        public static void Info(string message, string source = "Application")
        {
            Log("INFO", message, source);
        }

        /// <summary>
        /// Log warning message
        /// </summary>
        public static void Warning(string message, string source = "Application")
        {
            Log("WARNING", message, source);
        }

        /// <summary>
        /// Log error message
        /// </summary>
        public static void Error(string message, Exception exception = null, string source = "Application")
        {
            var errorMessage = message;
            if (exception != null)
            {
                errorMessage += $"\nException: {exception.Message}\nStackTrace: {exception.StackTrace}";
            }
            Log("ERROR", errorMessage, source);
        }

        /// <summary>
        /// Core logging method
        /// </summary>
        private static void Log(string level, string message, string source)
        {
            try
            {
                var logFileName = $"log_{DateTime.Now:yyyy-MM-dd}.txt";
                var logFilePath = System.IO.Path.Combine(LogPath, logFileName);

                var logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] [{source}] {message}\n";

                System.IO.File.AppendAllText(logFilePath, logEntry);
            }
            catch (Exception ex)
            {
                // Fallback to debug output if file logging fails
                System.Diagnostics.Debug.WriteLine($"Logger failed: {ex.Message}");
            }
        }
    }
}
