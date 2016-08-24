using System.IO;
using System.Net;

namespace MTC.Core
{
    public static class WebRequestCore
    {
        public static string GetUrlContext(string url)
        {
            WebRequest webRequest = null;            

            try
            {
                webRequest = WebRequest.Create(url);

                using (StreamReader reader = new StreamReader(webRequest.GetResponse().GetResponseStream()))
                    return reader.ReadToEnd();
            }
            catch 
            {

            }

            return string.Empty;
        }
    }
}
