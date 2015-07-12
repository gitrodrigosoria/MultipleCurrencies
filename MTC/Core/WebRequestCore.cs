using System;
using System.IO;
using System.Net;

namespace MTC.Core
{
    public static class WebRequestCore
    {
        public static String getUrlContext(String url)
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

            return String.Empty;
        }
    }
}
