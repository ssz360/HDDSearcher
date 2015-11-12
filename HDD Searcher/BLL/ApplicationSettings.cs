namespace HDD_Searcher
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// This class contains application settings
    /// </summary>
    public class ApplicationSettings
    {
        #region fields
        /// <summary>
        /// The show icons True/False
        /// </summary>
        private static bool showIcons;

        /// <summary>
        /// The monitoring True/False
        /// </summary>
        private static bool monitoring;
        #endregion

        #region properties
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ApplicationSettings"/> is monitoring.
        /// </summary>
        /// <value>
        ///   <c>true</c> if monitoring; otherwise, <c>false</c>.
        /// </value>
        public static bool Monitoring
        {
            get
            {
                var result = GetSettings("Monitoring");
                monitoring = bool.Parse(result);

                return monitoring;
            }

            set
            {
                monitoring = value;
                SetSettings("Monitoring", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [show icons].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show icons]; otherwise, <c>false</c>.
        /// </value>
        public static bool ShowIcons
        {
            get
            {
                var result = GetSettings("ShowIcons");
                showIcons = bool.Parse(result);

                return showIcons;
            }

            set
            {
                showIcons = value;
                SetSettings("ShowIcons", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [show logs].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show logs]; otherwise, <c>false</c>.
        /// </value>
        public static bool ShowLogs
        {
            get
            {
                var result = GetSettings("ShowLogs");
                showIcons = bool.Parse(result);

                return showIcons;
            }

            set
            {
                showIcons = value;
                SetSettings("ShowLogs", value.ToString());
            }
        }
        #endregion

        #region private methods
        /// <summary>
        /// Gets the settings form App.config.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Setting value</returns>
        private static string GetSettings(string key)
        {
            return ConfigurationManager.AppSettings.Get(key);
        }

        /// <summary>
        /// Sets the settings to App.config.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        private static void SetSettings(string key, string value)
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings[key].Value = value;
            configuration.Save();

            ConfigurationManager.RefreshSection("appSettings");
        }
        #endregion
    }
}
