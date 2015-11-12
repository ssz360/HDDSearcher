namespace HDD_Searcher
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// event handler for search events
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The e.</param>
    public delegate void SearchEventsHandler(object sender, SearchMethodsArgs e);

    /// <summary>
    /// the class contains search methods
    /// </summary>
    public class SearchMethods
    {
        #region events and enums
        /// <summary>
        /// Occurs when [search started].
        /// </summary>
        public event SearchEventsHandler SearchStarted;

        /// <summary>
        /// Occurs when [search ended].
        /// </summary>
        public event SearchEventsHandler SearchEnded;

        /// <summary>
        /// contains types of searches
        /// </summary>
        public enum SearchMethod
        {
            /// <summary>
            /// The MFT search method
            /// </summary>
            MFTSearch,

            /// <summary> 
            /// The recursive search method
            /// </summary>
            RecursiveSearch,

            /// <summary>
            /// The non recursive search method
            /// </summary>
            NonRecursiveSearch,
        }
        #endregion

        #region public methods
        /// <summary>
        /// Recursive search method.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>search result</returns>
        public List<FDInfo> RecursiveSearch(string path)
        {
            this.SearchStartedFire(
                this,
                new SearchMethodsArgs()
            {
                DriveLetter = path,
                SearchMethod = SearchMethod.RecursiveSearch,
            });

            var result = this.RecursiveSearch(path, true);

            this.SearchEndedFire(
                this,
                new SearchMethodsArgs()
            {
                DriveLetter = path,
                SearchMethod = SearchMethod.RecursiveSearch,
                SearchResult = result,
            });

            return result;
        }

        /// <summary>
        /// Non recursive search method.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>search result</returns>
        public List<FDInfo> NonRecursiveSearch(string path)
        {
            this.SearchStartedFire(
                this,
                new SearchMethodsArgs()
            {
                DriveLetter = path,
                SearchMethod = SearchMethod.NonRecursiveSearch,
            });

            Stack<FDInfo> stack = new Stack<FDInfo>();
            List<FDInfo> result = new List<FDInfo>();
            stack.Push(new FDInfo()
                {
                    FullPath = path,
                    Id = path.GetHashCode(),
                    ParentID = 0,
                    IsDirectory = true,
                    Name = path,
                });
            try
            {
                while (stack.Count > 0)
                {
                    FDInfo item = stack.Pop();

                    string fp = item.FullPath;

                    try
                    {
                        foreach (var directoryPath in Directory.GetDirectories(fp))
                        {
                            string dirocteryName = Path.GetFileName(directoryPath);
                            string parent = Path.GetDirectoryName(directoryPath);

                            stack.Push(new FDInfo()
                            {
                                FullPath = directoryPath,
                                Id = directoryPath.GetHashCode(),
                                IsDirectory = true,
                                Name = dirocteryName,
                                ParentID = parent.GetHashCode(),
                            });
                        }
                    }
                    catch
                    {
                    }

                    try
                    {
                        foreach (var filePath in Directory.GetFiles(fp))
                        {
                            string name = Path.GetFileName(filePath);
                            result.Add(new FDInfo()
                                                        {
                                                            FullPath = filePath,
                                                            Name = name,
                                                            Id = filePath.GetHashCode(),
                                                            IsDirectory = false,
                                                            ParentID = fp.GetHashCode(),
                                                        });
                        }
                    }
                    catch
                    {
                    }

                    result.Add(item);
                }

                this.SearchEndedFire(
                    this,
                    new SearchMethodsArgs()
                {
                    DriveLetter = path,
                    SearchMethod = SearchMethod.NonRecursiveSearch,
                    SearchResult = result,
                });
                return result;
            }
            catch
            {
                this.SearchEndedFire(
                    this,
                    new SearchMethodsArgs()
                {
                    DriveLetter = path,
                    SearchMethod = SearchMethod.NonRecursiveSearch,
                    SearchResult = result,
                });

                return result;
            }
        }

        /// <summary>
        /// MFTs search method.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>search result</returns>
        public List<FDInfo> MFTSearch(string path)
        {
            this.SearchStartedFire(
                this,
                new SearchMethodsArgs()
            {
                DriveLetter = path,
                SearchMethod = SearchMethod.MFTSearch,
            });

            List<FDInfo> result = new List<FDInfo>();
            Dictionary<ulong, MFTTest.FileNameAndParentFrn> filesNameCollection;
            MFTTest.PInvokeWin32 directorySearcher;

            MFTAdopter.Search(path, out filesNameCollection, out directorySearcher);

            foreach (var item in directorySearcher.Directories)
            {
                string name = item.Value.Name;
                if (name.Contains(@"\\.\"))
                {
                    name = name.Replace(@"\\.\", string.Empty);
                }

                result.Add(new FDInfo()
                {
                    Id = (long)item.Key,
                    IsDirectory = true,
                    Name = name,
                    ParentID = (long)item.Value.ParentFrn,
                });
            }

            foreach (var item in filesNameCollection)
            {
                result.Add(new FDInfo()
                {
                    Id = (long)item.Key,
                    IsDirectory = false,
                    Name = item.Value.Name,
                    ParentID = (long)item.Value.ParentFrn,
                });
            }

            this.SearchEndedFire(
                this,
                new SearchMethodsArgs()
            {
                DriveLetter = path,
                SearchMethod = SearchMethod.MFTSearch,
                SearchResult = result,
            });

            //// MakeFullPathAsync(filesNameCollection, directorySearcher, result);

            return result;
        }

        /// <summary>
        /// Recursive search asynchronous.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>search result</returns>
        public async Task<List<FDInfo>> RecursiveSearchAsync(string path)
        {
            List<FDInfo> result = new List<FDInfo>();
            await Task.Factory.StartNew(() =>
            {
                result = RecursiveSearch(path);
            });
            return result;
        }

        /// <summary>
        /// Non recursive search asynchronous.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>search result</returns>
        public async Task<List<FDInfo>> NonRecursiveSearchAsync(string path)
        {
            List<FDInfo> result = new List<FDInfo>();
            await Task.Factory.StartNew(() =>
            {
                result = NonRecursiveSearch(path);
            });

            return result;
        }

        /// <summary>
        /// MFTs search asynchronous.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>search result</returns>
        public async Task<List<FDInfo>> MFTSearchAsync(string path)
        {
            List<FDInfo> result = new List<FDInfo>();
            await Task.Factory.StartNew(() =>
             {
                 MFTSearch(path);
             });

            return result;
        }
        #endregion

        #region private methods
        /// <summary>
        /// Searches the started fire.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void SearchStartedFire(object sender, SearchMethodsArgs e)
        {
            if (this.SearchStarted != null)
            {
                this.SearchStarted(sender, e);
            }
        }

        /// <summary>
        /// Searches the ended fire.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void SearchEndedFire(object sender, SearchMethodsArgs e)
        {
            if (this.SearchEnded != null)
            {
                this.SearchEnded(sender, e);
            }
        }

        /// <summary>
        /// Recursive search method.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="addFirstNode">if set to <c>true</c> [add first node].</param>
        /// <returns>search result</returns>
        private List<FDInfo> RecursiveSearch(string path, bool addFirstNode)
        {
            List<FDInfo> result = new List<FDInfo>();
            if (addFirstNode)
            {
                result.Add(new FDInfo()
                {
                    FullPath = path,
                    Id = path.GetHashCode(),
                    ParentID = 0,
                    IsDirectory = true,
                    Name = path,
                });
            }

            try
            {
                foreach (string directoryPath in Directory.GetDirectories(path))
                {
                    try
                    {
                        foreach (string filePath in Directory.GetFiles(directoryPath))
                        {
                            string name = Path.GetFileName(filePath);
                            result.Add(new FDInfo()
                            {
                                FullPath = filePath,
                                Name = name,
                                Id = filePath.GetHashCode(),
                                IsDirectory = false,
                                ParentID = directoryPath.GetHashCode(),
                            });
                        }
                    }
                    catch
                    {
                    }

                    string dirocteryName = Path.GetFileName(directoryPath);
                    string parent = Path.GetDirectoryName(directoryPath);
                    result.Add(new FDInfo()
                    {
                        FullPath = directoryPath,
                        Id = directoryPath.GetHashCode(),
                        IsDirectory = true,
                        Name = dirocteryName,
                        ParentID = parent.GetHashCode(),
                    });

                    result.AddRange(this.RecursiveSearch(directoryPath, false));
                }

                return result;
            }
            catch
            {
                return result;
            }
        }
        #endregion

        #region not used methods
        ////private void MakeFullPathAsync(Dictionary<ulong, MFTTest.FileNameAndParentFrn> filesNameCollection,
        ////    MFTTest.PInvokeWin32 directorySearcher, List<FDInfo> result)
        ////{
        ////    List<KeyValuePair<UInt64, MFTTest.FileNameAndParentFrn>> collection = new List<KeyValuePair<UInt64, MFTTest.FileNameAndParentFrn>>();
        ////    collection.AddRange(filesNameCollection);
        ////    collection.AddRange(directorySearcher.Directories);

        ////    foreach (var item in result)
        ////    {
        ////        var itemt = result.Where(x => x.Id == item.ParentID);
        ////    }

        ////    foreach (KeyValuePair<UInt64, MFTTest.FileNameAndParentFrn> entry in collection)
        ////    {
        ////        var itemt = result.Where(x => x.Id == (long)entry.Key);

        ////        string path = MakeFullPath(entry, directorySearcher);
        ////        var item = result.Where(x => x.Id == (long)entry.Key).First();
        ////        if (item != null)
        ////        {

        ////            item.FullPath = path;
        ////        }
        ////    }
        ////}

        ////private string MakeFullPath(KeyValuePair<ulong, MFTTest.FileNameAndParentFrn> f, MFTTest.PInvokeWin32 mft)
        ////{
        ////    ulong parent = f.Value.ParentFrn;
        ////    List<string> parents = new List<string>();
        ////    parents.Add(f.Value.Name);
        ////    while (mft.Directories.ContainsKey(parent))
        ////    {
        ////        var pn = mft.Directories[parent];
        ////        string name = pn.Name;
        ////        parent = pn.ParentFrn;

        ////        parents.Add(name);
        ////    }
        ////    parents.Reverse();
        ////    string ty = string.Join("\\", parents).Replace(@"\\.\", "");
        ////    return ty;
        ////}
        #endregion
    }
}
