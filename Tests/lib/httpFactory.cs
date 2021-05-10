using System;
using nac.CurlHttpClient.LowLevel.model;

namespace Tests.lib
{
    public class httpFactory
    {
        public static nac.CurlHttpClient.LowLevel.http create(Action<nac.CurlHttpClient.LowLevel.model.HttpSetup> onSetup=null)
        {
            var options = new nac.CurlHttpClient.LowLevel.model.HttpSetup()
            {
                onNewHttpResponse = (_curlResult) =>
                {
                    System.Diagnostics.Debug.WriteLine(_curlResult.ToString());
                }
            };

            onSetup?.Invoke(options);

            var http = new nac.CurlHttpClient.LowLevel.http(options);
            return http;
        }



        public static nac.CurlHttpClient.LowLevel.http createHttp_BaseUrlHttpBinOrg()
        {
            return create(options =>
            {
                options.baseAddress = "http://httpbin.org";
            });
        }
                
    }
}