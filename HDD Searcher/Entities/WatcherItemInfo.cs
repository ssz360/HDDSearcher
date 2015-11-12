namespace HDD_Searcher
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// the class contains information of changes that fetch by watcher
    /// </summary>
    public class WatcherItemInfo
    {
        #region fields
        /// <summary>
        /// The is directory
        /// </summary>
        private bool? isDirectory;

        /// <summary>
        /// The full path
        /// </summary>
        private string fullPath;

        /// <summary>
        /// The name
        /// </summary>
        private string name;

        /// <summary>
        /// The time
        /// </summary>
        private DateTime time;

        /// <summary>
        /// The change type
        /// </summary>
        private WatcherChangeTypes changeType;

        /// <summary>
        /// The is rename
        /// </summary>
        private bool isRename;

        /// <summary>
        /// The old name
        /// </summary>
        private string oldName;

        /// <summary>
        /// The old full path
        /// </summary>
        private string oldFullPath;
        #endregion

        #region properties
        /// <summary>
        /// Gets or sets a value indicating whether this instance is rename.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is rename; otherwise, <c>false</c>.
        /// </value>
        public bool IsRename
        {
            get
            {
                return this.isRename;
            }

            set
            {
                this.isRename = value;
            }
        }

        /// <summary>
        /// Gets or sets the old name.
        /// </summary>
        /// <value>
        /// The old name.
        /// </value>
        public string OldName
        {
            get
            {
                return this.oldName;
            }

            set
            {
                this.oldName = value;
            }
        }

        /// <summary>
        /// Gets or sets the old full path.
        /// </summary>
        /// <value>
        /// The old full path.
        /// </value>
        public string OldFullPath
        {
            get
            {
                return this.oldFullPath;
            }

            set
            {
                this.oldFullPath = value;
            }
        }

        /// <summary>
        /// Gets or sets the is directory.
        /// </summary>
        /// <value>
        /// The is directory.
        /// </value>
        public bool? IsDirectory
        {
            get
            {
                return this.isDirectory;
            }

            set
            {
                this.isDirectory = value;
            }
        }

        /// <summary>
        /// Gets or sets the full path.
        /// </summary>
        /// <value>
        /// The full path.
        /// </value>
        public string FullPath
        {
            get
            {
                return this.fullPath;
            }

            set
            {
                this.fullPath = value;
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.name = value;
            }
        }

        /// <summary>
        /// Gets or sets the time.
        /// </summary>
        /// <value>
        /// The time.
        /// </value>
        public DateTime Time
        {
            get
            {
                return this.time;
            }

            set
            {
                this.time = value;
            }
        }

        /// <summary>
        /// Gets or sets the type of the change.
        /// </summary>
        /// <value>
        /// The type of the change.
        /// </value>
        public WatcherChangeTypes ChangeType
        {
            get
            {
                return this.changeType;
            }

            set
            {
                this.changeType = value;
            }
        }
        #endregion
    }
}