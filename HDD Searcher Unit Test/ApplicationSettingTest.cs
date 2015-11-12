using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HDD_Searcher;

namespace HDD_Searcher_Unit_Test
{
    [TestClass]
    public class ApplicationSettingTest
    {
        [TestMethod]
        public void propertiesTests()
        {
            var icon = ApplicationSettings.ShowIcons;
            var monitoring = ApplicationSettings.Monitoring;
            var log = ApplicationSettings.ShowLogs;

            ApplicationSettings.Monitoring = !monitoring;
            ApplicationSettings.ShowIcons = !icon;
            ApplicationSettings.ShowLogs = !log;

            Assert.AreEqual(ApplicationSettings.Monitoring, !monitoring);
            Assert.AreEqual(ApplicationSettings.ShowIcons, !icon);
            Assert.AreEqual(ApplicationSettings.ShowLogs, !log);
        }
    }
}
