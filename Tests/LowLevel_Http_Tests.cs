using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using nac.CurlHttpClient.model;

namespace Tests
{
    [TestClass]
    public class LowLevel_Http_Tests
    {

        
        [TestMethod]
        public void get()
        {
            var http = new nac.CurlHttpClient.http(new HttpSetup()
            {
                onNewHttpResponse = (_response) =>
                {
                    Console.WriteLine(_response);
                }
            });
            var result = http.get("http://httpbin.org/ip");
        }
    }
}
