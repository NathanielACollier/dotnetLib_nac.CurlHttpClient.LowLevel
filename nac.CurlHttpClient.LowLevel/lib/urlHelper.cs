namespace nac.CurlHttpClient.LowLevel.lib
{
    public static class urlHelper
    {
        
        /*
         Original for: https://stackoverflow.com/questions/372865/path-combine-for-urls
         */
        public static string UrlCombine(string url1, string url2)
        {
            // get rid of any whitespace, and turn null into empty string
            url1 = url1?.Trim() ?? "";
            url2 = url2?.Trim() ?? "";
            
            // see if either string is completely empty
            if (url1.Length == 0) {
                return url2;
            }

            if (url2.Length == 0) {
                return url1;
            }


            url1 = url1.TrimEnd('/', '\\');
            url2 = url2.TrimStart('/', '\\');

            return string.Format("{0}/{1}", url1, url2);
        }
        
        
    }
}