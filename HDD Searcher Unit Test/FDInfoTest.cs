using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HDD_Searcher_Unit_Test
{
    [TestClass]
    public class FDInfoTest
    {

        // *********************************************************************************//
        //              Uncomment "Test" in FDinfo Class for testing perposes
        // *********************************************************************************//

        [TestMethod]
        public void IconTest()
        {
            HDD_Searcher.FDInfo fd = new HDD_Searcher.FDInfo();
            fd.Name = "test.jpg";
            var icon = fd.Icon;

            Assert.IsNotNull(icon);
        }

        // *********************************************************************************//
        // We should use MOQ for testing other mehods and properties (GetParent, GetFullPath)
        // *********************************************************************************//

    }
}
