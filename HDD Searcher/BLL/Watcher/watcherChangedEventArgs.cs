namespace HDD_Searcher
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Watcher class change event Args
    /// </summary>
    public class WatcherChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the new item.
        /// </summary>
        /// <value>
        /// The new item.
        /// </value>
        public WatcherItemInfo NewItem
        {
            get;
            set;
        }
    }
}
