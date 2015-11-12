using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HDD_Searcher_Unit_Test
{
    [TestClass]
    public class DriveInfoTest
    {
        [TestMethod]
        public void GetDrivesTest()
        {
            var drives = HDD_Searcher.DriveInfo.GetDrives();

            Assert.AreNotEqual(drives.Count, 0);
        }
    }
}
