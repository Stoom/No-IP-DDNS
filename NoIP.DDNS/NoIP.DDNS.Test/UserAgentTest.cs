using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NoIP.DDNS.Test
{
    [TestClass]
    public class UserAgentTest
    {
        [TestMethod]
        public void SetProgramUserAgentAndReturnFullUserAgent()
        {
            const string programName = "UATest";
            var ua = new UserAgent(programName);

            var currentVersion = GetType().Assembly.GetName().Version;
            var expectedResults = String.Format("DDNS/1.0 (Windows NT 6.3; WOW64) {0}/{1}.{2}",
                                                programName,
                                                currentVersion.Major,
                                                currentVersion.Minor);
            Assert.AreEqual(expectedResults, ua.ToString());
        }

        [TestMethod]
        public void SetProgramUserAgentViaProgramNameAndReturnFullUserAgent()
        {
            const string programName = "UATest";
            var ua = new UserAgent("llama");
            ua.ProgramName = programName;

            var currentVersion = GetType().Assembly.GetName().Version;
            var expectedResults = String.Format("DDNS/1.0 (Windows NT 6.3; WOW64) {0}/{1}.{2}",
                                                programName,
                                                currentVersion.Major,
                                                currentVersion.Minor);
            Assert.AreEqual(expectedResults, ua.ToString());
        }
    }
}
