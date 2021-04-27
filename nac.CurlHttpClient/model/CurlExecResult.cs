using System.IO;
using nac.CurlThin.Enums;

namespace nac.CurlHttpClient.model
{
    public class CurlExecResult
    {
        public string RequestUrl { get; set; }
        public MemoryStream ResponseStream { get; set; }
        public CURLcode ResponseCode { get; set; }
    }
}