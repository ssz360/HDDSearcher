using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HDD_Searcher;

namespace HDD_Searcher_Unit_Test
{
    [TestClass]
    public class SearchMethodsTest
    {
        // *********************************************************************************//
        //              Uncomment "Test" in FDinfo Class for testing perposes
        // *********************************************************************************//

        [TestMethod]
        public void MFTTest()
        {
            SearchMethods sm = new SearchMethods();
            var c = sm.MFTSearch("d:\\");
            Assert.AreNotSame(c.Count, 0);
        }

        [TestMethod]
        public void RecursiveSearchTest()
        {
            SearchMethods sm = new SearchMethods();
            var c = sm.RecursiveSearch("d:\\");
            Assert.AreNotSame(c.Count, 0);
        }

        [TestMethod]
        public void NonRecursiveSearchTest()
        {
            SearchMethods sm = new SearchMethods();
            var c = sm.NonRecursiveSearch("d:\\");
            Assert.AreNotSame(c.Count, 0);
        }
    }
}
