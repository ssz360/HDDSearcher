namespace HDD_Searcher
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;

    /// <summary>
    /// Interaction logic for winSettings.xaml
    /// </summary>
    public partial class winSettings : Window
    {
        /// <summary>
        /// 0 means controls should be enabled and the of the search raised
        /// </summary>
        private int shouldEnable = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="winSettings"/> class.
        /// </summary>
        public winSettings()
        {
            this.InitializeComponent();

            new Log("initializing settings window");

            this.DataContext = DriveInfo.GetDrives();
            this.cbIcon.IsChecked = ApplicationSettings.ShowIcons;
            this.cbMonitor.IsChecked = ApplicationSettings.Monitoring;
            this.cbLogWindow.IsChecked = ApplicationSettings.ShowLogs;
        }

        /// <summary>
        /// Gets or sets the search results.
        /// </summary>
        /// <value>
        /// The search results.
        /// </value>
        public List<FDInfo> SearchResults
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the selected drives.
        /// </summary>
        /// <value>
        /// The selected drives.
        /// </value>
        public List<DriveInfo> SelectedDrives
        {
            get;
            set;
        }

        public bool ShowIcons
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is monitor.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is monitor; otherwise, <c>false</c>.
        /// </value>
        public bool IsMonitor
        {
            get;
            set;
        }

        public bool ShowLogWindow
        {
            get;
            set;
        }

        /// <summary>
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        /// <summary>
        /// Handles the Click event of the btnSearch control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            var items = lsbDrive.Items;
            List<DriveInfo> driveList = new List<DriveInfo>();
            SearchMethods.SearchMethod method = SearchMethods.SearchMethod.MFTSearch;

            this.SearchResults = null;

            foreach (var item in items)
            {
                if (((DriveInfo)item).IsChecked)
                {
                    driveList.Add((DriveInfo)item);
                }
            }

            if (rbMTF.IsChecked == true)
            {
                method = SearchMethods.SearchMethod.MFTSearch;
            }
            else if (rbNonRecursive.IsChecked == true)
            {
                method = SearchMethods.SearchMethod.NonRecursiveSearch;
            }
            else
            {
                method = SearchMethods.SearchMethod.RecursiveSearch;
            }

            this.SelectedDrives = driveList;

            this.Search(driveList, method);
        }

        /// <summary>
        /// Searches the specified drives list.
        /// </summary>
        /// <param name="DrivesList">The drives list.</param>
        /// <param name="method">The method.</param>
        private void Search(List<DriveInfo> DrivesList, SearchMethods.SearchMethod method)
        {
            SearchMethods searchMethod = new SearchMethods();
            searchMethod.SearchStarted += searchMethod_SearchStarted;
            searchMethod.SearchEnded += searchMethod_SearchEnded;

            new Log("starting searchs", true, "startingsearchs");
            switch (method)
            {
                case SearchMethods.SearchMethod.MFTSearch:
                    Parallel.ForEach(
                        DrivesList,
                        d =>
                        {
                            searchMethod.MFTSearchAsync(d.DriveLetter);
                        });
                    break;
                case SearchMethods.SearchMethod.NonRecursiveSearch:
                    Parallel.ForEach(
                        DrivesList,
                        d =>
                        {
                            searchMethod.NonRecursiveSearchAsync(d.DriveLetter);
                        });
                    break;
                case SearchMethods.SearchMethod.RecursiveSearch:
                    Parallel.ForEach(
                        DrivesList,
                        d =>
                        {
                            searchMethod.RecursiveSearchAsync(d.DriveLetter);
                        });
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// call when searches ended.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void searchMethod_SearchEnded(object sender, SearchMethodsArgs e)
        {
            this.shouldEnable--;

            new Log(string.Format("end of searching drive {0}", e.DriveLetter)).EndOfLog(e.DriveLetter);

            if (SearchResults == null)
            {
                this.SearchResults = e.SearchResult.ToList();
            }
            else
            {
                this.SearchResults.AddRange(e.SearchResult);
            }

            if (this.shouldEnable == 0)
            {
                new Log("end of all searchs").EndOfLog("startingsearchs");
                Dispatcher.Invoke(() =>
                {
                    this.btnCancle.IsEnabled = true;
                    this.btnSearch.IsEnabled = true;
                    this.btnSave.IsEnabled = true;
                });
            }
        }

        /// <summary>
        /// call when searches started.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void searchMethod_SearchStarted(object sender, SearchMethodsArgs e)
        {
            new Log(string.Format("start searching drive {0}", e.DriveLetter), true, e.DriveLetter);
            this.shouldEnable++;
            Dispatcher.Invoke(() =>
            {
                this.btnCancle.IsEnabled = false;
                this.btnSearch.IsEnabled = false;
                this.btnSave.IsEnabled = false;
            });
        }

        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;

            if (cbIcon != null)
            {
                this.ShowIcons = (bool)cbIcon.IsChecked;
            }
            else
            {
                this.ShowIcons = false;
            }

            if (cbMonitor != null)
            {
                this.IsMonitor = (bool)cbMonitor.IsChecked;
            }
            else
            {
                this.IsMonitor = false;
            }


            if (cbLogWindow != null)
            {
                this.ShowLogWindow = (bool)cbLogWindow.IsChecked;
            }
            else
            {
                this.ShowLogWindow = false;
            }

            ApplicationSettings.ShowIcons = this.ShowIcons;
            ApplicationSettings.Monitoring = this.IsMonitor;
            ApplicationSettings.ShowLogs = this.ShowLogWindow;

            this.Close();
        }

        /// <summary>
        /// Handles the Click event of the btnShowlog control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnShowlog_Click(object sender, RoutedEventArgs e)
        {
            winLog log = new winLog();
            log.LogsSource = Log.LogCollections;
            log.Show();
        }
    }
}
