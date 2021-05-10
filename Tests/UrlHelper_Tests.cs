using Microsoft.VisualStudio.TestTools.UnitTesting;
using urlHelper =  nac.CurlHttpClient.LowLevel.lib.urlHelper;

namespace Tests
{
    [TestClass]
    public class UrlHelper_Tests
    {



        [TestMethod]
        public void TestUrlCombine()
        {
            
            Assert.IsTrue(urlHelper.UrlCombine(null,null) == "");
            Assert.IsTrue(urlHelper.UrlCombine("test1", "test2") == "test1/test2");
            Assert.IsTrue(urlHelper.UrlCombine("test1/", "test2") == "test1/test2");
            Assert.IsTrue(urlHelper.UrlCombine("test1", "/test2") == "test1/test2");
            Assert.IsTrue(urlHelper.UrlCombine("test1/", "/test2") == "test1/test2");
            Assert.IsTrue(urlHelper.UrlCombine("/test1/", "/test2/") == "/test1/test2/");
            Assert.IsTrue(urlHelper.UrlCombine("", "/test2/") == "/test2/");
            Assert.IsTrue(urlHelper.UrlCombine("/test1/", "") == "/test1/");
            Assert.IsTrue(urlHelper.UrlCombine("https://www.google.com/      ", null) == "https://www.google.com/");
            
        }
    }
}