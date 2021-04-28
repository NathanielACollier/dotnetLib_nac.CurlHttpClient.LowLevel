using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class LowLevel_Http_Tests
    {
        [TestMethod]
        public void get()
        {
            var http = new nac.CurlHttpClient.http();
            var result = http.get("http://httpbin.org/ip");
        }
    }
}
