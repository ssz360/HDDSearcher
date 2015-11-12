namespace HDD_Searcher
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using EntityFramework.BulkInsert.Extensions;

    /// <summary>
    /// files and directories information of data access layer
    /// </summary>
    public class FDInfoDAL : Repository<FDInfo>
    {
        #region fields
        /// <summary>
        /// The context
        /// </summary>
        private static Context context;
        #endregion

        #region constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="FDInfoDAL"/> class.
        /// </summary>
        public FDInfoDAL()
        {
            context = this.BuildContext();
            this.DbContext = context;
            this.DbSet = this.DbContext.Set<FDInfo>();
        }
        #endregion

        #region events
        /// <summary>
        /// Occurs when [bulk insert start].
        /// </summary>
        public event EventHandler BulkInsertStart;

        /// <summary>
        /// Occurs when [bulk insert stop].
        /// </summary>
        public event EventHandler BulkInsertStop;
        #endregion

        #region public method
        /// <summary>
        /// Bulks the insert.
        /// </summary>
        /// <param name="collection">The collection.</param>
        public void BulkInsert(IEnumerable<FDInfo> collection)
        {
            this.BulkStartFire();
            context.BulkInsert<FDInfo>(collection);
            this.BulkStopFire();
        }

        /// <summary>
        /// Bulks the insert asynchronous.
        /// </summary>
        /// <param name="collection">The collection.</param>
        public async void BulkInsertAsync(IEnumerable<FDInfo> collection)
        {
            await Task.Factory.StartNew(() =>
            {
                this.BulkInsert(collection);
            });
        }

        /// <summary>
        /// Deletes all.
        /// </summary>
        public void DeleteAll()
        {
            context.Database.ExecuteSqlCommand("TRUNCATE TABLE [FDInfoes]");
        }
        #endregion

        #region private methods
        /// <summary>
        /// Builds the context.
        /// </summary>
        /// <returns>singleton context</returns>
        private Context BuildContext()
        {
            if (context == null)
            {
                context = new Context();
            }

            return context;
        }

        /// <summary>
        /// Bulks the start fire.
        /// </summary>
        private void BulkStartFire()
        {
            if (this.BulkInsertStart != null)
            {
                this.BulkInsertStart(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Bulks the stop fire.
        /// </summary>
        private void BulkStopFire()
        {
            if (this.BulkInsertStop != null)
            {
                this.BulkInsertStop(this, EventArgs.Empty);
            }
        }
        #endregion
    }
}
