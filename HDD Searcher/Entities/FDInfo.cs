//#define Test
namespace HDD_Searcher
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Interop;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// folders and directories information
    /// </summary>
    public class FDInfo : DependencyObject, IDbEssentials
    {
        #region fields
        /// <summary>
        /// The identifier
        /// </summary>
        private long id;

        /// <summary>
        /// The database identifier
        /// </summary>
        private int databaseId;

        /// <summary>
        /// The is directory
        /// </summary>
        private bool isDirectory;

        /// <summary>
        /// The full path
        /// </summary>
        private string fullPath;

        /// <summary>
        /// The name
        /// </summary>
        private string name;

        /// <summary>
        /// The parent identifier
        /// </summary>
        private long parentID;

        /// <summary>
        /// The dal
        /// </summary>
        private FDInfoDAL dal;
        #endregion

        #region constructors
        
#if Test
        /// <summary>
        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        /// this constructor is for testing perposes only
        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        /// </summary>
        public FDInfo()
        {
        }
#else
        /// <summary>
        /// Initializes a new instance of the <see cref="FDInfo"/> class.
        /// </summary>
        public FDInfo()
        {
            this.dal = new FDInfoDAL();
        }
#endif
        #endregion

        #region properties
        /// <summary>
        /// Gets or sets the database identifier.
        /// </summary>
        /// <value>
        /// The database identifier.
        /// </value>
        [Key]
        public int DbId
        {
            get
            {
                return this.databaseId;
            }

            set
            {
                this.databaseId = value;
            }
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public long Id
        {
            get
            {
                return this.id;
            }

            set
            {
                this.id = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is directory.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is directory; otherwise, <c>false</c>.
        /// </value>
        public bool IsDirectory
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
        [NotMapped]
        public string FullPath
        {
            get
            {
                if (this.fullPath == null)
                {
                    this.fullPath = this.GetFullPath();
                }

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
        [Required]
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
        /// Gets or sets the parent identifier.
        /// </summary>
        /// <value>
        /// The parent identifier.
        /// </value>
        public long ParentID
        {
            get
            {
                return this.parentID;
            }

            set
            {
                this.parentID = value;
            }
        }

        /// <summary>
        /// Gets the icon.
        /// </summary>
        /// <value>
        /// The icon.
        /// </value>
        [NotMapped]
        public ImageSource Icon
        {
            get
            {
                ////if (this.icon == null)
                ////{
                ////    System.Drawing.Icon icon = System.Drawing.SystemIcons.WinLogo;
                ////    ImageSource imageSource;
                ////    try
                ////    {
                ////        icon = System.Drawing.Icon.ExtractAssociatedIcon(this.FullPath);
                ////    }
                ////    catch
                ////    {
                ////    }
                ////    finally
                ////    {
                ////        imageSource = Imaging.CreateBitmapSourceFromHIcon(
                ////            icon.Handle,
                ////            Int32Rect.Empty,
                ////            BitmapSizeOptions.FromEmptyOptions());
                ////        this.icon = imageSource;
                ////    }
                ////}
                ////return this.icon;

                return IconManager.FindIconForFilename(this.Name, false);
            }
        }
        #endregion

        #region public methods
        /// <summary>
        /// Gets the parent.
        /// </summary>
        /// <returns>file or directory info</returns>
        public FDInfo GetParent()
        {
            return this.dal.GetById(this.parentID);
        }

        /// <summary>
        /// Gets the full path.
        /// </summary>
        /// <returns>get path</returns>
        public string GetFullPath()
        {
            FDInfo parent = this;
            List<string> pathParts = new List<string>();
            pathParts.Add(this.name);
            do
            {
                parent = parent.GetParent();

                if (parent != null)
                {
                    pathParts.Add(parent.name);
                }
            } while (parent != null && parent.parentID != 0);

            pathParts.Reverse();
            string fullPath = string.Join("\\", pathParts).Replace("\\\\", "\\");

            return fullPath;
        }
        #endregion
    }
}