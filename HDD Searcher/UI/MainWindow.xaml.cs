namespace HDD_Searcher
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region fields
        /// <summary>
        /// The data access layer of files and directories
        /// </summary>
        private FDInfoDAL dal;

        /// <summary>
        /// The files and directories collection
        /// </summary>
        private ObservableCollection<FDInfo> fdCollection;

        /// <summary>
        /// The watcher collection
        /// </summary>
        private List<FileWatcher> watcherCollection = new List<FileWatcher>();
        #endregion

        #region constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            AppDomain.CurrentDomain.SetData("DataDirectory", AppDomain.CurrentDomain.BaseDirectory);
        }
        #endregion

        #region callback events methods
        /// <summary>
        /// Handles the ChangedEvent event of the watcher control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="WatcherChangedEventArgs"/> instance containing the event data.</param>
        private void watcher_ChangedEvent(object sender, WatcherChangedEventArgs e)
        {
            // nothing should process if the type of changes is "changed"
            if (e.NewItem.ChangeType == WatcherChangeTypes.Changed || this.fdCollection == null)
            {
                return;
            }

            string fileName = Path.GetFileName(e.NewItem.FullPath);
            string parentPath = Path.GetDirectoryName(e.NewItem.FullPath);
            string parentName = Path.GetFileName(parentPath);

            // if the "parentName" is empty the parent is a drive and becuase we saved drives name we can
            // use "parentName = parentPath" and find it by it's name
            if (string.IsNullOrWhiteSpace(parentName))
            {
                parentName = parentPath;
            }

            var parent = this.fdCollection.Where(x =>
            {
                // parent can't be a file
                if (!x.IsDirectory)
                {
                    return false;
                }

                if (!string.IsNullOrEmpty(parentName) &&
                    x.Name.ToLowerInvariant() != parentName.ToLowerInvariant())
                {
                    return false;
                }

                return x.GetFullPath().ToLowerInvariant() == parentPath.ToLowerInvariant();
            });

            // if parent couldn't be found
            if (parent.Count() == 0)
            {
                new Log(string.Format("change ocured but not saved becuase it's parent not found ({0}-{1}", e.NewItem.ChangeType.ToString(), e.NewItem.FullPath));

                return;
            }

            FDInfo changeItem = new FDInfo()
            {
                FullPath = e.NewItem.FullPath,
                Id = e.NewItem.FullPath.GetHashCode(),
                Name = fileName,
                IsDirectory = e.NewItem.IsDirectory ?? false,
                ParentID = parent.First().Id,
            };

            switch (e.NewItem.ChangeType)
            {
                case WatcherChangeTypes.All:
                    break;
                case WatcherChangeTypes.Changed:
                    break;
                case WatcherChangeTypes.Created:
                    Dispatcher.Invoke(() =>
                    {
                        fdCollection.Add(changeItem);
                        dal.Insert(changeItem);
                    });
                    break;
                case WatcherChangeTypes.Deleted:
                    var itemd = this.fdCollection.Where(x =>
                    {
                        if (x.ParentID != changeItem.ParentID)
                        {
                            return false;
                        }

                        if (x.Name.ToLowerInvariant() == changeItem.Name.ToLowerInvariant())
                        {
                            return true;
                        }

                        return false;
                    });
                    if (itemd.Count() > 0)
                    {
                        var item = itemd.First();
                        Dispatcher.Invoke(() =>
                        {
                            fdCollection.Remove(item);
                            dal.Delete(item);
                        });
                    }

                    break;
                case WatcherChangeTypes.Renamed:

                    var itemR = this.fdCollection.Where(x =>
                    {
                        if (x.ParentID != changeItem.ParentID)
                        {
                            return false;
                        }

                        if (x.Name.ToLowerInvariant() == e.NewItem.OldName.ToLowerInvariant())
                        {
                            return true;
                        }

                        return false;
                    });
                    if (itemR.Count() > 0)
                    {
                        var item = itemR.First();

                        Dispatcher.Invoke(() =>
                        {
                            fdCollection.Remove(item);
                            dal.Delete(item);
                            fdCollection.Add(changeItem);
                            dal.Insert(changeItem);
                        });
                    }

                    break;
                default:
                    break;
            }

            // save changes to the database
            this.dal.Commit();

            new Log(string.Format("change ocured ({0}-{1}", e.NewItem.ChangeType.ToString(), e.NewItem.FullPath));
        }

        /// <summary>
        /// Handles the KeyUp event of the txtSearch control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            string filter = txtSearch.Text;
            Log startLog = new Log(string.Format("start search \"{0}\"", filter), true);

            ListCollectionView view = CollectionViewSource.GetDefaultView(lstView.ItemsSource) as ListCollectionView;
            if (string.IsNullOrEmpty(filter))
            {
                view.Filter = null;
            }
            else
            {
                view.Filter = obj =>
                {
                    return ((FDInfo)obj).Name.ToLower().Contains(filter);
                };
            }

            new Log(string.Format("end search \"{0}\"", filter)).EndOfLog(startLog.Id);
        }

        /// <summary>
        /// Handles the Click event of the btnSettings control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            winSettings settings = new winSettings();
            bool? result = settings.ShowDialog();

            if (result != null && result == true)
            {
                this.cbIcon.IsChecked = settings.ShowIcons;

                if (settings.IsMonitor)
                {
                    this.WatcherInit();
                }
                else
                {
                    this.WatcherStops();
                }

                if (settings.SearchResults != null)
                {
                    IEnumerable<FDInfo> items = settings.SearchResults;
                    this.fdCollection = new ObservableCollection<FDInfo>(items);
                    lstView.ItemsSource = this.fdCollection;

                    // delete the database entries
                    this.dal.DeleteAll();

                    // save to database
                    this.dal.BulkInsertAsync(settings.SearchResults);
                }
            }
        }

        /// <summary>
        /// Handles the Loaded event of the Window control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.cbIcon.IsChecked = ApplicationSettings.ShowIcons;

            if (ApplicationSettings.Monitoring)
            {
                this.WatcherInit();
            }

            this.AddToListBox();
        }
        #endregion

        #region helper methods
        /// <summary>
        /// initialize the Watchers.
        /// </summary>
        private void WatcherInit()
        {
            new Log("init monitoring");

            foreach (var item in this.watcherCollection)
            {
                item.Stop();
            }

            this.watcherCollection.Clear();

            var drives = DriveInfo.GetDrives();
            foreach (var item in drives)
            {
                FileWatcher watcher = new FileWatcher(item.DriveLetter);
                watcher.ChangedEvent += watcher_ChangedEvent;
                watcher.Srart();
                watcherCollection.Add(watcher);
            }
        }

        /// <summary>
        /// Watchers the stops.
        /// </summary>
        private void WatcherStops()
        {
            new Log("stop monitoring");

            foreach (var item in this.watcherCollection)
            {
                item.Stop();
            }

            this.watcherCollection.Clear();
        }

        /// <summary>
        /// Adds to ListBox.
        /// </summary>
        private async void AddToListBox()
        {
            try
            {
                Log startLog = new Log("start loading database", true);
                await Task.Factory.StartNew(() =>
                {
                    dal = new FDInfoDAL();
                    dal.BulkInsertStart += Dal_BulkInsertStart;
                    dal.BulkInsertStop += Dal_BulkInsertStop;
                    List<FDInfo> items = dal.GetAll().ToList();
                    fdCollection = new ObservableCollection<FDInfo>(items);
                    Dispatcher.Invoke(() =>
                    {
                        lstView.ItemsSource = fdCollection;
                    });
                });
                new Log("database loaded").EndOfLog(startLog.Id);
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("database error");
            }
        }

        /// <summary>
        /// Handles the BulkInsertStop event of the dal control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void Dal_BulkInsertStop(object sender, EventArgs e)
        {
            new Log("new data of searching saved").EndOfLog("startbulkinsert");
        }

        /// <summary>
        /// Handles the BulkInsertStart event of the dal control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void Dal_BulkInsertStart(object sender, EventArgs e)
        {
            new Log("start saving new data of searching", true, "startbulkinsert");
        }
        #endregion
    }
}
