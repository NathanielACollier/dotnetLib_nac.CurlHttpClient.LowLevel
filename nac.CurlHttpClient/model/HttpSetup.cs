using System;
using System.Collections.Generic;

namespace nac.CurlHttpClient.model
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

        /*
         Use this to modify headers on every call.  Could have other uses later
         */
        public delegate void onNewHttpRequestDelegate(Dictionary<string, string> headers);
        
        public onNewHttpRequestDelegate onNewHttpRequest { get; set; }

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
            if (url.Substring(-1) != "/")
            {
                url += "/";
            }

            return url + relativeUrl;
        }
        
    }
}