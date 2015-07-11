using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NoIP.DDNS
{
    public class Client
    {
        protected readonly string AssemblyVersion;
        private static readonly Assembly ddnsAssembly = Assembly.GetExecutingAssembly();

        public string UserAgent
        {
            get { return _userAgent; }
            set
            {
                _userAgent = string.Format("DDNS/{0} ({1}) {2}/{3}",
                                            AssemblyVersion,
                                            Environment.OSVersion)
            }
        }

        private string _userAgent;

        public Client(string userAgentName)
        {
            // Set assembly version
            var version = ddnsAssembly.GetName().Version;
            AssemblyVersion = String.Format("{0}.{1}", version.Major, version.Minor);
        }
    }
}
