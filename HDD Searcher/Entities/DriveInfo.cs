namespace HDD_Searcher
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Class od drives info
    /// </summary>
    public class DriveInfo
    {
        #region fields
        /// <summary>
        /// The drive letter
        /// </summary>
        private string driveLetter;

        /// <summary>
        /// should search
        /// </summary>
        private bool isChecked;

        /// <summary>
        /// The searched items
        /// </summary>
        private List<FDInfo> searchedItems;
        #endregion

        #region public methods
        /// <summary>
        /// Gets or sets the searched items.
        /// </summary>
        /// <value>
        /// The searched items.
        /// </value>
        public List<FDInfo> SearchedItems
        {
            get
            {
                return this.searchedItems;
            }

            set
            {
                this.searchedItems = value;
            }
        }

        /// <summary>
        /// Gets or sets the drive letter.
        /// </summary>
        /// <value>
        /// The drive letter.
        /// </value>
        public string DriveLetter
        {
            get
            {
                return this.driveLetter;
            }

            set
            {
               this.driveLetter = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is checked.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is checked; otherwise, <c>false</c>.
        /// </value>
        public bool IsChecked
        {
            get
            {
                return this.isChecked;
            }

            set
            {
                this.isChecked = value;
            }
        }

        /// <summary>
        /// Gets the drives.
        /// </summary>
        /// <returns>derives list</returns>
        public static List<DriveInfo> GetDrives()
        {
            List<DriveInfo> result = new List<DriveInfo>();

            var drives = System.IO.DriveInfo.GetDrives();

            foreach (var item in drives)
            {
                if (item.DriveType == System.IO.DriveType.Fixed)
                {
                    result.Add(new DriveInfo()
                    {
                        DriveLetter = item.Name,
                        IsChecked = true,
                    });
                }
            }

            return result;
        }
        #endregion
    }
}
