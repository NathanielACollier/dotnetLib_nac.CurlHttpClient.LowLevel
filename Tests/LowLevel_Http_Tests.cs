using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using nac.CurlHttpClient.LowLevel.model;

namespace Tests
{
    [TestClass]
    public class LowLevel_Http_Tests
    {



        [TestMethod]
        public void get()
        {
            var http = lib.httpFactory.create();
            var result = http.get("http://httpbin.org/ip");
        }


        [TestMethod]
        public void get_testBaseAddress()
        {
            var http = lib.httpFactory.createHttp_BaseUrlHttpBinOrg();
            var result = http.get("ip");
            
            Console.WriteLine(result);
        }


        [TestMethod]
        public void post()
        {
            var http = lib.httpFactory.create();
            var result = http.post("http://httpbin.org/post",
                requestBody: "fieldname1=fieldvalue1&fieldname2=fieldvalue2");
        }


        [TestMethod]
        public void testReallyShortTimeout()
        {
            
        }
        
        
    }
}
