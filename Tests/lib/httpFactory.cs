using System;
using nac.CurlHttpClient.LowLevel.model;

namespace Tests.lib
{
    public class httpFactory
    {

        public static nac.CurlHttpClient.LowLevel.http createHttp()
        {
            var http = new nac.CurlHttpClient.LowLevel.http(new HttpSetup()
            {
                onNewHttpResponse = (_response) =>
                {
                    Console.WriteLine(_response);
                }
            });
            return http;
        }



        public static nac.CurlHttpClient.LowLevel.http createHttp_BaseUrlHttpBinOrg()
        {
            var http = new nac.CurlHttpClient.LowLevel.http(new HttpSetup()
            {
                baseAddress = "http://httpbin.org",
                onNewHttpResponse = (_response) =>
                {
                    Console.WriteLine(_response);
                }
            });
            return http;
        }
                
    }
}