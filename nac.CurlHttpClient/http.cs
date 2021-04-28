using System;
using System.Collections.Generic;
using System.Linq;
using nac.CurlHttpClient.model;
using nac.CurlThin.Enums;
using nac.CurlThin.SafeHandles;
using curl = nac.CurlThin.CurlNative.Easy;
using CurlHandleType = nac.CurlThin.SafeHandles.SafeEasyHandle;

namespace nac.CurlHttpClient
{
    public class http
    {
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


        private void curlSetupAuthentication(CurlHandleType curlHandle)
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

        private void curlSetupCookies(CurlHandleType curlHandle)
        {
            // cookies, got a bunch of info from this place on how to do cookies: http://stackoverflow.com/questions/13020404/keeping-session-alive-with-curl-and-php
            
            //string cookieFilePath = "/tmp/cookie.txt";
            string cookieFilePath = "";
            curl.SetOpt(curlHandle, CURLoption.COOKIEJAR, cookieFilePath);
            curl.SetOpt(curlHandle, CURLoption.COOKIEFILE, cookieFilePath);
            curl.SetOpt(curlHandle, CURLoption.COOKIESESSION, 1);
        }


        private void curlSetupSSLVerification(CurlHandleType curlHandle)
        {
            // for now don't do any ssl verification, later we could make this an option or something
            curl.SetOpt(curlHandle, CURLoption.SSL_VERIFYHOST, 0);
            curl.SetOpt(curlHandle, CURLoption.SSL_VERIFYPEER, 0);
        }


        private CurlHandleType curlSetup()
        {
            var curlHandle = curl.Init();

            curl.SetOpt(curlHandle, CURLoption.USERAGENT,
                "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.8.1.1) Gecko/20061204 Firefox/2.0.0.1");
            curl.SetOpt(curlHandle, CURLoption.FOLLOWLOCATION, 1);

            if (this.options.useProxy)
            {
                curl.SetOpt(curlHandle, CURLoption.PROXY, this.options.getProxyCurlOptValue());
            }
            
            this.curlSetupAuthentication(curlHandle);
            //this.curlSetupCookies(curlHandle);
            this.curlSetupSSLVerification(curlHandle);

            return curlHandle;
        }
        
        private bool isAbsoluteUrl(string url)
        {
            url = url?.Trim() ?? "";

            return System.Text.RegularExpressions.Regex.IsMatch(url, "^https?://");
        }

        private nac.CurlThin.SafeHandles.SafeSlistHandle curlSetHeader(CurlHandleType curlHandle,
                                                                    Dictionary<string,string> headers)
        {
            // followed documentation here: https://curl.se/libcurl/c/CURLOPT_HTTPHEADER.html
            if (headers?.Any() == true)
            {
                nac.CurlThin.SafeHandles.SafeSlistHandle list = SafeSlistHandle.Null;

                foreach (var pair in headers)
                {
                    list = nac.CurlThin.CurlNative.Slist.Append(list, $"{pair.Key}: {pair.Value}");
                }

                curl.SetOpt(curlHandle, CURLoption.HTTPHEADER, list.DangerousGetHandle());

                return list; // list will need to be freed
            }

            return null;
        }


        private model.CurlExecResult execCurl(CurlHandleType curlHandle, string url, Dictionary<string,string> headers=null)
        {
            var result = new model.CurlExecResult();
            if (!this.isAbsoluteUrl(url))
            {
                url = this.options.appendToBaseAddress(url);
            }
            
            // we know the final URL here
            result.RequestUrl = url;
            
            
            /*
             people may need to modify headers on every call
               + like setting an oauth Authorization header
             */
            if (this.options.onNewHttpRequest != null)
            {
                /*
                 make sure headers is not null
                  + If they specified a onNewHttpRequest callback they probably want to modify headers, so it needs to be non null
                  + Normally it's fine for headers to be null, so just set it if they wanted to get a onNewHttpRequest callback
                 */
                if (headers == null)
                {
                    headers = new Dictionary<string, string>();
                }
                
                this.options.onNewHttpRequest.Invoke(headers: headers);
            }
            
            // all the request headers should be set by this point
            result.RequestHeaders = headers;
            var headerListHandle = this.curlSetHeader(curlHandle, headers);
            curl.SetOpt(curlHandle, CURLoption.URL, url);
            
            result.ResponseStream = new System.IO.MemoryStream();
            curl.SetOpt(curlHandle, CURLoption.WRITEFUNCTION, (data, size, nmemb, user) =>
            {
                var length = (int) size * (int) nmemb;
                var buffer = new byte[length];
                System.Runtime.InteropServices.Marshal.Copy(data, buffer, 0, length);
                result.ResponseStream.Write(buffer, 0, length);
                return (UIntPtr) length;
            });

            result.ResponseCode = curl.Perform(curlHandle);
            
            // free up some stuff
            if (headerListHandle != null)
            {
                headerListHandle.Dispose();
            }
            
            // give back result
            return result;
        }


        public model.CurlExecResult get(string url="", Dictionary<string, string> headers = null)
        {
            var curlHandle = this.curlSetup();

            var result = this.execCurl(curlHandle, url, headers);
            
            // Trigger OnNewHttpResponse
            // + also fill in some other stuff
            result.RequestMethod = "GET";
            result.setupOptions = this.options;
            this.options.onNewHttpResponse?.Invoke(result);
            
            // end things out
            curlHandle.Dispose();
            return result;
        }

        public model.CurlExecResult post(string url = "", Dictionary<string, string> headers = null, string requestBody="")
        {
            var curlHandle = this.curlSetup();

            curl.SetOpt(curlHandle, CURLoption.POST, 1);

            if (!string.IsNullOrEmpty(requestBody))
            {
                if (headers == null)
                {
                    headers = new Dictionary<string, string>();
                }
                curl.SetOpt(curlHandle, CURLoption.POSTFIELDS, requestBody);
                headers.Add("Content-Length",requestBody.Length.ToString());
            }

            var result = this.execCurl(curlHandle, url, headers);
            
            // Trigger OnNewHttpResponse
            // + also fill in some other stuff
            result.RequestMethod = "POST";
            result.RequestBody = requestBody;
            result.setupOptions = this.options;
            this.options.onNewHttpResponse?.Invoke(result);
            
            // end things out
            curlHandle.Dispose();
            return result;
        }
        
        
    }
}
