namespace HDD_Searcher
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Diagnostics;
    using System.Linq;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using System.Windows;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Handles the Startup event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="StartupEventArgs"/> instance containing the event data.</param>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // force software to run as administrator

            bool isElevated;
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            isElevated = principal.IsInRole(WindowsBuiltInRole.Administrator);
            if (!isElevated)
            {
                ProcessStartInfo proc = new ProcessStartInfo();
                proc.UseShellExecute = true;

                String exePath = System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName;
                proc.WorkingDirectory = System.IO.Path.GetDirectoryName(exePath);
                proc.FileName = exePath;
                proc.Verb = "runas";

                try
                {
                    Process.Start(proc);
                }
                catch
                {
                    // The user refused the elevation.
                    // Do nothing and return directly ...
                    return;
                }

                Environment.Exit(0);
            }
            else
            {
                // show log window
                if (ApplicationSettings.ShowLogs)
                {
                    winLog winLog = new winLog();
                    winLog.LogsSource = Log.LogCollections;
                    winLog.Show();
                }

                new Log("Starting Software");

                var mainWin = new HDD_Searcher.MainWindow();
                mainWin.Show();
            }
        }
    }
}
