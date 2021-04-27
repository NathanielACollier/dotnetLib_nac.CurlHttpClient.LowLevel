using System;
using System.Collections.Generic;
using nac.CurlHttpClient.model;
using nac.CurlThin.Enums;
using curl = nac.CurlThin.CurlNative.Easy;

namespace nac.CurlHttpClient
{
    public class http
    {

        private Dictionary<string, string> headers = new Dictionary<string, string>();
        private Dictionary<string, string> headersNextCall = new Dictionary<string, string>();

        private model.HttpSetup options;

        public http(model.HttpSetup __options=null)
        {
            if (__options == null)
            {
                this.options = new HttpSetup();
            }
            else
            {
                this.options = __options;
            }

            var globalCurlInitResult = nac.CurlThin.CurlNative.Init(); // this gets called once, but we init curl easy every request
        }


        public http useProxy(string host, int port)
        {
            this.options.useProxy = true;
            this.options.proxyHost = host;
            this.options.proxyPort = port;
            return this;
        }


        private void curlSetupAuthentication(nac.CurlThin.SafeHandles.SafeEasyHandle curlHandle)
        {
            if (this.options.useKerberosAuthentication)
            {
                // kerberos auth
                curl.SetOpt(curlHandle, CURLoption.HTTPAUTH, CURLAUTH.GSSNEGOTIATE);
                curl.SetOpt(curlHandle, CURLoption.USERPWD, ":");
            }
            else if( !string.IsNullOrWhiteSpace(this.options.user))
            {
                // basic auth
                curl.SetOpt(curlHandle, CURLoption.HTTPAUTH, CURLAUTH.BASIC);
                curl.SetOpt(curlHandle, CURLoption.USERPWD, this.options.getUserPasswordCurlOptValue());
            }
        }
        
        
        
        
    }
}
