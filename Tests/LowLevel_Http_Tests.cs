using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using nac.CurlHttpClient.model;

namespace Tests
{
    [TestClass]
    public class LowLevel_Http_Tests
    {

        private nac.CurlHttpClient.http createHttp()
        {
            var http = new nac.CurlHttpClient.http(new HttpSetup()
            {
                onNewHttpResponse = (_response) =>
                {
                    Console.WriteLine(_response);
                }
            });
            return http;
        }
        
        [TestMethod]
        public void get()
        {
            var http = createHttp();
            var result = http.get("http://httpbin.org/ip");
        }


        [TestMethod]
        public void post()
        {
            var http = createHttp();
            var result = http.post("http://httpbin.org/post",
                requestBody: "fieldname1=fieldvalue1&fieldname2=fieldvalue2");
        }
    }
}
