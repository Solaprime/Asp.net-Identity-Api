using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.netIdentityApi.ConfigurationObject
{
    public class ExternalServicesConfig
    {
        public string Url { get; set; }
        public int MinsToCache { get; set; }
    }
}

// we are binding two object to this
//We use named option for this
