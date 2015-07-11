using System;
using System.Linq;
using System.Reflection;

namespace NoIP.DDNS
{
    public sealed class UserAgent
    {
        private static readonly string LibraryName;
        private static readonly string LibraryVersion;
        private static readonly string OsVersion;
        static UserAgent()
        {
            var asm = Assembly.GetExecutingAssembly().GetName();
            LibraryName = asm.Name.Split('.').Last();
            LibraryVersion = asm.Version.ToString(2);
            OsVersion = GenerateOsVersionString();
        }

        public string ProgramName
        {
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                    throw new ArgumentException();
                _userAgent = GenerateUserAgent(value, Assembly.GetCallingAssembly().GetName().Version);
            }
        }
        
        private string _userAgent;

        public UserAgent(string programName)
        {
            if (String.IsNullOrWhiteSpace(programName))
                throw new ArgumentException();
            ProgramName = programName;
        }

        private string GenerateUserAgent(string programName, Version programVersion)
        {
            return String.Format("{0}/{1} ({2}) {3}/{4}",
                                 LibraryName, LibraryVersion,
                                 OsVersion,
                                 programName, programVersion.ToString(2));
        }

        private static string GenerateOsVersionString()
        {
            var os = String.Empty;
            var wow = String.Empty;

            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32NT:
                    os = "Windows NT";
                    break;
                case PlatformID.Win32Windows:
                    if ((Environment.OSVersion.Version.Major > 4) ||
                        ((Environment.OSVersion.Version.Major == 4) && (Environment.OSVersion.Version.Minor > 0)))
                        os = "Windows 98";
                    else
                        os = "Windows 95";
                    break;
                case PlatformID.Win32S:
                    os = "Win32S";
                    break;
                case PlatformID.WinCE:
                    os = "Windows CE";
                    break;
#if !FEATURE_LEGACYNETCF
                case PlatformID.MacOSX:
                    os = "Mac OS X";
                    break;
#endif
                case PlatformID.Unix:
                    os = "Unix";
                    break;
                default:
                    os = "<unknown> ";
                    break;
            }

            if (Environment.Is64BitOperatingSystem && !Environment.Is64BitProcess)
            {
                wow = "; WOW64";
            }

            return String.Format("{0} {1}{2}", os, Environment.OSVersion.Version.ToString(2), wow);
        }

        public override string ToString()
        {
            return _userAgent;
        }
    }
}
