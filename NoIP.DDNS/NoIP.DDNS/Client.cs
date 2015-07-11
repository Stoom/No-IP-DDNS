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
        public UserAgent UserAgent { get; set; }
        public Boolean IsRegistered {
            get { return !String.IsNullOrWhiteSpace(Id) && !String.IsNullOrWhiteSpace(Key); }
        }
        public String Id { get; set; }
        public String Key { get; set; }

        public Client(UserAgent userAgent)
        {
            if (userAgent == null)
                throw new ArgumentNullException("userAgent");

            UserAgent = userAgent;
        }
    }
}
