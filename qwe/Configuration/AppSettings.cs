using System;
using System.Configuration;

namespace qwe.Configuration
{
    /// <summary>
    /// Centralized configuration management
    /// </summary>
    public static class AppSettings
    {
        /// <summary>
        /// Maximum file upload size in bytes (default: 10MB)
        /// </summary>
        public static long MaxFileUploadSize
        {
            get
            {
                var setting = ConfigurationManager.AppSettings["MaxFileUploadSize"];
                return long.TryParse(setting, out var size) ? size : 10485760; // 10MB default
            }
        }

        /// <summary>
        /// Allowed file extensions for upload
        /// </summary>
        public static string[] AllowedFileExtensions
        {
            get
            {
                var setting = ConfigurationManager.AppSettings["AllowedFileExtensions"];
                return setting?.Split(',') ?? new[] { ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".jpg", ".jpeg", ".png" };
            }
        }

        /// <summary>
        /// Upload directory path
        /// </summary>
        public static string UploadDirectory
        {
            get
            {
                return ConfigurationManager.AppSettings["UploadDirectory"] ?? "~/App_Data/Uploads";
            }
        }

        /// <summary>
        /// Enable detailed error messages (for development only)
        /// </summary>
        public static bool ShowDetailedErrors
        {
            get
            {
                var setting = ConfigurationManager.AppSettings["ShowDetailedErrors"];
                return bool.TryParse(setting, out var show) && show;
            }
        }

        /// <summary>
        /// Company contact email
        /// </summary>
        public static string ContactEmail
        {
            get
            {
                return ConfigurationManager.AppSettings["ContactEmail"] ?? "contact@hhrcpa.us";
            }
        }

        /// <summary>
        /// Application environment (Development, Staging, Production)
        /// </summary>
        public static string Environment
        {
            get
            {
                return ConfigurationManager.AppSettings["Environment"] ?? "Development";
            }
        }

        /// <summary>
        /// Check if running in development mode
        /// </summary>
        public static bool IsDevelopment => Environment.Equals("Development", StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// Check if running in production mode
        /// </summary>
        public static bool IsProduction => Environment.Equals("Production", StringComparison.OrdinalIgnoreCase);
    }
}
