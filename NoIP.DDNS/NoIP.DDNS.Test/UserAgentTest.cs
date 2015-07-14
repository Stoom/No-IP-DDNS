using System;
using System.Reflection;
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
            var libVersion = Assembly.GetAssembly(ua.GetType()).GetName().Version;
            var expectedResults = String.Format("{0}/{1} (Windows NT 6.3; WOW64) DDNS/{2}",
                                                programName,
                                                currentVersion.ToString(2),
                                                libVersion.ToString(2));
            Assert.AreEqual(expectedResults, ua.ToString());
        }

        [TestMethod]
        public void SetProgramUserAgentViaProgramNameAndReturnFullUserAgent()
        {
            const string programName = "UATest";
            var ua = new UserAgent("llama");
            ua.ProgramName = programName;

            var currentVersion = GetType().Assembly.GetName().Version;
            var libVersion = Assembly.GetAssembly(ua.GetType()).GetName().Version;
            var expectedResults = String.Format("{0}/{1} (Windows NT 6.3; WOW64) DDNS/{2}",
                                                programName,
                                                currentVersion.ToString(2),
                                                libVersion.ToString(2));
            Assert.AreEqual(expectedResults, ua.ToString());
        }
    }
}
