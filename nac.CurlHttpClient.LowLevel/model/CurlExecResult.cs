using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using nac.CurlThin.Enums;

namespace nac.CurlHttpClient.LowLevel.model
{
    public class CurlExecResult
    {
        public string RequestUrl { get; set; }
        public MemoryStream ResponseStream { get; set; }
        public CURLcode ResponseCode { get; set; }

        public Dictionary<string, string> RequestHeaders { get; set; } // need it for logging, and callback type stuff

        public string RequestBody { get; set; }
        
        public string RequestMethod { get; set; }
        
        public model.HttpSetup setupOptions { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("###### CURL ######");
            sb.AppendLine("Request:");
            sb.AppendLine("-----------------");
            sb.AppendLine(this.buildCurlCommand());
            sb.AppendLine("-----------------");
            sb.AppendLine($"Response: ({this.ResponseCode})");
            sb.AppendLine("-----------------");
            sb.AppendLine(this.readResponseAsString());
            sb.AppendLine("-----------------");

            return sb.ToString();
        }

        public string readResponseAsString()
        {
            if (this.ResponseStream != null && this.ResponseStream.Length > 0)
            {
                // make sure stream is reset
                this.ResponseStream.Seek(0, SeekOrigin.Begin);
                var responseBody = System.Text.Encoding.UTF8.GetString(this.ResponseStream.ToArray());
                // reset the stream again
                this.ResponseStream.Seek(0, SeekOrigin.Begin);
                return responseBody;
            }

            return string.Empty;
        }
        
        
        public string buildCurlCommand()
        {
            var sb = new StringBuilder();
            sb.Append($"curl -X {this.RequestMethod}");

            if (this.RequestHeaders?.Any() == true)
            {
                sb.Append(" -H ");
                sb.Append(string.Join(",", this.RequestHeaders.Select(pair => $"\"{pair.Key}: {pair.Value}\"")));
            }

            if (!string.IsNullOrWhiteSpace(this.RequestBody))
            {
                sb.Append($" -d '{this.RequestBody}'");
            }

            sb.Append($" {this.RequestUrl}");

            if (this.setupOptions.useKerberosAuthentication)
            {
                sb.Append(" --negotiate -u : ");
            }

            return sb.ToString();
        }
        
    }
}