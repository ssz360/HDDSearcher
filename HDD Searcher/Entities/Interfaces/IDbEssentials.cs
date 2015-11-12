namespace HDD_Searcher
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// interface for classes needs to save in database
    /// </summary>
    public interface IDbEssentials
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        long Id
        {
            get;
            set;
        }
    }
}
