using System;
using System.Collections.Generic;
using System.Linq;

namespace nac.CurlHttpClient.LowLevel.model
{
    public class HttpSetup
    {
        public string user { get; set; }
        public string password { get; set; }
        public bool useKerberosAuthentication { get; set; }
        public bool useProxy { get; set; }
        public string proxyHost { get; set; }
        public int proxyPort { get; set;  }
        public string baseAddress { get; set; }
        public TimeSpan? Timeout { get; set; }

        /*
         Use this to modify headers on every call.  Could have other uses later
         */
        public delegate void onNewHttpRequestDelegate(Dictionary<string, string> headers);
        
        public onNewHttpRequestDelegate onNewHttpRequest { get; set; }

        public delegate void onNewHttpResponseDelegate(model.CurlExecResult response);
        public onNewHttpResponseDelegate onNewHttpResponse { get; set; }

        public HttpSetup()
        {
            this.useKerberosAuthentication = false;
            this.useProxy = false;
        }

        public string getProxyCurlOptValue()
        {
            return $"{this.proxyHost}:{this.proxyPort}";
        }

        public string getUserPasswordCurlOptValue()
        {
            return $"{this.user}:{this.password}";
        }


        public string appendToBaseAddress(string relativeUrl)
        {
            var url = this.baseAddress?.Trim() ?? "";
            // if last character is not forward slash, then append forward slash
            if (url.Last() != '/')
            {
                url += "/";
            }

            return url + relativeUrl;
        }
        
    }
}