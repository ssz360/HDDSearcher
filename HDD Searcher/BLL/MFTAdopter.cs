namespace HDD_Searcher
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MFTTest;

    /// <summary>
    /// Adopter for MFT library
    /// </summary>
    public class MFTAdopter
    {
        /// <summary>
        /// Searches the specified drive.
        /// </summary>
        /// <param name="drive">The drive.</param>
        /// <param name="filesNameCollection">The files name collection.</param>
        /// <param name="directorySearcher">The directory searcher.</param>
        /// <param name="extensions">The extensions.</param>
        public static void Search(
            string drive,
            out Dictionary<ulong, MFTTest.FileNameAndParentFrn> filesNameCollection,
            out PInvokeWin32 directorySearcher,
            string[] extensions = null)
        {
            drive = drive.Replace(":\\", ":");
            if (extensions == null)
            {
                extensions = new string[] { "*" };
            }

            filesNameCollection = new Dictionary<ulong, MFTTest.FileNameAndParentFrn>();

            directorySearcher = new PInvokeWin32();
            directorySearcher.Drive = drive;

            directorySearcher.EnumerateVolume(out filesNameCollection, new string[] { "*" });
        }
    }
}
