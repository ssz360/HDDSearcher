namespace HDD_Searcher
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// search methods events args
    /// </summary>
    public class SearchMethodsArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the drive letter.
        /// </summary>
        /// <value>
        /// The drive letter.
        /// </value>
        public string DriveLetter
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the search method.
        /// </summary>
        /// <value>
        /// The search method.
        /// </value>
        public SearchMethods.SearchMethod SearchMethod
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the search result.
        /// </summary>
        /// <value>
        /// The search result.
        /// </value>
        public IEnumerable<FDInfo> SearchResult
        {
            get;
            set;
        }
    }
}
