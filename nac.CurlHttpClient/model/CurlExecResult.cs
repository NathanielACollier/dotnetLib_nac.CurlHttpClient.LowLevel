using System.Collections.Generic;
using System.IO;
using nac.CurlThin.Enums;

namespace nac.CurlHttpClient.model
{
    public class CurlExecResult
    {
        public string RequestUrl { get; set; }
        public MemoryStream ResponseStream { get; set; }
        public CURLcode ResponseCode { get; set; }

        public Dictionary<string, string> RequestHeaders { get; set; } // need it for logging, and callback type stuff
    }
}