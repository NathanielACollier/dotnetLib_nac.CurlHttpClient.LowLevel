﻿using System;
using System.Collections.Generic;
using nac.CurlHttpClient.model;
using nac.CurlThin.Enums;
using curl = nac.CurlThin.CurlNative.Easy;
using CurlHandleType = nac.CurlThin.SafeHandles.SafeEasyHandle;

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
            this.clearHeaders();
            this.transferHeadersForNextCall();

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


        public http addHeaderForNextCall(string key, string value)
        {
            this.headersNextCall.Add(key,value);
            return this;
        }

        private void transferHeadersForNextCall()
        {
            foreach (var pair in this.headersNextCall)
            {
                this.headers.Add(pair.Key, pair.Value);
            }
            this.headersNextCall.Clear();
        }

        private void clearHeaders()
        {
            this.headers.Clear();
        }

        private bool isAbsoluteUrl(string url)
        {
            url = url?.Trim() ?? "";

            return System.Text.RegularExpressions.Regex.IsMatch(url, "^https?://");
        }
    }
}