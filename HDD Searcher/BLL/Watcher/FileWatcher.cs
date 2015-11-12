namespace HDD_Searcher
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// changed files event handler
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="WatcherChangedEventArgs"/> instance containing the event data.</param>
    public delegate void ChangedEventHandler(object sender, WatcherChangedEventArgs e);

    /// <summary>
    /// This class monitor file system for files and directories changes
    /// </summary>
    public class FileWatcher
    {
        #region fields
        /// <summary>
        /// The watcher
        /// </summary>
        private FileSystemWatcher watcher;

        /// <summary>
        /// The is started shows the watcher started or no.
        /// </summary>
        private bool isStarted;

        /// <summary>
        /// Occurs when [changed event].
        /// </summary>
        public event ChangedEventHandler ChangedEvent;
        #endregion

        #region properties
        /// <summary>
        /// Gets or sets the filter.
        /// </summary>
        /// <value>
        /// The filter of results.
        /// </value>
        public string Filter
        {
            get
            {
                return this.watcher.Filter;
            }

            set
            {
                this.watcher.Filter = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [include subdirectories].
        /// </summary>
        /// <value>
        /// <c>true</c> if [include subdirectories]; otherwise, <c>false</c>.
        /// </value>
        public bool IncludeSubdirectories
        {
            get
            {
                return this.watcher.IncludeSubdirectories;
            }

            set
            {
                this.watcher.IncludeSubdirectories = value;
            }
        }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>
        /// The path that should monitor.
        /// </value>
        public string Path
        {
            get
            {
                return this.watcher.Path;
            }

            set
            {
                this.watcher.Path = value;
            }
        }
        #endregion

        #region constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="FileWatcher"/> class.
        /// </summary>
        /// <param name="path">The path should be monitored.</param>
        /// <param name="filter">The filter of results.</param>
        public FileWatcher(string path, string filter = "*.*")
        {
           this.watcher = new FileSystemWatcher();
            this.isStarted = false;

            this.watcher.NotifyFilter = NotifyFilters.Attributes |
                                      NotifyFilters.CreationTime |
                                      NotifyFilters.DirectoryName |
                                      NotifyFilters.FileName |
                                      NotifyFilters.LastAccess |
                                      NotifyFilters.LastWrite |
                                      NotifyFilters.Security |
                                      NotifyFilters.Size;

            this.watcher.Changed += new FileSystemEventHandler(this.OnChanged);
            this.watcher.Created += new FileSystemEventHandler(this.OnChanged);
            this.watcher.Deleted += new FileSystemEventHandler(this.OnChanged);
            this.watcher.Renamed += new RenamedEventHandler(this.OnRenamed);

            this.watcher.Filter = filter;
            this.watcher.Path = path;
            this.watcher.IncludeSubdirectories = true;
        }
        #endregion

        #region public methods
        /// <summary>
        /// Starts monitoring.
        /// </summary>
        public void Srart()
        {
            if (!this.isStarted)
            {
                this.watcher.EnableRaisingEvents = true;
                this.isStarted = true;
            }
        }

        /// <summary>
        /// Stops this monitoring.
        /// </summary>
        public void Stop()
        {
            if (this.isStarted)
            {
                this.watcher.EnableRaisingEvents = false;
                this.isStarted = false;
            }
        }
        #endregion

        #region private methods
        /// <summary>
        /// Changes the event fire.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="WatcherChangedEventArgs"/> instance containing the event data.</param>
        private void ChangedEventFire(object sender, WatcherChangedEventArgs e)
        {
            if (this.ChangedEvent != null)
            {
                this.ChangedEvent(sender, e);
            }
        }

        /// <summary>
        /// Called when [renamed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RenamedEventArgs"/> instance containing the event data.</param>
        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            string fullPath = e.FullPath;
            string name = e.Name;
            WatcherChangeTypes type = e.ChangeType;
            string oldPath = e.OldFullPath;
            string oldName = e.OldName;

            this.ChangeHandler(fullPath, name, type, true, oldPath, oldName);
        }

        /// <summary>
        /// Called when [changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="FileSystemEventArgs"/> instance containing the event data.</param>
        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            string fullPath = e.FullPath;
            string name = e.Name;
            WatcherChangeTypes type = e.ChangeType;

            this.ChangeHandler(fullPath, name, type);
        }

        /// <summary>
        /// Changes the handler.
        /// </summary>
        /// <param name="fullPath">The full path.</param>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="isRename">if set to <c>true</c> [is rename].</param>
        /// <param name="oldFullPath">The old full path.</param>
        /// <param name="oldName">The old name.</param>
        private void ChangeHandler(
            string fullPath,
            string name,
            WatcherChangeTypes type,
            bool isRename = false,
            string oldFullPath = "",
            string oldName = "")
        {
            bool? isDirectory = null;

            // the approach doesn't work if file deleted

            // http://stackoverflow.com/questions/1395205/better-way-to-check-if-path-is-a-file-or-a-directory
            // FileAttributes attr = File.GetAttributes(FullPath);
            // if (attr.HasFlag(FileAttributes.Directory))
            // IsDirectory = true;
            // else
            // IsDirectory = false;
            if (File.Exists(fullPath))
            {
                isDirectory = false;
            }
            else if (Directory.Exists(fullPath))
            {
                isDirectory = true;
            }
            else
            {
                isDirectory = null;
            }

            WatcherItemInfo info = new WatcherItemInfo()
            {
                ChangeType = type,
                FullPath = fullPath,
                OldFullPath = fullPath,
                Name = name,
                OldName = name,
                Time = DateTime.Now,
                IsDirectory = isDirectory,
            };

            if (isRename)
            {
                info.IsRename = true;
                info.OldFullPath = oldFullPath;
                info.OldName = oldName;
            }

            WatcherChangedEventArgs arg = new WatcherChangedEventArgs()
            {
                NewItem = info,
            };

            this.ChangedEventFire(this, arg);
        }
        #endregion
    }
}
