using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreOpenTelemetryTest
{
    public class OpenTelemetryOptions
    {
        public bool Enabled { get; set; }
        public string ServiceName { get; set; }
        public string Sampler { get; set; }
        public string ZipkinEndpoint { get; set; }
    }
}
