namespace HDD_Searcher
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// custom context of entity framework
    /// </summary>
    public class Context : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Context"/> class.
        /// </summary>
        public Context()
            : base("Context")
        {
        }

        /// <summary>
        /// Gets or sets the files directories information.
        /// </summary>
        /// <value>
        /// The files directories information.
        /// </value>
        public DbSet<FDInfo> FilesDirectoriesInfo
        {
            get;
            set;
        }
    }
}
